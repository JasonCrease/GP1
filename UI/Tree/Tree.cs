using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Tree
{
    class Tree
    {
        public Node m_Root;

        public int Evaluate()
        {
            return m_Root.Evaluate();
        }
    }
}
