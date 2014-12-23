using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP1
{
    /// <summary>
    /// Interface for implementing Custom fitness function for chromosome evaluation.
    /// </summary>
    public interface IFitnessFunction
    {
        float Evaluate(IChromosome chromosome);
    }
}
