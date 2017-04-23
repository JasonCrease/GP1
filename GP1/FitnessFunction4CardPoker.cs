using System;

namespace GP1
{
    public class FitnessFunction4CardPoker : IFitnessFunction
    {
        public string[] Variables
        {
            get
            {
                return new[] { "C1", "C2", "C3", "C4" };
            }
        }

        public double TerminatingFitness
        {
            get
            {
                return 5;
            }
        }
        
        const int HANDSTODEAL = 300;
        int[][] C = new int[HANDSTODEAL][];

        public FitnessFunction4CardPoker()
        {
            int EACHTODEAL = HANDSTODEAL / 4;
            Random r = new Random(13);

            int straightsAdded = 0;
            int fourKindAdded = 0;
            int threeKindAdded = 0;
            int twoKindAdded = 0;
            int unknownAdded = 0;
            int i = 0;

            while (i < HANDSTODEAL - 1)
            {
                i = straightsAdded + fourKindAdded + threeKindAdded + twoKindAdded + unknownAdded;
                C[i] = new int[5];
                int card1 = C[i][1] = r.Next(1, 13);
                int card2 = C[i][2] = r.Next(1, 13);
                int card3 = C[i][3] = r.Next(1, 13);
                int card4 = C[i][4] = r.Next(1, 13);

                Array.Sort(C[i]);

                if (C[i][1] + 1 == C[i][2] && C[i][2] + 1 == C[i][3] && C[i][3] + 1 == C[i][4] && straightsAdded < EACHTODEAL)  
                {
                    C[i][0] = 4;
                    straightsAdded++;
                }
                else if (C[i][1] == C[i][2] && C[i][2] == C[i][3] && C[i][3] == C[i][4] && fourKindAdded < EACHTODEAL)  
                {
                    C[i][0] = 3;
                    fourKindAdded++;
                }
                else if (((C[i][1] == C[i][2] && C[i][2] == C[i][3]) ||  (C[i][2] == C[i][3] && C[i][3] == C[i][4]))
                    && threeKindAdded < EACHTODEAL)  
                {
                    C[i][0] = 2;
                    threeKindAdded++;
                }
                else if ((C[i][1] == C[i][2] ||  C[i][2] == C[i][3] ||  C[i][3] == C[i][4] ) && twoKindAdded < EACHTODEAL)  
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

            fitness += (double)program.TreeSize / 10D;

            return fitness;
        }
    }
}
