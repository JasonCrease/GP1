using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Drawing;

namespace GP1
{
    public class PopulationStatistics
    {
        private double m_FitnessFirstQuartile;
        private double m_FitnessSecondQuartile;
        private double m_FitnessThirdQuartile;
        private double m_FitnessFourthQuartile;

        public double FitnessFirstQuartile
        {
            get { return m_FitnessFirstQuartile; }
        }
        public double FitnessSecondQuartile
        {
            get { return m_FitnessSecondQuartile;  }
        }
        public double FitnessThirdQuartile
        {
            get { return m_FitnessThirdQuartile; }
        }
        public double FitnessFourthQuartile
        {
            get { return m_FitnessFourthQuartile; }
        }

        public PopulationStatistics(List<Program> programs)
        {
            double[] fitnesses = programs.Select(x => x.Fitness).ToArray();
            Array.Sort(fitnesses);
            int numFitness = fitnesses.Length;
            m_FitnessFirstQuartile = fitnesses[numFitness * 1 / 4];
            m_FitnessSecondQuartile = fitnesses[numFitness * 2 / 4];
            m_FitnessThirdQuartile = fitnesses[numFitness * 3 / 4];
            m_FitnessFourthQuartile = fitnesses[numFitness - 1];
        }

        public void DrawFitnessDistribution(Graphics g)
        {

        }
    }
}
