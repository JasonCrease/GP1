using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP1.Tree
{
    public class FuncNode : Node
    {
        private Node[] m_ChildNodes;
        private Func m_Function;

        internal Node[] Children { get { return m_ChildNodes; } }
        internal Func Function { get { return m_Function; } }

        internal override Node Simplify()
        {
            bool areAllValueNodes = true;

            // If all children are static values, just calculate this node and return
            for (int i = 0; i < m_ChildNodes.Length; i++)
            {
                m_ChildNodes[i] = m_ChildNodes[i].Simplify();
                if (!(m_ChildNodes[i] is ValueNode)) areAllValueNodes = false;
            }
            if (areAllValueNodes)
            {
                ValueNode thisNode = new ValueNode(this.Evaluate());
                return thisNode;
            }

            // If this is an if statement, check for conditions that are always true
            if (m_Function is FuncIf)
            {
                FuncIf funcAsIf = (FuncIf)m_Function;
                if (Children[0] is ValueNode && Children[1] is ValueNode)
                {
                    if (funcAsIf.Compare(Children[0], Children[1]))
                        return Children[2];
                    else
                        return Children[3];
                }

                if (Children[0] is VariableNode && Children[1] is VariableNode)
                {
                    if (Children[0] == Children[1])
                    {
                        if (funcAsIf.Comparator == Comparator.Equal || funcAsIf.Comparator == Comparator.GreaterThanOrEqual)
                            return Children[2];
                        else
                            return Children[3];
                    }
                }

            }


            return this;
        }

        public FuncNode(Node[] childNodes, Func function)
        {
            m_ChildNodes = childNodes;
            m_Function = function;
        }

        public override Node CloneTree()
        {
            Node[] childNodes = new Node[m_ChildNodes.Length];

            for (int i = 0; i < childNodes.Length; i++)
                childNodes[i] = m_ChildNodes[i].CloneTree();

            return new FuncNode(childNodes, this.m_Function);
        }

        public override int Evaluate()
        {
            return m_Function.Evaluate(m_ChildNodes);
        }
    }
}
