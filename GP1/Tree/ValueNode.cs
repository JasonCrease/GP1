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
        internal int Value { get { return m_Value; } }

        public ValueNode(int value)
        {
            m_Value = value;
        }

        public override Node CloneTree()
        {
            return new ValueNode(m_Value);
        }

        public override int Evaluate()
        {
            return m_Value;
        }
    }
}
