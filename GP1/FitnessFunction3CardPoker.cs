using System;

namespace GP1
{
    public class FitnessFunction3CardPoker : IFitnessFunction
    {
        public string[] Variables
        {
            get
            {
                return new[] { "C1", "C2", "C3" };
            }
        }

        public double TerminatingFitness
        {
            get
            {
                return 4;
            }
        }
        
        const int HANDSTODEAL = 200;
        int[][] C = new int[HANDSTODEAL][];

        public FitnessFunction3CardPoker()
        {
            int EACHTODEAL = HANDSTODEAL / 4;
            Random r = new Random(13);

            int straightsAdded = 0;
            int threeKindAdded = 0;
            int twoKindAdded = 0;
            int unknownAdded = 0;
            int i = 0;

            while (i < HANDSTODEAL - 1)
            {
                i = straightsAdded + threeKindAdded + twoKindAdded + unknownAdded;
                C[i] = new int[4];
                int card1 = C[i][1] = r.Next(1, 13);
                int card2 = C[i][2] = r.Next(1, 13);
                int card3 = C[i][3] = r.Next(1, 13);

                Array.Sort(C[i]);

                if (C[i][1] + 1 == C[i][2] && C[i][2] + 1 == C[i][3] && straightsAdded < EACHTODEAL)  //straight
                {
                    C[i][0] = 3;
                    straightsAdded++;
                }
                else if (C[i][1] == C[i][2] && C[i][2] == C[i][3] && threeKindAdded < EACHTODEAL)  // 3 of a kind
                {
                    C[i][0] = 2;
                    threeKindAdded++;
                }
                else if ((C[i][1] == C[i][2] || C[i][2] == C[i][3]) && twoKindAdded < EACHTODEAL)  // 2 of a kind
                {
                    C[i][0] = 1;
                    twoKindAdded++;
                }
                else if (unknownAdded < EACHTODEAL)
                {
                    C[i][0] = 0;
                    unknownAdded++;
                }
            }

        }

        public double Evaluate(Program program)
        {
            double fitness = 0.00;

            for (int i = 0; i < HANDSTODEAL; i += 1)
            {
                program.Variables[0].Value = C[i][1];
                program.Variables[1].Value = C[i][2];
                program.Variables[2].Value = C[i][3];
                program.Run();
                float desiredResult = C[i][0];
                fitness += program.Result == desiredResult ? 0 : 1;
            }

            fitness += (double)program.TreeSize / 5D;

            return fitness;
        }
    }
}
