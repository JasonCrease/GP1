using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP1
{
    public interface IFitnessFunction
    {
        static double Evaluate(Program program);
    }
}
