using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TreeLanguageEvolute;

namespace CellularAutomataTest
{
    public partial class CellularAutomatonForm : Form
    {
        #region Evolution properties

        BaseProgram progBestProgramInPreviousGeneration;
        string strLabelString;

        public CellularAutomatonForm()
        {
            InitializeComponent();
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
            engEngine.NumberOfPrograms = 100;
            engEngine.NumberOfIslands = 4;
            engEngine.MutationChance = 0.1f;
            engEngine.EnableParsimonyPressure = 0.0001F;
            engEngine.MigrationsPerMigrationGeneration = 4;
            engEngine.OnceInHowManyGenerationsTopIndividualsShouldBeEvaluated = 20;
            engEngine.SetFunctions = new FunctionType[] { new Add(), new Substract(), new Multiply(),
                new Divide(), new IfGreaterThenElse(), new Modulo(),
                new CellularAutomataTest.CAEvolution.GetEnergyAmount(),
                new CellularAutomataTest.CAEvolution.Set(),
                new CellularAutomataTest.CAEvolution.ConstructCellDown(),
                new CellularAutomataTest.CAEvolution.ConstructCellLeft(),
                new CellularAutomataTest.CAEvolution.ConstructCellRight(),
                new CellularAutomataTest.CAEvolution.ConstructCellUp(),
                new CellularAutomataTest.CAEvolution.LookDown(),
                new CellularAutomataTest.CAEvolution.LookLeft(),
                new CellularAutomataTest.CAEvolution.LookRight(),
                new CellularAutomataTest.CAEvolution.LookUp(),
                new CellularAutomataTest.CAEvolution.SellExistingCellDown(),
                new CellularAutomataTest.CAEvolution.SellExistingCellLeft(),
                new CellularAutomataTest.CAEvolution.SellExistingCellRight(),
                new CellularAutomataTest.CAEvolution.SellExistingCellUp(),
                new CellularAutomataTest.CAEvolution.SendEnergyDown(),
                new CellularAutomataTest.CAEvolution.SendEnergyLeft(),
                new CellularAutomataTest.CAEvolution.SendEnergyRight(),
                new CellularAutomataTest.CAEvolution.SendEnergyUp(),
                new CellularAutomataTest.CAEvolution.SetVar()};
            engEngine.SetValues = new TreeLanguageEvolute.ValueType[] { new RandomMacro(),
                new Value(0), new Value(1), new Value(2), new Value(3)};
            engEngine.DeclareVariable("a1");
            engEngine.DeclareVariable("a2");
            engEngine.DeclareVariable("a3");
            engEngine.DeclareVariable("a4");
            engEngine.DeclareVariable("a5");
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
            
            // Attempt to gather from 20 different tests.
            for (int nTestNum = 0; nTestNum < 5; nTestNum++)
            {
                dAbsoluteFitness += CAEvolution.ProgramRunner(progProgram,
                    progBestProgramInPreviousGeneration, null, false);
            }

            progProgram.Fitness = 100000 - dAbsoluteFitness;
        }

        public void engEngine_GenerationIsCompleteEvent(Statistics stsStatistics,
       BaseEngine sender)
        {

            strLabelString = "Generation num = " + stsStatistics.GenerationNumber + ", Min fitness = " +
                    stsStatistics.MinFitnessProgram.Fitness + ", Min fitness nodes = " + stsStatistics.MinFitnessProgram.Size;
            ((TreeProgram)stsStatistics.MinFitnessProgram).Draw(pnlDrawProgram.CreateGraphics(), pnlDrawProgram.Width, pnlDrawProgram.Height, this);

            if ((stsStatistics.GenerationNumber % 20) == 0)
            {
                CAEvolution.ProgramRunner(stsStatistics.MinFitnessProgram,
                progBestProgramInPreviousGeneration, pnlMatrixDisplay.CreateGraphics(), true);

                CAEvolution.ProgramRunner(stsStatistics.MinFitnessProgram,
                progBestProgramInPreviousGeneration, pnlMatrixDisplay.CreateGraphics(), true);

                CAEvolution.ProgramRunner(stsStatistics.MinFitnessProgram,
                progBestProgramInPreviousGeneration, pnlMatrixDisplay.CreateGraphics(), true);

            }

            progBestProgramInPreviousGeneration = stsStatistics.MinFitnessProgram;
        }

        #endregion

        private void CellularAutomatonForm_Resize(object sender, EventArgs e)
        {
            pnlDrawProgram.Width = this.Width - pnlDrawProgram.Location.X;
            pnlDrawProgram.Height = this.Height - pnlDrawProgram.Location.Y;
        }
    }
}
