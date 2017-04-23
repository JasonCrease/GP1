using System;
using System.Drawing;
using GP1.Tree;

namespace GP1
{
    public class Program
    {
        private Tree.Node m_TopNode;
        private double m_Fitness;
        private float m_Result;

        private Tree.Func[] m_Functions;
        private int[] m_Values;

        private Tree.Variable[] m_Variables;
        public Tree.Variable[] Variables { get { return m_Variables;  } }

        const int MAXPROGRAMDEPTH = 6;
        const int MAXNODES = 50;

        private static Random s_Random = new Random();

        private bool m_FitnessIsDirty = true;
        public bool FitnessIsDirty {  set { m_FitnessIsDirty = value; } get { return m_FitnessIsDirty; } }

        public Tree.Node TopNode
        {
            get { return m_TopNode; }
            set {
                if (m_TopNode != value)
                {
                    m_TopNode = value;
                    m_FitnessIsDirty = true;
                }
            }
        }
        public double Fitness
        {
            get { return m_Fitness; }
            set {
                    m_Fitness = value;
                    m_FitnessIsDirty = false;
                }
        }
        public float Result
        {
            get { return m_Result; }
        }

        public static Program GenerateRandomProgram(Tree.Variable[] variables, Tree.Func[] functions, int[] values)
        {
            Program p = new Program();
            p.m_FitnessIsDirty = true;
            p.m_Functions = functions;
            p.m_Values = values;
            p.m_Variables = variables;
            p.m_TopNode = p.GenerateRandomNode(0);

            return p;
        }

        int m_NodesAdded = 0;

        private Tree.Node GenerateRandomNode(int depth)
        {
            double randType = s_Random.NextDouble();
            Tree.Node node;

            if (randType > 0.7 || depth >= MAXPROGRAMDEPTH || m_NodesAdded > MAXNODES)
            {
                int valueNum = s_Random.Next(m_Values.Length);
                node = new Tree.ValueNode(m_Values[valueNum]);
            }
            else if (randType > 0.4)
            {
                int variableNum = s_Random.Next(m_Variables.Length);
                node = new Tree.VariableNode(m_Variables[variableNum]);
            }
            else
            {
                int funcNum = s_Random.Next(m_Functions.Length);

                if (m_Functions[funcNum].NumberOfArguments == 2)
                    node = new Tree.FuncNode(
                        new Tree.Node[] { GenerateRandomNode(depth + 1), GenerateRandomNode(depth + 1) },
                        m_Functions[funcNum]);
                else if (m_Functions[funcNum].NumberOfArguments == 4)
                    node = new Tree.FuncNode(
                        new Tree.Node[] { GenerateRandomNode(depth + 1), GenerateRandomNode(depth + 1), GenerateRandomNode(depth + 1), GenerateRandomNode(depth + 1) },
                        m_Functions[funcNum]);
                else throw new ApplicationException();
            }

            node.Depth = depth;

            m_NodesAdded++;
            return node;
        }

        public Program CloneProgram()
        {
            Program p = new Program();
            p.m_FitnessIsDirty = this.m_FitnessIsDirty;
            p.m_Fitness = this.m_Fitness;
            p.m_Functions = this.m_Functions;
            p.m_Variables = this.m_Variables;
            p.m_Values = this.m_Values;
            p.m_TopNode = this.m_TopNode.CloneTree();

            return p;
        }

        public Program Mutate()
        {
            Program retProg = this.CloneProgram();

            int nodeToReplace = s_Random.Next(0, retProg.TreeSize);
            int currentNodeNum = 0;

            Node replacementNode = GenerateRandomNode(retProg.m_TopNode.Depth / 2);

            if (nodeToReplace == 0)
                retProg.m_TopNode = replacementNode;
            else
            { 
            retProg.m_TopNode.SetNode(nodeToReplace, replacementNode, ref currentNodeNum);
            if (!Node.m_ReplacementDone)
                throw new ApplicationException();
            }
            return retProg;
        }

        public Program Crossover(Program prog2)
        {
            Program prog1Clone = this.CloneProgram();
            Tree.Node nodeToTake = null;

            int nodeToTakeNum = s_Random.Next(prog2.TreeSize);
            int currentNodeNum = 0;
            nodeToTake = prog2.m_TopNode.GetNodeNumber(nodeToTakeNum, ref currentNodeNum).CloneTree();


            int prog1NodeToReplace = s_Random.Next(0, prog1Clone.TreeSize);
            currentNodeNum = 0;

            if (prog1NodeToReplace == 0)
                prog1Clone.m_TopNode = nodeToTake;
            else
            {
                prog1Clone.m_TopNode.SetNode(prog1NodeToReplace, nodeToTake, ref currentNodeNum);
                if (!Node.m_ReplacementDone)
                    throw new ApplicationException();
            }

            return prog1Clone;
        }
        

        public void Run()
        {
            m_Result = m_TopNode.Evaluate();
            m_FitnessIsDirty = false;
        }

        public Bitmap Draw(String title, float width, float height)
        {
            Bitmap bmp = new Bitmap((int)width, (int)height);
            Brush brush = Brushes.Black;
            Graphics g = Graphics.FromImage(bmp);

            DrawNode(m_TopNode, g, 1, width / 2, 20, (float)width);

            g.DrawString(title, TitleFont, Brushes.Black, 10, 10);

            return bmp;
        }

        static Font TitleFont = new Font("Arial", 16);
        static Pen LinePen = new Pen(Brushes.Black, 2);
        static Font Font = new Font("Arial", 12);

        private static void DrawNode(Tree.Node node, Graphics g, int depth, float x, float y, float imageWidth)
        {
            const int CHAR_WIDTH = 16;
            const int rectHeight = 20;
            const float distanceYBetweenNodes = (float)rectHeight * 1.5f;

            int rectWidth = 40;
            if (depth > 12) return; //only draw to depth 12;

            if (node is Tree.FuncNode)
            {
                Tree.FuncNode functionNode = node as Tree.FuncNode;
                rectWidth = functionNode.Function.Name.Length * CHAR_WIDTH;
                g.DrawRectangle(Pens.Blue, x - (rectWidth / 2), y - (rectHeight / 2), rectWidth, rectHeight);
                g.DrawString(functionNode.Function.Name, Font, Brushes.Blue, x - (rectWidth / 2), y - 10);

                int numberOfChildren = functionNode.Children.Length;
                float nodeSplitting = (float)(imageWidth / 2) * (float)Math.Pow(0.5, depth) * (float)1.0;

                for (int i = 0; i < numberOfChildren; i++)
                {
                    float childX = (x - nodeSplitting) + ((i * nodeSplitting * 2) / (numberOfChildren - 1));
                    g.DrawLine(LinePen, x, y + rectHeight / 2, childX, y + distanceYBetweenNodes - rectHeight / 2);
                    DrawNode(functionNode.Children[i], g, depth + 1, childX, y + distanceYBetweenNodes, imageWidth);
                }
            }
            else if (node is Tree.ValueNode)
            {
                Tree.ValueNode valueNode = node as Tree.ValueNode;
                rectWidth = valueNode.Value.ToString().Length * CHAR_WIDTH;
                g.DrawRectangle(Pens.Red, x - (rectWidth / 2), y - (rectHeight / 2), rectWidth, rectHeight);
                g.DrawString(valueNode.Value.ToString(), Font, Brushes.Blue, x - (rectWidth / 2), y - 10);
            }
            else if (node is Tree.VariableNode)
            {
                Tree.VariableNode variableNode = node as Tree.VariableNode;
                rectWidth = variableNode.Variable.Name.Length * CHAR_WIDTH;
                g.DrawRectangle(Pens.DarkGreen, x - (rectWidth / 2), y - (rectHeight / 2), rectWidth, rectHeight);
                g.DrawString(variableNode.Variable.Name.ToString(), Font, Brushes.DarkGreen, x - (rectWidth / 2), y - 10);
            }
            else throw new ApplicationException();
        }

        // Number of nodes in program
        public int TreeSize
        {
            get
            {
                return m_TopNode.Treesize;
            }
        }
    }
}
