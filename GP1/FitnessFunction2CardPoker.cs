namespace GP1
{
    public class FitnessFunction2CardPoker : IFitnessFunction
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
                // Nothing
                {0,     1, 4},
                {0,     1, 8},
                {0,     2, 4},
                {0,     2, 9},
                {0,     4, 6},
                {0,     6, 9},
                {0,     7, 9},
                {0,     9, 12},

                // Pair
                {1,     2, 2},
                {1,     3, 3},
                {1,     4, 4},
                {1,     5, 5},
                {1,     6, 6},
                {1,     7, 7},
                {1,     9, 9},
                {1,     12, 12},

                // Straight
                {2,     1, 2},
                {2,     2, 3},
                {2,     3, 4},
                {2,     4, 5},
                {2,     5, 6},
                {2,     6, 7},
                {2,     7, 8},
                {2,     10, 11},
                {2,     11, 12},
            };

            for (int i = 0; i < C.Length / 3; i += 1)
            {
                program.Variables[0].Value = C[i, 1];
                program.Variables[1].Value = C[i, 2];
                program.Run();
                float desiredResult = C[i, 0];
                fitness += program.Result == desiredResult ? 0 : 1;
            }

            fitness += (double)program.TreeSize / 20D;

            return fitness;
        }
    }
}
