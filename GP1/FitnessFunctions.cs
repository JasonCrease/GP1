using System;
using System.Collections.Generic;
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
                return new[] { "C1", "C2" };
            }
        }

        public double Evaluate(Program program)
        {
            double fitness = 0.00;

            int[,] C = new int[,]
            {
                {0, 1,   0},
                {0, 1,   0},
                {2, 3,   0},
                {2, 2,   1},
                {4, 4,   1},
                {8, 8,   1},
                {2, 5,   0},
                {6, 1,   0},
                {9, 9,   1},
                {1, 8,   0},
                {3, 3,   1}
            };

            for (int i = 0; i < C.Length / 3; i += 1)
            {
                program.Variables[0].Value = C[i, 0];
                program.Variables[1].Value = C[i, 1];
                program.Run();
                float desiredResult = C[i, 2];
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

            for (int x = 0; x < 5; x += 1)
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
