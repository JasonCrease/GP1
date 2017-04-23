using System;

namespace TestOutputProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            int rights = 0;
            int errors = 0;

            for (int a = 1; a < 14; a++)
                for (int b = a; b < 14; b++)
                    for (int c = b; c < 14; c++)
                    {
                        int real = ActualScore(a, b, c);
                        int calc = Prog.Program.Function(a, b, c);

                        if (real == calc)
                        {
                            Console.WriteLine("{0},{1},{2} was correctly {3}", a, b, c, calc);
                            rights++;
                        }
                        else
                        {
                            Console.WriteLine("{0},{1},{2} should be {3}. Was {4}", a, b, c, real, calc);
                            errors++;
                        }
                    }

            Console.WriteLine("{0} right. {1} wrong", rights, errors);
            Console.ReadLine();
        }

        private static int ActualScore(int a, int b, int c)
        {
            if (a + 1 == b && b + 1 == c)
                return 3;
            else if (a == b && b == c)
                return 2;
            else if (a == b || b == c)
                return 1;
            else
                return 0;
        }
    }
}
