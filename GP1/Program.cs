using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Drawing;

namespace GP1
{
    public class Program
    {
        private Tree.Node m_TopNode;
        private double m_Fitness;
        private float m_Result;

        public Tree.Variable[] m_Variables;
        private Tree.Func[] m_Functions;
        private int[] m_Values;
        
        const int MAXPROGRAMDEPTH = 5;

        private static Random s_Random = new Random();

        public double Fitness
        {
            get { return m_Fitness; }
        }
        public float Result
        {
            get { return m_Result; }
        }

        public static Program GenerateRandomProgram(Tree.Variable[] variables, Tree.Func[] functions, int[] values)
        {
            Program p = new Program();
            p.m_Functions = functions;
            p.m_Values = values;
            p.m_Variables = variables;
            p.m_TopNode = p.GenerateRandomNode(0);

            return p;
        }

        private Tree.Node GenerateRandomNode(int depth)
        {
            double randType = s_Random.NextDouble();
            Tree.Node node;

            if (randType > 0.9 || depth == MAXPROGRAMDEPTH)
            {
                int valueNum = s_Random.Next(m_Values.Length);
                node = new Tree.ValueNode(m_Values[valueNum]);
            }
            else if (randType > 0.6)
            {
                int variableNum = s_Random.Next(m_Variables.Length);
                node = new Tree.VariableNode(m_Variables[variableNum]);
            } 
            else
            {
                int funcNum = s_Random.Next(m_Functions.Length);
                node = new Tree.FuncNode(
                    new Tree.Node[] { GenerateRandomNode(depth + 1), GenerateRandomNode(depth + 1) }, 
                    m_Functions[funcNum]);
            }

            node.Depth = depth;

            return node;
        }

        public Program Clone()
        {
            Program p = new Program();
            p.m_Functions = this.m_Functions;
            p.m_TopNode = this.m_TopNode.CloneTree();
            p.m_Values = this.m_Values;
            p.m_Variables = this.m_Variables;

            return p;
        }

        public void Mutate()
        {
            if (this.TreeSize == 1)
            {
                m_TopNode = GenerateRandomNode(0);
                return;
            }

            int funcNumToMutate = s_Random.Next(m_TopNode.TreeSizeFunctionsOnly);
            int currentFuncNum = 0;
            Tree.FuncNode funcToMutate = m_TopNode.GetFunctionNumber(funcNumToMutate, ref currentFuncNum);

            if (s_Random.Next(2) == 1)
                funcToMutate.Children[0] = GenerateRandomNode(funcToMutate.Depth);
            else
                funcToMutate.Children[1] = GenerateRandomNode(funcToMutate.Depth);
        }

        public Program Crossover(Program prog2)
        {
            Program prog1Clone = this.CloneProgram();
            Tree.Node nodeToTake = null;

            if (prog2.TreeSize == 1)
            {
                nodeToTake = prog2.m_TopNode.CloneTree();
            }
            else
            {
                int funcNumToMutate = s_Random.Next(prog2.m_TopNode.TreeSizeFunctionsOnly);
                int currentFuncNum = 0;
                Tree.FuncNode funcToMutate = prog2.m_TopNode.GetFunctionNumber(funcNumToMutate, ref currentFuncNum);

                if (s_Random.Next(2) == 1)
                    nodeToTake = funcToMutate.Children[0].CloneTree();
                else
                    nodeToTake = funcToMutate.Children[1].CloneTree();
            }

            if (this.TreeSize == 1)
            {
                prog1Clone.m_TopNode = nodeToTake;
            }
            else
            {
                int funcNumToMutate = s_Random.Next(prog1Clone.m_TopNode.TreeSizeFunctionsOnly);
                int currentFuncNum = 0;
                Tree.FuncNode funcToMutate = prog1Clone.m_TopNode.GetFunctionNumber(funcNumToMutate, ref currentFuncNum);

                if (s_Random.Next(2) == 1)
                    funcToMutate.Children[0] = nodeToTake;
                else
                    funcToMutate.Children[1] = nodeToTake;
            }

            return prog1Clone;
        }

        public void Run()
        {
            m_Result = m_TopNode.Evaluate();
        }

        public Bitmap Draw()
        {
            float width = 1000;
            float height = 500;

            Bitmap bmp = new Bitmap((int)width, (int)height);
            Brush brush = Brushes.Black;
            Graphics g = Graphics.FromImage(bmp);

            DrawNode(m_TopNode, g, 1, width / 2, 50, (float)width);

            return bmp;
        }

        private void DrawNode(Tree.Node node, Graphics g, int depth, float x, float y, float imageWidth)
        {
            Font font = new Font("Script", 14);
            int rectWidth = 50;
            int rectHeight = 20;
            int distanceYBetweenNodes = rectHeight * 2;
            if (depth > 10) return; //only draw to depth 10;

            if (node is Tree.FuncNode)
            {
                Tree.FuncNode functionNode = node as Tree.FuncNode;
                g.DrawRectangle(Pens.Blue, x - (rectWidth / 2), y - (rectHeight / 2), rectWidth, rectHeight);
                g.DrawString(functionNode.Function.Name, font, Brushes.Blue, x - 10, y - 10);

                float nodeSplitting = (imageWidth / 2) * (float)Math.Pow(0.5, depth);
                for (int i = 0; i < functionNode.Children.Length; i++)
                {
                    float childX = x + (i == 0 ? -nodeSplitting : nodeSplitting);
                    g.DrawLine(Pens.Black, x, y + rectHeight / 2, childX, y + distanceYBetweenNodes - rectHeight / 2);
                    DrawNode(functionNode.Children[i], g, depth + 1, childX, y + distanceYBetweenNodes, imageWidth);
                }
            }
            else if (node is Tree.ValueNode)
            {
                Tree.ValueNode valueNode = node as Tree.ValueNode;
                g.DrawRectangle(Pens.Red, x - (rectWidth / 2), y - (rectHeight / 2), rectWidth, rectHeight);
                g.DrawString(valueNode.Value.ToString(), font, Brushes.Blue, x - 10, y - 10);
            }
            else if (node is Tree.VariableNode)
            {
                Tree.VariableNode variableNode = node as Tree.VariableNode;
                g.DrawRectangle(Pens.DarkGreen, x - (rectWidth / 2), y - (rectHeight / 2), rectWidth, rectHeight);
                g.DrawString(variableNode.Variable.Name.ToString(), font, Brushes.DarkGreen, x - 10, y - 10);
            }
            else throw new ApplicationException();
        }

        private void DrawNode(Tree.Node m_TopNode)
        {
            throw new NotImplementedException();
        }

        // Number of nodes in program
        public int TreeSize
        {
            get
            {
                return m_TopNode.Treesize;
            }
        }

        public Program CloneProgram()
        {
            Program p = new Program();
            p.m_Functions = this.m_Functions;
            p.m_Variables = this.m_Variables;
            p.m_Values = this.m_Values;
            p.m_TopNode = this.m_TopNode.CloneTree();

            return p;
        }
    }
}
