using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TreeLanguageEvolute;

namespace ChaserAndRunner
{
    public partial class EvolutionForm : Form
    {
       #region Evolution properties

        BaseProgram progBestProgramInPreviousGeneration;
        string strLabelString;
        World m_cWorld;

        public EvolutionForm()
        {
            InitializeComponent();

            m_cWorld = new World(new Bitmap("racetrack.png"));

            GeneticProgrammingEngine engEngine = new GeneticProgrammingEngine();
            engEngine.MinInitialTreeDepth = 2;
            engEngine.MaxInitialTreeDepth = 4;
            engEngine.MaxOverallTreeDepth = 30;
            engEngine.FitnessGoal = BaseEngine.FitnessGoalEnum.MIN_FITNESS;
            engEngine.Overselection = 2;
            engEngine.ChanceForCrossover = 0.9F;
            engEngine.SaveTopIndividuals = 0.1F;
            engEngine.NumberOfThreads = 4;
            engEngine.OnlyBetterFitnessIslands = false;
            engEngine.NumberOfPrograms = 5000;
            engEngine.NumberOfIslands = 25;
            engEngine.MutationChance = 0.1f;
            engEngine.EnableParsimonyPressure = 0.001F;
            engEngine.MigrationsPerMigrationGeneration = 25;
            engEngine.OnceInHowManyGenerationsTopIndividualsShouldBeEvaluated = 5;
            engEngine.SetFunctions = new FunctionType[] { new Add(), new Substract(), new Multiply(),
                new Divide(), new IfGreaterThenElse(), new Modulo(),
                new ChaserAndRunner.RunnerEvolution.Set(),
                new ChaserAndRunner.RunnerEvolution.RotateRight(),
                new ChaserAndRunner.RunnerEvolution.RotateLeft(),
                new ChaserAndRunner.RunnerEvolution.Accelerate(),
                new ChaserAndRunner.RunnerEvolution.DistanceAhead(),
                new ChaserAndRunner.RunnerEvolution.Distance30DegreesLeft(),
                new ChaserAndRunner.RunnerEvolution.Distance30DegreesRight(),
                new ChaserAndRunner.RunnerEvolution.GetSpeed()
            };
            engEngine.SetValues = new TreeLanguageEvolute.ValueType[] { new RandomMacro()};
            engEngine.DeclareVariable("a1");
            engEngine.DeclareVariable("a2");
            engEngine.DeclareVariable("a3");
            engEngine.EvalFitnessForProgramEvent +=
                new GeneticProgrammingEngine.EvalFitnessHandler(engEngine_EvalFitnessForProgramEvent);
            engEngine.GenerationIsCompleteEvent +=
                new GeneticProgrammingEngine.GenerationIsCompleteHandler(engEngine_GenerationIsCompleteEvent);
            engEngine.RunEvoluteOnThread(100000);
            Timer tmTimer = new Timer();
            tmTimer.Interval = 1000;
            tmTimer.Tick += new EventHandler(tmTimer_Tick);
            tmTimer.Start();
        }

        void tmTimer_Tick(object sender, EventArgs e)
        {
            this.label1.Text = strLabelString;
        }

        public void engEngine_EvalFitnessForProgramEvent(BaseProgram progProgram,
                  BaseEngine sender)
        {
            if (progBestProgramInPreviousGeneration == null)
            {
                progBestProgramInPreviousGeneration = sender.Population[0][0];
            }

            float dAbsoluteFitness = 0;
            
            // Attempt to gather from 10 different tests.
            float dWorstCaseScenario = 99999999;
            for (int nTestNum = 0; nTestNum < 5; nTestNum++)
            {
                float dFitness = RunnerEvolution.ProgramRunner(m_cWorld, progProgram,
                    null, false);
                if (dFitness < dWorstCaseScenario)
                {
                    dWorstCaseScenario = dFitness;
                }

               /* dAbsoluteFitness += RunnerEvolution.ProgramRunner(m_cWorld, progProgram,
                    null, false);*/
            }

            progProgram.Fitness = 1000000 - dWorstCaseScenario;
             
        }

        public void engEngine_GenerationIsCompleteEvent(Statistics stsStatistics,
       BaseEngine sender)
        {
            strLabelString = "Generation num = " + stsStatistics.GenerationNumber + ", Min fitness = " +
                    stsStatistics.MinFitnessProgram.Fitness + ", Min fitness nodes = " + stsStatistics.MinFitnessProgram.Size;
            ((TreeProgram)stsStatistics.MinFitnessProgram).Draw(pnlDrawProgram.CreateGraphics(), pnlDrawProgram.Width, pnlDrawProgram.Height, this);

           if (((stsStatistics.GenerationNumber % 10) == 0) && (stsStatistics.GenerationNumber != 0))
            {
                for (int i = 0; i < 5; i++)
                {
                    RunnerEvolution.ProgramRunner(m_cWorld, stsStatistics.MinFitnessProgram,
                        pnlWorldView.CreateGraphics(), true);
                }
            }


            // Show statistics about the islands. Make a graph about the islands fitnesses.
            progBestProgramInPreviousGeneration = stsStatistics.MinFitnessProgram;

            float dMinimalFitnessInPopulation = stsStatistics.MinFitnessProgram.Fitness;
            float dMaxMinFitnessInIslands = 0;
            for (int i = 0; i < sender.NumberOfIslands; i++)
            {
                BaseProgram cMinFitnessProgramForIsland = stsStatistics.GetMinFitnessProgramForIsland(i);
                if (cMinFitnessProgramForIsland.Fitness > dMaxMinFitnessInIslands)
                {
                    dMaxMinFitnessInIslands = cMinFitnessProgramForIsland.Fitness;
                }
            }

            int ISLAND_BAR_WIDTH = (int)(pnlViewIslands.Width / sender.NumberOfIslands);
            Graphics g = pnlViewIslands.CreateGraphics();
            g.Clear(Color.Black);
            for (int i = 0; i < sender.NumberOfIslands; i++)
            {
                float dIslandFitness = stsStatistics.GetMinFitnessProgramForIsland(i).Fitness;
                float dPrecentageOfMinFitness = (dIslandFitness - dMinimalFitnessInPopulation) / (dMaxMinFitnessInIslands - dMinimalFitnessInPopulation);
                g.FillRectangle(Brushes.RoyalBlue, i * ISLAND_BAR_WIDTH, pnlViewIslands.Height - (pnlViewIslands.Height * dPrecentageOfMinFitness), ISLAND_BAR_WIDTH, pnlViewIslands.Height);
            }

            // Write the max and min fitnesses value on the graph.
            g.DrawString(dMaxMinFitnessInIslands.ToString(), new Font("Times New Roman", 12.0f), Brushes.Red, 0, 0);
            g.DrawString(dMinimalFitnessInPopulation.ToString(), new Font("Times New Roman", 12.0f), Brushes.Red, 0, pnlViewIslands.Height - 20);
        }

        #endregion

        private void EvolutionForm_Resize(object sender, EventArgs e)
        {
            pnlDrawProgram.Width = this.Width - pnlDrawProgram.Location.X;
            pnlDrawProgram.Height = this.Height - pnlDrawProgram.Location.Y;
        }
    }
}
