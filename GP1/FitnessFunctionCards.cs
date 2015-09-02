using System;

namespace GP1
{
    public class FitnessFunctionCards : IFitnessFunction
    {
        public float Evaluate(Program program)
        {
            Tree.Variable card11 = program.Variables[0];
            Tree.Variable card12 = program.Variables[1];
            Tree.Variable card21 = program.Variables[2];
            Tree.Variable card22 = program.Variables[3];

            float fitness = 0;

            int[,] hands =
            {
               // hand1      hand2       1 if hand1 > hand2, 0 if draw, -1 if hand1 < hand2
                { 1, 2,      1, 2,       0},
                { 1, 3,      7, 2,       0},
                { 4, 3,      4, 2,       0},
                { 1, 8,      2, 8,       0},
                { 7, 7,      7, 7,       0},
                { 3, 4,      4, 4,       0},
                { 1, 4,      3, 4,       0},
                { 2, 2,      2, 1,       0},
                { 6, 2,      3, 6,       0},

                { 8, 2,      2, 4,       1},
                { 4, 3,      3, 3,       1},
                { 2, 1,      1, 1,       1},
                { 7, 1,      4, 6,       1},
                { 7, 7,      7, 7,       1},
                { 2, 9,      6, 2,       1},
                { 6, 7,      6, 5,       1},
                { 9, 3,      5, 7,       1},
                { 4, 1,      2, 6,       1},
                
                { 3, 4,      5, 4,       -1},
                { 5, 2,      3, 6,       -1},
                { 1, 6,      7, 1,       -1},
                { 7, 1,      4, 8,       -1},
                { 3, 3,      2, 7,       -1},
                { 4, 6,      6, 7,       -1},
                { 2, 3,      4, 2,       -1},
                { 6, 2,      7, 2,       -1},
                { 1, 2,      4, 3,       -1},
                
            };

            for (int i = 0; i < hands.Length / 5; i += 1)
            {
                card11.Value = hands[i, 0];
                card12.Value = hands[i, 1];
                card21.Value = hands[i, 2];
                card22.Value = hands[i, 3];

                program.Run();
                float desiredResult = hands[i, 4];
                fitness += Math.Abs(program.Result - desiredResult);
            }

            fitness += program.TreeSize / 10;

            return fitness;
        }
    }

}
