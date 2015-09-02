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
        public float Evaluate(Program program)
        {
            Tree.Variable XVariable = program.Variables[0];
            float fitness = 0;

            for (int x = -5; x < 5; x += 1)
            {
                XVariable.Value = x;
                program.Run();
                float desiredResult = (x * x) - x;
                fitness += Math.Abs(program.Result - desiredResult);
            }

            fitness += program.TreeSize / 5;

            return fitness;
        }
    }

    public class FitnessFunctionAlternatesGettingLarger : IFitnessFunction
    {
        public float Evaluate(Program program)
        {
            Tree.Variable XVariable = program.Variables[0];
            float fitness = 0;
            int[] Xs = new int[] { 1, 2, 5, 10, 17, 0, 37, 50, 65 };

            for (int x = 0; x < 8; x += 1)
            {
                XVariable.Value = x;
                program.Run();
                float desiredResult = Xs[x];
                fitness += Math.Abs(program.Result - desiredResult);
            }

            fitness += program.TreeSize / 5;

            return fitness;
        }
    }

    public class FitnessFunctionOneSometimesZeroes : IFitnessFunction
    {
        public float Evaluate(Program program)
        {
            Tree.Variable XVariable = program.Variables[0];
            float fitness = 0;
            int[] Ys = new int[] { 5, 4, 3, 2, 1, 2, 3, 4, 5, 6 };

            for (int x = 0; x < 10; x += 1)
            {
                XVariable.Value = x;
                program.Run();
                float desiredResult = Ys[x];
                fitness += Math.Abs(program.Result - desiredResult);
            }

            fitness += program.TreeSize / 5;

            return fitness;
        }
    }

}
