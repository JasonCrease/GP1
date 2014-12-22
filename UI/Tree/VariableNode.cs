using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Tree
{
    class VariableNode : Node
    {
        Variable m_Variable;

        public VariableNode(Variable variable)
        {
            m_Variable = variable;
        }

        public override int Evaluate()
        {
            return m_Variable.Value;
        }
    }
}
