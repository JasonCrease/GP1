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
                node = new Tree.FunctionNode(
                    new Tree.Node[] { GenerateRandomNode(depth + 1), GenerateRandomNode(depth + 1) }, 
                    m_Functions[funcNum]);
            }

            return node;
        }

        public void Clone()
        {
            throw new NotImplementedException();
        }

        public void Mutate()
        {
            throw new NotImplementedException();
        }

        public void Crossover(Program prog2)
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            m_Result = m_TopNode.Evaluate();
        }

        public Bitmap Draw()
        {
            float width = 900;
            float height = 500;

            Bitmap bmp = new Bitmap((int)width, (int)height);
            Brush brush = Brushes.Black;
            Graphics g = Graphics.FromImage(bmp);

            DrawNode(m_TopNode, g, 1, width / 2, 50, (float)width);

            return bmp;
        }

        private void DrawNode(Tree.Node node, Graphics g, int depth, float x, float y, float imageWidth)
        {
            Font font = new Font("Courier New", 12);
            int rectWidth = 60;
            int rectHeight = 30;
            int distanceYBetweenNodes= 60;
            if (depth > 10) return; //only draw to depth 10;

            if (node is Tree.FunctionNode)
            {
                Tree.FunctionNode functionNode = node as Tree.FunctionNode;
                g.DrawRectangle(Pens.Blue, x - (rectWidth / 2), y - (rectHeight / 2), rectWidth, rectHeight);
                g.DrawString(functionNode.Function.Name, font, Brushes.Blue, x - 10, y - 10);

                float nodeSplitting = (imageWidth / 4) / depth;
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
    }
}
