using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        private const int MAXGENERATIONS = 10000;
        private const int TARGETPOPULATION = 1000;
        private const float MUTATIONRATE = 0.01f;
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

                    if (m_Gen % 100 == 0)
                        m_PopulationStatistics = new PopulationStatistics(m_Progs);
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

            // Always reproduce best 2 programs
            for (int i = 0; i < 2; i++)
            {
                nextGenPrograms.Add(orderedPrograms[i]);
                progsAdded++;
            }

            // Always add 10 random programs
            nextGenPrograms.AddRange(GetPopulation(10));
            progsAdded += 10;
            

            while (progsAdded < TARGETPOPULATION)
            {
                double operation = s_Random.NextDouble();

                if(operation < MUTATIONRATE)
                {
                    // Mutation
                    int progToMutate = (int)(s_Random.NextDouble() * s_Random.NextDouble() * existingProgsCount);
                    nextGenPrograms.Add(m_Progs[progToMutate].Mutate());
                    progsAdded++;
                }
                else if (operation < MUTATIONRATE + CROSSOVERRATE)
                {
                    // Crossover
                    int parent1 = (int)(s_Random.NextDouble() * s_Random.NextDouble() * existingProgsCount);
                    int parent2 = (int)(s_Random.NextDouble() * s_Random.NextDouble() * existingProgsCount);
                    nextGenPrograms.Add(m_Progs[parent1].Crossover(m_Progs[parent2]));
                    progsAdded++;
                }
                else
                {
                    // Reproduction
                    int progToReproduce = (int)(s_Random.NextDouble() * s_Random.NextDouble() * existingProgsCount);
                    nextGenPrograms.Add(m_Progs[progToReproduce]);
                    progsAdded++;
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
                new Tree.FuncMultiply(), new Tree.FuncAdd(), new Tree.FuncModulo(), new Tree.FuncSubtract(), 
                //new Tree.FuncMax(), 
                //new Tree.FuncIf(Tree.Comparator.GreaterThan), 
                new Tree.FuncIf(Tree.Comparator.Equal), 
                //new Tree.FuncIf(Tree.Comparator.GreaterThanOrEqual),
                //new Tree.FuncAnd(), new Tree.FuncOr()
            };
            m_Values = new int[] { 0, 1, 2 };
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

    }
}
