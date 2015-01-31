﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP1.Tree
{
    public class FunctionNode : Node
    {
        private Node[] m_ChildNodes;
        private Func m_Function;

        internal Node[] Children { get { return m_ChildNodes; } }
        internal Func Function { get { return m_Function; } }

        public FunctionNode(Node[] childNodes, Func function)
        {
            m_ChildNodes = childNodes;
            m_Function = function;
        }

        public override int Evaluate()
        {
            return m_Function.Evaluate(m_ChildNodes);
        }
    }
}
