using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GP1
{
    public class Engine
    {
        public static Random s_Random = new Random();
        public IFitnessFunction FitnessFunction { get { return m_FitnessFunction; } }
        public IFitnessFunction m_FitnessFunction;

        private Tree.Variable[] m_Variables;
        private Tree.Func[] m_Functions;
        private int[] m_Values;
        private List<Program> m_Progs;

        private const int MAXGENERATIONS = 50000;
        private const int TARGETPOPULATION = 100;
        private const float MUTATIONRATE = 0.4f;
        private const float CROSSOVERRATE = 0.1f;

        private Thread m_RunThread;
        private event EventHandler m_EvolutionDone;

        private int m_Gen = 0;
        public int CurrentGeneration
        {
            get { return m_Gen; }
        }

        public void RunAsync(EventHandler evolutionDone)
        {
            m_EvolutionDone = evolutionDone;
            m_RunThread = new Thread(Run);
            m_RunThread.IsBackground = true;
            m_RunThread.Start();
        }

        private object m_LockObject = new object();

        public void UpdatePopulationStatistics()
        {
            m_PopulationStatistics = new PopulationStatistics(m_Progs);
        }

        private PopulationStatistics m_PopulationStatistics;
        public PopulationStatistics PopulationStatistics { get { return m_PopulationStatistics; } }

        public void Run()
        {
            m_Progs = GetPopulation(TARGETPOPULATION);

            for (m_Gen = 0; m_Gen < MAXGENERATIONS; m_Gen++)
            {
                lock (m_LockObject)
                {
                    m_Progs = GenerateNextGeneration();
                    UpdateFitnesses();

                    if (m_Gen % 20 == 0)
                        UpdatePopulationStatistics();
                }
            }

            UpdateFitnesses();
            m_EvolutionDone.Invoke(this, null);
        }

        private List<Program> GenerateNextGeneration()
        {
            List<Program> nextGenPrograms = new List<Program>();
            int progsAdded = 0;

            Program[] orderedPrograms = m_Progs.OrderBy(x => x.Fitness).ToArray();
            int existingProgsCount = orderedPrograms.Length;

            const int elitism = 5;
            // Always reproduce best 5 programs
            for (int i = 0; i < elitism; i++)
            {
                nextGenPrograms.Add(orderedPrograms[i]);
                progsAdded++;
            }

            // Always add targetpop / 5 random programs
            nextGenPrograms.AddRange(GetPopulation(TARGETPOPULATION / 5));
            progsAdded += TARGETPOPULATION / 5;
            
            while (progsAdded < TARGETPOPULATION)
            {
                double operation = s_Random.NextDouble();

                if(operation < MUTATIONRATE)
                {
                    // Mutation
                    int progToMutateNum = (int)(s_Random.NextDouble() * s_Random.NextDouble() * existingProgsCount);
                    Program progToMutate = m_Progs[progToMutateNum].Mutate();
                    progToMutate.FitnessIsDirty = true;
                    nextGenPrograms.Add(progToMutate);
                    progsAdded++;
                }
                else if (operation < MUTATIONRATE + CROSSOVERRATE)
                {
                    // Crossover
                    int parent1 = (int)(s_Random.NextDouble() * s_Random.NextDouble() * existingProgsCount);
                    int parent2 = (int)(s_Random.NextDouble() * s_Random.NextDouble() * existingProgsCount);
                    Program childProgram = m_Progs[parent1].Crossover(m_Progs[parent2]);
                    childProgram.FitnessIsDirty = true;
                    nextGenPrograms.Add(childProgram);
                    progsAdded++;
                }
                else
                {
                    // Reproduction
                    int progToReproduce = (int)(s_Random.NextDouble() * s_Random.NextDouble() * existingProgsCount);
                    if (progToReproduce >= elitism)
                    {
                        nextGenPrograms.Add(m_Progs[progToReproduce]);
                        progsAdded++;
                    }
                }
            }

            return nextGenPrograms;
        }

        private void UpdateFitnesses()
        {
            foreach (Program p in m_Progs)
            {
                if(p.FitnessIsDirty)
                    p.Fitness = FitnessFunction.Evaluate(p);
            }
        }

        private List<Program> GetPopulation(int size)
        {
            List<Program> ps = new List<Program>();

            for (int i = 0; i < size; i++)
                ps.Add(Program.GenerateRandomProgram(this.m_Variables, this.m_Functions, this.m_Values));

            return ps;
        }

        public Engine(IFitnessFunction fitnessFunction)
        {
            m_FitnessFunction = fitnessFunction;

            m_Variables = new Tree.Variable[m_FitnessFunction.Variables.Length];
            for (int i = 0; i < m_FitnessFunction.Variables.Length; i++)
                m_Variables[i] = new Tree.Variable(m_FitnessFunction.Variables[i], -1000);

            m_Functions = new Tree.Func[] { 
                //new Tree.FuncMultiply(), 
                new Tree.FuncAdd(), 
                //new Tree.FuncModulo(), 
                new Tree.FuncSubtract(), 
                //new Tree.FuncMax(), 
                new Tree.FuncIf(Tree.Comparator.GreaterThan), 
                new Tree.FuncIf(Tree.Comparator.Equal), 
                //new Tree.FuncIf(Tree.Comparator.GreaterThanOrEqual),
                new Tree.FuncAnd(), 
                new Tree.FuncOr()
            };
            m_Values = new int[] { 0, 1, 2, 3 };
        }

        public Program CreateRandomProgram()
        {
            Program p = Program.GenerateRandomProgram(m_Variables, m_Functions, m_Values);
            return p;
        }

        public Program GetStrongestProgram()
        {
            Program best = null;

            lock (m_LockObject)
            {
                UpdateFitnesses();
                best = m_Progs.OrderBy(x => x.Fitness).First();
                best.TopNode.Simplify();
            }
            
            return best;
        }


        public Program getRandomProgram()
        {
            Program best = null;
            Random r = new Random();

            lock (m_LockObject)
            {
                best = m_Progs[r.Next(TARGETPOPULATION)];
            }

            return best;
        }
    }
}
