using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP1.Tree
{

    class ValueNode : Node
    {
        private int m_Value;

        public ValueNode(int value)
        {
            m_Value = value;
        }

        public override int Evaluate()
        {
            return m_Value;
        }
    }
}
