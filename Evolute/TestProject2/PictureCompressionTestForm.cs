using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TreeLanguageEvolute;
using System.Collections;

namespace TestProject2
{
    public partial class PictureCompressionTestForm : Form
    {
        FastBitmap fstBitmap;

        // @debug number of skipped pixels changed in 6.2.2013
        int nSkippedPixelsInImage = 3;
        string strLabelString;
        public PictureCompressionTestForm()
        {
            InitializeComponent();
            fstBitmap = new FastBitmap(new Bitmap("test.bmp"));
            GeneticProgrammingEngine engEngine = new GeneticProgrammingEngine();
            engEngine.MinInitialTreeDepth = 2;
            engEngine.MaxInitialTreeDepth = 4;
            engEngine.MaxOverallTreeDepth = 30;
            engEngine.FitnessGoal = BaseEngine.FitnessGoalEnum.MIN_FITNESS;
            engEngine.Overselection = 2;
            engEngine.ChanceForCrossover = 0.9F;
            engEngine.SaveTopIndividuals = 0.1F;
            engEngine.NumberOfThreads = 4;
            engEngine.OnlyBetterFitnessIslands = true;
            engEngine.NumberOfPrograms = 10000;
            engEngine.NumberOfIslands = 500;
            engEngine.MutationChance = 0.1f;
            engEngine.EnableParsimonyPressure = 0.01F;
            engEngine.MigrationsPerGeneration = 5000;
            engEngine.SetFunctions = new FunctionType[]
            { new Add(), new Substract(), new Multiply(), new Divide(), new Cos(), new Modulo(), new IfGreaterThenElse()};
            engEngine.SetValues = new TreeLanguageEvolute.ValueType[] {new RandomMacro()};
            engEngine.DeclareVariable("X");
            engEngine.DeclareVariable("Y");
            engEngine.EvalFitnessForProgramEvent +=
                new GeneticProgrammingEngine.EvalFitnessHandler(engEngine_EvalFitnessForProgramEvent);
            engEngine.GenerationIsCompleteEvent +=
                new GeneticProgrammingEngine.GenerationIsCompleteHandler(engEngine_GenerationIsCompleteEvent);
            engEngine.RunEvoluteOnThread(50000);
            Timer tmTimer = new Timer();
            tmTimer.Interval = 1000;
            tmTimer.Tick += new EventHandler(tmTimer_Tick);
            tmTimer.Start();
        }

        void tmTimer_Tick(object sender, EventArgs e)
        {
            this.label1.Text = strLabelString;
        }

        void engEngine_GenerationIsCompleteEvent(Statistics stsStatistics, 
            BaseEngine sender)
        {
            this.strLabelString = "Generation number " + stsStatistics.GenerationNumber
                                + " : min fitness = " + stsStatistics.MinFitnessProgram.Fitness
                                + " skipped pixels : " + nSkippedPixelsInImage
                                + ", Total nodes = " + stsStatistics.TotalNodes;

            BaseProgram progMinFitness = stsStatistics.MinFitnessProgram;

            // If the generation is between 10-20, 40-50, 60-70 and so forth,
            // Make 10 generations of "tree minimizing which should minimize its fitness
            // According to tree size.
           /*
             if (((stsStatistics.GenerationNumber / 50) % 2) == 1)
                        {
                            float fMinFitness = progMinFitness.Fitness;
                            foreach (BaseProgram[] islIsland in sender.Population)
                            {
                                foreach (BaseProgram progProgram in islIsland)
                                {
                                    TreeProgram treProgram = (TreeProgram)progProgram;
                                    if (treProgram.Fitness <= fMinFitness)
                                    {
                                        treProgram.Fitness = fMinFitness * (1 - ((float)1 / treProgram.Depth));
                                    }
                                }
                            }
                        }*/
            

            FastBitmap fstOutputBitmap = null;
            if ((stsStatistics.GenerationNumber % 1) == 0)
            {
                fstOutputBitmap =
                    new FastBitmap(fstBitmap.Width * 6, fstBitmap.Height * 6);

                Hashtable hsVariables = progMinFitness.GetVariables();
                Variable varX = (Variable)hsVariables["X"];
                Variable varY = (Variable)hsVariables["Y"];

                for (int nY = 0;
                     nY < fstOutputBitmap.Height;
                     nY++)
                {
                    for (int nX = 0;
                         nX < fstOutputBitmap.Width;
                         nX++)
                    {
                        varX.Value = (float)nX / fstOutputBitmap.Width;
                        varY.Value = (float)nY / fstOutputBitmap.Height;
                        progMinFitness.Run();
                        float fActualResult = progMinFitness.Result;
                        byte nActualGrayScaleColor = (byte)((fActualResult + 1.0F) * 127.0F);
                        fstOutputBitmap.SetPixel(nX, nY,
                            nActualGrayScaleColor, nActualGrayScaleColor, nActualGrayScaleColor);
                    }
                }


                fstOutputBitmap.Draw(this.CreateGraphics(), 0, 0,
              this.Width, this.Height);
           
            }
            /*else
            {
                fstOutputBitmap =
                    new FastBitmap(fstBitmap.Width * 6, fstBitmap.Height * 6);
            }*/

           
//              try
//              {
//                 Graphics g = this.CreateGraphics();
//                 stsStatistics.MinFitnessProgram.Draw(g, this.Width, this.Height, this);
//              }
//              catch (System.Exception ex)
//              {  
//              }
        }

        void engEngine_EvalFitnessForProgramEvent(BaseProgram progProgram,
            BaseEngine sender)
        {
            progProgram.Fitness = 0;
            Hashtable hsVariables = progProgram.GetVariables();
            Variable varX = (Variable)hsVariables["X"];
            Variable varY = (Variable)hsVariables["Y"];

            for (int nY = 0;
                 nY < fstBitmap.Height;
                 nY += nSkippedPixelsInImage)
            {
                for (int nX = 0;
                     nX < fstBitmap.Width;
                     nX += nSkippedPixelsInImage)
                {
                    varX.Value = (float)nX / fstBitmap.Width;
                    varY.Value = (float)nY / fstBitmap.Height;
                    Color clrPixel = fstBitmap.GetPixel(nX, nY);
                    byte nGrayScaleColor = (byte)((clrPixel.R + clrPixel.G + clrPixel.B) / 3);
                    float fExpectedResult = (float)((nGrayScaleColor / 128.0) - 1);
                    progProgram.Run();
                    float fActualResult = progProgram.Result;
                    float fDifference = Math.Abs(fExpectedResult - fActualResult);
                    progProgram.Fitness += fDifference;
                    byte nActualGrayScaleColor = (byte)((fActualResult + 1) * 128.0);
                }
            }

            // @debug 6.2.2013 : in each generation, we select different pixels.
            /*for (int nY = 0;
                 nY < fstBitmap.Height;
                 nY ++)
            {
                for (int nX = (sender.GenerationNum % nSkippedPixelsInImage);
                     nX < fstBitmap.Width;
                     nX += nSkippedPixelsInImage)
                {
                    varX.Value = (float)nX / fstBitmap.Width;
                    varY.Value = (float)nY / fstBitmap.Height;
                    Color clrPixel = fstBitmap.GetPixel(nX, nY);
                    byte nGrayScaleColor = (byte)((clrPixel.R + clrPixel.G + clrPixel.B) / 3);
                    float fExpectedResult = (float)((nGrayScaleColor / 128.0) - 1);
                    progProgram.Run();
                    float fActualResult = progProgram.Result;
                    float fDifference = Math.Abs(fExpectedResult - fActualResult);
                    progProgram.Fitness += fDifference;
                    byte nActualGrayScaleColor = (byte)((fActualResult + 1) * 128.0);
                }
            }*/
        }
    }

    public class FastBitmap
    {
        Color[,] m_arrColors;
        int m_nWidth;
        int m_nHeight;

        public int Width
        {
            get
            {
                return (this.m_nWidth);
            }
        }

        public int Height
        {
            get
            {
                return (this.m_nHeight);
            }
        }

        public FastBitmap(int nWidth, int nHeight)
        {
            this.m_nWidth = nWidth;
            this.m_nHeight = nHeight;
            this.m_arrColors = new Color[nWidth, nHeight];
        }

        public FastBitmap(Bitmap bmpBitmap)
            : this(bmpBitmap.Width, bmpBitmap.Height)
        {
            for (int nY = 0;
                 nY < bmpBitmap.Width;
                 nY++)
            {
                for (int nX = 0;
                     nX < bmpBitmap.Height;
                     nX++)
                {
                    this.m_arrColors[nX, nY] = bmpBitmap.GetPixel(nX, nY);
                }
            }
        }

        public Color GetPixel(int nX, int nY)
        {
            return (this.m_arrColors[nX, nY]);
        }
        
        public void SetPixel(int nX, int nY, Color clrColor)
        {
            this.m_arrColors[nX, nY] = clrColor;
        }

        public void SetPixel(int nX, int nY, byte nRed, byte nGreen, byte nBlue)
        {
            this.m_arrColors[nX, nY] = Color.FromArgb(nRed, nGreen, nBlue);
        }

        public void Draw(Graphics g, int nPosToDrawX, int nPosToDrawY, int nWidth, int nHeight)
        {
            Bitmap bmpToDraw = new Bitmap(this.m_nWidth, this.m_nHeight);
            for (int nY = 0;
                 nY < this.m_nHeight;
                 nY++)
            {
                for (int nX = 0;
                     nX < this.m_nWidth;
                     nX++)
                {
                    bmpToDraw.SetPixel(nX, nY, this.m_arrColors[nX, nY]);
                }
            }

            g.DrawImage(bmpToDraw, nPosToDrawX, nPosToDrawY, nWidth, nHeight);
        }
    }
}
