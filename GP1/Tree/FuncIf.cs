using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP1.Tree
{
    public class FuncIf : Func
    {
        private Comparator m_Comparator;

        public Comparator Comparator
        {
            get { return m_Comparator; }
        }

        public FuncIf(Comparator comparator)
        {
            m_Comparator = comparator;
        }

        public override int Evaluate(Node[] nextNodes)
        {
            bool resultOfComparison = Compare(nextNodes[0], nextNodes[1]);

            if (resultOfComparison)
                return nextNodes[2].Evaluate();
            else
                return nextNodes[3].Evaluate();
        }

        public override int NumberOfArguments
        {
            get
            {
                return 4;
            }
        }

        public override string Name
        {
            get
            {
                if (m_Comparator == Comparator.Equal)
                    return "==";
                else if (m_Comparator == Comparator.GreaterThan)
                    return ">";
                else if (m_Comparator == Comparator.GreaterThanOrEqual)
                    return ">=";
                else
                    throw new ApplicationException();
            }
        }

        internal bool Compare(Node node0, Node node1)
        {
            if (m_Comparator == Comparator.Equal)
            {
                if (node0.Evaluate() == node1.Evaluate())
                    return true;
            }
            else if (m_Comparator == Comparator.GreaterThan)
            {
                if (node0.Evaluate() > node1.Evaluate())
                    return true;
            }
            else if (m_Comparator == Comparator.GreaterThanOrEqual)
            {
                if (node0.Evaluate() >= node1.Evaluate())
                    return true;
            }
            else
                throw new ApplicationException();

            return false;
        }
    }
}
