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
                float desiredResult = (x * x * x) + x + 2;
                fitness += Math.Abs(program.Result - desiredResult);
            }

            //fitness += program.Length / 2;

            return fitness;
        }
    }

    public class FitnessFunctionAlternatesGettingLarger : IFitnessFunction
    {
        public float Evaluate(Program program)
        {
            Tree.Variable XVariable = program.m_Variables[0];
            float fitness = 0;
            int[] Xs = new int[] { 2, 4, 6, 8};

            for (int x = 0; x < 4; x += 1)
            {
                XVariable.Value = x;
                program.Run();
                float desiredResult = Xs[x];
                fitness += Math.Abs(program.Result - desiredResult);
            }

            fitness += program.Length / 4;

            return fitness;
        }
    }
}
