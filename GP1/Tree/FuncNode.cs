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

        internal Node GetSimplifiedNode()
        {
            m_ChildNodes[0].Simplify();
            m_ChildNodes[1].Simplify();

            return m_Function.Simplify(m_ChildNodes);
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

        internal override void Simplify()
        { throw new NotImplementedException();
        }

        public override int Evaluate()
        {
            return m_Function.Evaluate(m_ChildNodes);
        }
    }
}
