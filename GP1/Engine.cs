using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP1
{
    public class Engine
    {
        public static Random s_Random = new Random(1234);
        public IFitnessFunction FitnessFunction { get; set; }

        private Tree.Variable[] m_Variables;
        private Tree.Func[] m_Functions;
        private int[] m_Values;

        public void Run()
        {

        }

        public Engine()
        {
            m_Variables = new Tree.Variable[] { new Tree.Variable("X", 2) };
            m_Functions = new Tree.Func[] { new Tree.FuncMultiply(), new Tree.FuncAdd() };
            m_Values = new int[] { 0, 1, 2 };
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
