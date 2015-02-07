using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP1
{
    public class Engine
    {
        public static Random s_Random = new Random();
        public IFitnessFunction FitnessFunction { get; set; }

        private Tree.Variable[] m_Variables;
        private Tree.Func[] m_Functions;
        private int[] m_Values;
        private List<Program> m_Progs;
        private const int MAXGENERATIONS = 100;
        private const int TARGETPOPULATION = 60;

        public void Run()
        {
            m_Progs = GetPopulation(TARGETPOPULATION);

            for(int gen=0; gen<MAXGENERATIONS; gen++)
            {
                List<Program> bestProgs = GetFittestPrograms(5);
                m_Progs.AddRange(GenerateOffspring(bestProgs));
                AddMutatedPrograms(5);
                RemoveWorstPrograms(15);
                FillPopulation();
            }
        }

        private void AddMutatedPrograms(int size)
        {
            int popSize = m_Progs.Count();
            for(int i=0; i<size; i++)
            {
                Program progToMutate = m_Progs[s_Random.Next(popSize)];
                Program newProgram = progToMutate.Clone();
                newProgram.Mutate();
                m_Progs.Add(newProgram);
            }
        }

        private void FillPopulation()
        {
            throw new NotImplementedException();
        }

        private void RemoveWorstPrograms(int size)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Program> GenerateOffspring(List<Program> parents)
        {
            throw new NotImplementedException();
        }

        private List<Program> GetFittestPrograms(int size)
        {
            throw new NotImplementedException();
        }

        private List<Program> GetPopulation(int size)
        {
            List<Program> ps = new List<Program>();

            for(int i=0; i<size; i++)
                ps.Add(Program.GenerateRandomProgram(this.m_Variables, this.m_Functions, this.m_Values));

            return ps;
        }

        public Engine()
        {
            m_Variables = new Tree.Variable[] { new Tree.Variable("N", -1000) };
            m_Functions = new Tree.Func[] { new Tree.FuncMultiply(), new Tree.FuncAdd(), new Tree.FuncModulo(), new Tree.FuncSubtract() };
            m_Values = new int[] { 0, 1 };
        }

        public Program CreateRandomProgram()
        {
            Program p = Program.GenerateRandomProgram(m_Variables, m_Functions, m_Values);
            return p;
        }

        public void CreateRandomNode(Program program)
        {
            throw new NotImplementedException();
        }
    }
}
