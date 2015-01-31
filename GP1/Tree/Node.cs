using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP1.Tree
{
    public abstract class Node
    {
        public abstract int Evaluate();

        public int ChildrenCount
        {
            get
            {
                int count = 0;

                if (this is Tree.FunctionNode)
                {
                    FunctionNode thisNode = this as FunctionNode;
                    foreach (Node n in thisNode.Children)
                    {
                        count += n.ChildrenCount;
                    }
                }
                else
                    count = 1;

                return count + 1;
            }
        }
    }
}
