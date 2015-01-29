using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP1
{
    public class Program
    {
        private Tree.Node m_TopNode;
        private double m_Fitness;
        private float m_Result;

        public double Fitness
        {
            get { return m_Fitness; }
        }
        public float Result
        {
            get { return m_Result; }
        }

        public Program(Tree.Node[] possibleNodes)
        {
            
        }

        public void Run()
        {
            m_Result = m_TopNode.Evaluate();
        }
    }
}
