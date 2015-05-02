﻿using System;
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
        private const int TARGETPOPULATION = 500;
        private const float MUTATIONRATE = 0.02f;
        private const float REPRODUCTIONRATE = 0.1f;

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

        public void Run()
        {
            m_Progs = GetPopulation(TARGETPOPULATION);
            int numToMutate = (int)((float)TARGETPOPULATION * MUTATIONRATE);
            int numParents = (int)((float)TARGETPOPULATION * REPRODUCTIONRATE * 2f);

            for (m_Gen = 0; m_Gen < MAXGENERATIONS; m_Gen++)
            {
                lock (m_LockObject)
                {
                    UpdateFitnesses();

                    CloneBestPrograms(5);
                    Program[] selectedParents = SelectParents(numParents);
                    GenerateOffspring(selectedParents);
                    AddMutatedPrograms(numToMutate);

                    UpdateFitnesses();
                    ManagePopulation();
                }
            }

            UpdateFitnesses();
            m_EvolutionDone.Invoke(this, null);
        }

        private void UpdateFitnesses()
        {
            foreach (Program p in m_Progs)
            {
                if(p.FitnessIsDirty)
                    p.Fitness = FitnessFunction.Evaluate(p);
            }
        }

        private Program[] SelectParents(int parentsToSelect)
        {
            Program[] selectedParents = new Program[parentsToSelect];
            Program[] orderedPrograms = m_Progs.OrderByDescending(x => x.Fitness).ToArray();
            int totalParents = orderedPrograms.Length;

            // Pick parents randomly with a squared density, so better programs are more likely to be picked
            for (int i = 0; i < parentsToSelect; i++)
            {
                int parentToPick = (int)(s_Random.NextDouble() * s_Random.NextDouble() * totalParents);
                selectedParents[i] = orderedPrograms[parentToPick];
            }

            return selectedParents;
        }

        private void CloneBestPrograms(int numToClone)
        {
            var progsToClone = m_Progs.OrderBy(x => x.Fitness).Take(numToClone);
            foreach (Program progToClone in progsToClone)
                m_Progs.Add(progToClone.CloneProgram());
        }

        private void GenerateOffspring(Program[] parents)
        {
            for (int parentNum = 0; parentNum < parents.Length; parentNum++)
            {
                Program parent1 = parents[parentNum];
                parentNum++;
                Program parent2 = parents[parentNum];
                Program child = parent1.Crossover(parent2);
                m_Progs.Add(child);
            }
        }

        private void AddMutatedPrograms(int numToMutate)
        {
            int popSize = m_Progs.Count();
            for (int i = 0; i < numToMutate; i++)
            {
                Program progToMutate = m_Progs[s_Random.Next(popSize)];
                Program newProgram = progToMutate.CloneProgram();
                newProgram.Mutate();
                m_Progs.Add(newProgram);
            }
        }

        /// <summary>
        /// Kill off bad programs or add programs as necessary
        /// </summary>
        private void ManagePopulation()
        {
            int numToKill = 5;
            int numToCreate = 5;
            int popDiff = m_Progs.Count() - TARGETPOPULATION;

            if (popDiff > 0) numToKill += popDiff;
            if (popDiff < 0) numToCreate -= popDiff;

            var programsToKill = m_Progs.OrderByDescending(x => x.Fitness).Take(numToKill);

            foreach (Program programToKill in programsToKill)
                m_Progs.Remove(programToKill);

            m_Progs.AddRange(GetPopulation(numToCreate));
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
                new Tree.FuncIf(Tree.Comparator.GreaterThan), new Tree.FuncIf(Tree.Comparator.Equal), new Tree.FuncIf(Tree.Comparator.GreaterThanOrEqual),
               // new Tree.FuncAnd(), new Tree.FuncOr()
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
