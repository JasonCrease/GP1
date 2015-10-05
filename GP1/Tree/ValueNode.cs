using System;

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

        internal override Node Simplify()
        {
            return this;
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
