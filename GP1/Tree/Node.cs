using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP1.Tree
{
    public abstract class Node
    {
        public int Depth;
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

        internal Tree.Node RecurseToNodeNumber(int nodeToMutate, ref int currentNodeNum)
        {
            if(currentNodeNum == nodeToMutate)
                return this;
            else
            {
                currentNodeNum++;
                if (this is Tree.FunctionNode)
                {
                    FunctionNode thisNode = this as FunctionNode;
                    foreach (Node n in thisNode.Children)
                    {
                        Tree.Node posNode = RecurseToNodeNumber(nodeToMutate, ref currentNodeNum);
                        if (posNode != null) return posNode;
                    }
                }
            }

            return null;
        }
    }
}
