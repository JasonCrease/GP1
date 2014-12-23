using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP1
{
    public interface IChromosome
    {
        /// <summary>
        /// Chromosome's fintess value
        /// </summary>
        float Fitness { get; set; }

        /// <summary>
        /// Generate random chromosome
        /// </summary>
        void Generate(int param = 0);

        /// <summary>
        /// Clone the chromosome
        /// </summary>
        IChromosome Clone();

        /// <summary>
        /// Mutation operator
        /// </summary>
        void Mutate();

        /// <summary>
        /// Crossover operator
        /// </summary>
        void Crossover(IChromosome ch2);

        /// <summary>
        /// Evaluate chromosome with specified fitness function
        /// </summary>
        void Evaluate(IFitnessFunction function);
    }
}
