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
        public static Random s_Random = new Random();

        public Node CreateNodeRandomly(int distanceFromMaxDepth)
        {
            double rand = s_Random.NextDouble();

            if(rand > 0.8 )
            {

            } 
            else if (rand > 0.8)
            {

            }
            else
            {

            }
        }
    }
}
