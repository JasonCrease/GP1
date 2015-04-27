using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP1
{
    public class FitnessFunctionIsQuadratic : IFitnessFunction
    {
        public float Evaluate(Program program)
        {
            Tree.Variable XVariable = program.m_Variables[0];
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
            Tree.Variable XVariable = program.m_Variables[0];
            float fitness = 0;
            int[] Xs = new int[] { 1, 2, 6, 12, 20 };

            for (int x = 0; x < 5; x += 1)
            {
                XVariable.Value = x;
                program.Run();
                float desiredResult = Xs[x];
                fitness += Math.Abs(program.Result - desiredResult);
            }

            fitness += program.TreeSize / 2;

            return fitness;
        }
    }

    public class FitnessFunctionOneSometimesZeroes : IFitnessFunction
    {
        public float Evaluate(Program program)
        {
            Tree.Variable XVariable = program.m_Variables[0];
            float fitness = 0;
            int[] Ys = new int[] { 5, 5, 5, 5, 4, 5, 6, 7, 8, 9 };

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
