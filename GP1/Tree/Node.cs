﻿using System;

namespace GP1.Tree
{
    public abstract class Node
    {
        public int Depth;
        public abstract int Evaluate();
        public abstract Node CloneTree();

        // The total children under this node
        public int Treesize
        {
            get
            {
                int count = 1;

                if (this is Tree.FuncNode)
                {
                    FuncNode thisNode = this as FuncNode;
                    foreach (Node n in thisNode.Children)
                    {
                        count += n.Treesize;
                    }
                }
                else
                    return 1;

                return count;
            }
        }

        public int TreeSizeFunctionsOnly
        {
            get
            {
                int count = 1;

                if (this is Tree.FuncNode)
                {
                    FuncNode thisNode = this as FuncNode;
                    foreach (Node n in thisNode.Children)
                    {
                        count += n.TreeSizeFunctionsOnly;
                    }
                }
                else
                    return 0;

                return count;
            }
        }

        internal abstract Node Simplify();

        internal Tree.Node GetNodeNumber(int nodeToMutate, ref int currentNodeNum)
        {
            if (currentNodeNum == nodeToMutate)
                return this;
            else if (currentNodeNum > nodeToMutate)
                throw new ApplicationException();
            else
            {
                currentNodeNum++;
                if (this is Tree.FuncNode)
                {
                    FuncNode thisNode = this as FuncNode;
                    foreach (Node n in thisNode.Children)
                    {
                        Tree.Node posNode = n.GetNodeNumber(nodeToMutate, ref currentNodeNum);
                        if (posNode != null) return posNode;
                    }
                }
            }

            return null;
        }

        public static bool m_ReplacementDone = false;

        internal void SetNode(int nodeNumToReplace, Node nodeToReplaceWith, ref int currentNodeNum)
        {
            if (currentNodeNum > nodeNumToReplace)
                throw new ApplicationException();
            currentNodeNum++;
            if (this is Tree.FuncNode)
            {
                FuncNode thisNode = this as FuncNode;

                for (int childNum = 0; childNum < thisNode.Children.Length; childNum++)
                {
                    if (currentNodeNum == nodeNumToReplace) {
                        thisNode.Children[childNum] = nodeToReplaceWith;
                        m_ReplacementDone = true;
                        return;
                    }
                    else  {
                        thisNode.Children[childNum].SetNode(nodeNumToReplace, nodeToReplaceWith, ref currentNodeNum);
                    }
                }
            }
            
            return;
        }


        internal Tree.FuncNode GetFunctionNumber(int funcNumToMutate, ref int currentFuncNum)
        {
            if (this is Tree.FuncNode)
            {
                FuncNode thisFunc = this as FuncNode;
                if (currentFuncNum == funcNumToMutate)
                    return thisFunc;
                currentFuncNum++;

                foreach (Node n in thisFunc.Children)
                {
                    Tree.FuncNode posNode = n.GetFunctionNumber(funcNumToMutate, ref currentFuncNum);
                    if (posNode != null) return posNode;
                }
            }

            return null;
        }

    }
}
