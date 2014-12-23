using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP1.Tree
{
    public abstract class Func
    {
        public abstract int Evaluate(Node[] childNodes);

        public abstract int NumberOfArguments
        {
            get;
        }

        /// <summary>
        /// Name of the function.
        /// </summary>
        public abstract string Name
        {
            get;
        }
    }

    public class FuncAdd : Func
    {
        public override int Evaluate(Node[] nextNodes)
        {
            return nextNodes[0].Evaluate() + nextNodes[1].Evaluate();
        }

        public override int NumberOfArguments
        {
            get
            {
                return 2;
            }
        }

        public override string Name
        {
            get { return "+"; }
        }
    }

    public class FuncMultiply : Func
    {
        public override int Evaluate(Node[] nextNodes)
        {
            return nextNodes[0].Evaluate() * nextNodes[1].Evaluate();
        }

        public override int NumberOfArguments
        {
            get
            {
                return 2;
            }
        }

        public override string Name
        {
            get { return "*"; }
        }
    }
}
