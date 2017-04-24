using System;
using System.Collections.Generic;
using System.Drawing;
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

        public const int MAXGENERATIONS = 20000;
        public const int TARGETPOPULATION = 500;
        public const float MUTATIONRATE = 0.2f;
        public const float CROSSOVERRATE = 0.2f;
        public const int RANDOMPROGSTOADD = TARGETPOPULATION / 20;
        private readonly int MAXTREESIZE = 120;
        public const double TOURNAMENT_SELECTION_P = (1d / TARGETPOPULATION) * 1.0f; // exponential p

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
        public void Stop()
        {
            m_EvolutionDone.BeginInvoke(this, null, null, null);
            m_RunThread.Abort();
        }

        private object m_LockObject = new object();

        public void UpdatePopulationStatistics()
        {
            m_PopulationStatistics = new PopulationStatistics(m_Progs);
        }

        private PopulationStatistics m_PopulationStatistics;
        public PopulationStatistics PopulationStatistics { get { return m_PopulationStatistics; } }

        public void Pause()
        {
            m_Paused = true;
        }
        public void UnPause()
        {
            m_Paused = false;
        }

        public void Run()
        {
            m_Progs = GetPopulation(TARGETPOPULATION);

            for (m_Gen = 0; m_Gen < MAXGENERATIONS && 
                GetStrongestProgram().Fitness > m_FitnessFunction.TerminatingFitness;
                m_Gen++)
            {
                lock (m_LockObject)
                {
                    m_Progs = GenerateNextGeneration();
                    UpdateFitnesses();

                    if (m_Gen % 20 == 0)
                        UpdatePopulationStatistics();
                }

                while (m_Paused) {
                    Thread.Sleep(1000);
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

            const int elitism = 1;
            // Always reproduce best 1 programs
            for (int i = 0; i < elitism; i++)
            {
                nextGenPrograms.Add(orderedPrograms[i]);
                progsAdded++;
            }

            // Always add targetpop / 10 random programs
            nextGenPrograms.AddRange(GetPopulation(TARGETPOPULATION / RANDOMPROGSTOADD));
            progsAdded += TARGETPOPULATION / RANDOMPROGSTOADD;
            
            while (progsAdded < TARGETPOPULATION)
            {
                Program programToAdd = null;
                double operation = s_Random.NextDouble();

                if(operation < MUTATIONRATE)
                {
                    // Mutation
                    int progToMutateNum = SelectFrom(existingProgsCount);
                    Program mutatedProgram = orderedPrograms[progToMutateNum].Mutate();
                    mutatedProgram.FitnessIsDirty = true;
                    programToAdd = mutatedProgram;
                }
                else if (operation < MUTATIONRATE + CROSSOVERRATE)
                {
                    // Crossover
                    int parent1 = SelectFrom(existingProgsCount);
                    int parent2 = SelectFrom(existingProgsCount);
                    Program childProgram = orderedPrograms[parent1].Crossover(orderedPrograms[parent2]);
                    childProgram.FitnessIsDirty = true;
                    m_ExampleParent1 = orderedPrograms[parent1];
                    m_ExampleParent2 = orderedPrograms[parent2];
                    m_ExampleChild = childProgram;
                    programToAdd = childProgram;
                }
                else
                {
                    // Reproduction
                    int progToReproduce = SelectFrom(existingProgsCount);
                    if (progToReproduce >= elitism)
                    {
                        programToAdd = orderedPrograms[progToReproduce];
                    }
                }

                if (programToAdd != null)
                {
                    if (s_Random.NextDouble() < 0.1)
                        programToAdd.TopNode = programToAdd.TopNode.Simplify();
                    if (programToAdd.TreeSize < MAXTREESIZE)
                    { 
                        progsAdded++;
                        nextGenPrograms.Add(programToAdd);
                    }
                }
            }

            return nextGenPrograms;
        }

        double LOG_1_OVER_TOURNAMENT_SELECTION_P = Math.Log(1 - TOURNAMENT_SELECTION_P);

        private int SelectFrom(int existingProgsCount)
        {
            double x = 0;
            double r = 0;

            do
            {
                r = s_Random.NextDouble();
                x = Math.Log(1 - r) / LOG_1_OVER_TOURNAMENT_SELECTION_P;
            } while (x > existingProgsCount);

            return (int)x;
        }

        private void UpdateFitnesses()
        {
            foreach (Program p in m_Progs)
            {
                if (p.FitnessIsDirty)
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
                //new Tree.FuncAdd(), 
                //new Tree.FuncModulo(), 
                new Tree.FuncSubtract(), 
                //new Tree.FuncMax(), 
                new Tree.FuncIf(Tree.Comparator.GreaterThan), 
                new Tree.FuncIf(Tree.Comparator.Equal), 
                //new Tree.FuncIf(Tree.Comparator.GreaterThanOrEqual),
                //new Tree.FuncAnd(), 
                //new Tree.FuncOr()
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
            }
            
            return best;
        }
        public Tuple<Program, Program, Program> GetProgramFamily()
        {
            return new Tuple<Program, Program, Program>(m_ExampleParent1, m_ExampleParent2, m_ExampleChild);
        }

        Program m_ExampleParent1;
        Program m_ExampleParent2;
        Program m_ExampleChild;

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

        static Pen LinePen = new Pen(Brushes.Red, 2);
        private bool m_Paused;

        public Bitmap DrawPopulationHistogram(float width, float height)
        {
            Bitmap bmp = new Bitmap((int)width, (int)height);
            Graphics g = Graphics.FromImage(bmp);

            lock (m_LockObject)
            {
                double[] orderedByFitness = m_Progs.OrderBy(x => x.Fitness).Select(x => x.Fitness).ToArray();
                double maxFitness = orderedByFitness.Last();

                for (int i = 0; i < height; i += 2)
                {
                    int k = (int)((i * TARGETPOPULATION) / height);
                    float wid = ((float)orderedByFitness[k] * (float)width) / (float)maxFitness;
                    g.DrawLine(LinePen, 0, i, wid, i);
                }
            }

            return bmp;
        }
    
    }
}
