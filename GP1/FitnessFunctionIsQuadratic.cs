using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP1
{
    public class FitnessFunctionIsQuadratic : IFitnessFunction
    {
        public static double Evaluate(Program program)
        {
            Tree.Variable XVariable = program.m_Variables[0];
            float totalError = 0;

            for (int x = -5; x < 5; x += 1)
            {
                XVariable.Value = x;
                program.Run();
                totalError = Math.Abs(program.Result - x);
            }

            return totalError;
        }
    }
}
