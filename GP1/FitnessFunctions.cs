using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP1
{
    public class FitnessFunctionIsQuadratic : IFitnessFunction
    {
        public double Evaluate(Program program)
        {
            Tree.Variable XVariable = program.Variables[0];
            double fitness = 0;

            for (int x = -5; x < 5; x += 1)
            {
                XVariable.Value = x;
                program.Run();
                double desiredResult = (x * x) - x;
                fitness += Math.Abs(program.Result - desiredResult);
            }

            fitness += (double)program.TreeSize / 5D;

            return fitness;
        }

        public string[] Variables
        {
            get
            {
                return new []{"X"};
            }
        }
    }

    public class FitnessFunctionIsItAStraight : IFitnessFunction
    {
        public string[] Variables
        {
            get
            {
                return new[] { "C1", "C2", "C3" };
            }
        }

        public double Evaluate(Program program)
        {
            double fitness = 0.00;

            int[,] C = new int[,]
            {
                // Nothing
                {0,     1, 4, 6},
                {0,     4, 5, 9},
                {0,     2, 8, 9},
                {0,     1, 2, 8},
                {0,     3, 8, 9},
                {0,     5, 6, 9},
                {0,     1, 4, 8},
                {0,     2, 3, 9},
                {0,     3, 5, 9},
                {0,     8, 9, 11},
                {0,     4, 7, 12},

                
                // Pair
                {1,     2, 2, 3},
                {1,     4, 5, 5},
                {1,     1, 6, 6},
                {1,     1, 1, 4},
                {1,     7, 7, 9},
                {1,     8, 8, 9},
                {1,     2, 2, 4},
                {1,     5, 5, 6},
                {1,     5, 5, 8},
                {1,     2, 2, 4},
                {1,     7, 7, 8},
                {1,     1, 5, 5},
                {1,     1, 8, 8},

                //// Straight
                //{2,     1, 2, 3},
                //{2,     2, 3, 4},
                //{2,     3, 4, 5},
                //{2,     4, 5, 6},
                //{2,     5, 6, 7},
                //{2,     6, 7, 8},
                //{2,     7, 8, 9},
            };

            for (int i = 0; i < C.Length / 4; i += 1)
            {
                program.Variables[0].Value = C[i, 1];
                program.Variables[1].Value = C[i, 2];
                program.Variables[2].Value = C[i, 3];
                program.Run();
                float desiredResult = C[i, 0];
                fitness += (double)Math.Abs(program.Result - desiredResult);
            }

            fitness += (double)program.TreeSize / 20D;

            return fitness;
        }
    }


    public class FitnessFunctionAlternatesGettingLarger : IFitnessFunction
    {
        public double Evaluate(Program program)
        {
            Tree.Variable XVariable = program.Variables[0];
            double fitness = 0;
            int[] Xs = new int[] { 1, 2, 6, 12, 20 };

            for (int x = 0; x < 8; x += 1)
            {
                XVariable.Value = x;
                program.Run();
                double desiredResult = Xs[x];
                fitness += Math.Abs(program.Result - desiredResult);
            }

            fitness += (double)program.TreeSize / 5D;

            return fitness;
        }

        public string[] Variables
        {
            get
            {
                return new[] { "X" };
            }
        }
    }

    public class FitnessFunctionOneSometimesZeroes : IFitnessFunction
    {
        public double Evaluate(Program program)
        {
            Tree.Variable XVariable = program.Variables[0];
            double fitness = 0;
            int[] Ys = new int[] { 0, 2, -4, 6, -8, 10, -12, 14, -16, 18, -20 };

            for (int x = 0; x < 10; x += 1)
            {
                XVariable.Value = x;
                program.Run();
                double desiredResult = Ys[x];
                fitness += Math.Abs(program.Result - desiredResult);
            }

            fitness += (double)program.TreeSize / 5D;

            return fitness;
        }

        public string[] Variables
        {
            get
            {
                return new[] { "X" };
            }
        }
    }

}
