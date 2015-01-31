using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP1.Tree
{
    public class Variable
    {
        int m_Value;
        string m_Name;

        public Variable(string name, int value)
        {
            m_Name = name;
            m_Value = value;
        }

        public int Value
        {
            get
            {
                return this.m_Value;
            }
            set
            {
                this.m_Value = value;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }
    }
}
