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

namespace TestProject
{
    public partial class FunctionGuesserTestForm : Form
    {
        string strLabelString;
        public FunctionGuesserTestForm()
        {
            InitializeComponent();
            GeneticProgrammingEngine engEngine = new GeneticProgrammingEngine();
            engEngine.MinInitialTreeDepth = 2;
            engEngine.MaxInitialTreeDepth = 4;
            engEngine.MaxOverallTreeDepth = 15;
            engEngine.FitnessGoal = BaseEngine.FitnessGoalEnum.MIN_FITNESS;
            engEngine.Overselection = 2;
            engEngine.ChanceForCrossover = 0.9F;
            engEngine.SaveTopIndividuals = 0.1F;
            engEngine.NumberOfThreads = 4;
            engEngine.OnlyBetterFitnessIslands = false;
            engEngine.NumberOfPrograms = 1000;
            engEngine.NumberOfIslands = 100;
            engEngine.MutationChance = 0.1f;
            engEngine.EnableParsimonyPressure = 0.001f;
            engEngine.MigrationsPerMigrationGeneration = 10;
            engEngine.SetFunctions = new FunctionType[]
            { new Add(), new Substract(), new Multiply(), new Divide() };
            engEngine.DeclareVariable("X");
            engEngine.SetValues = new TreeLanguageEvolute.ValueType[] {new RandomMacro()};
            engEngine.EvalFitnessForProgramEvent += new GeneticProgrammingEngine.EvalFitnessHandler(engEngine_EvalFitnessForProgramEvent);
            engEngine.GenerationIsCompleteEvent += new GeneticProgrammingEngine.GenerationIsCompleteHandler(engEngine_GenerationIsCompleteEvent);
            engEngine.RunEvoluteOnThread(10000);

            Timer tmTimer = new Timer();
            tmTimer.Interval = 1000;
            tmTimer.Tick += new EventHandler(tmTimer_Tick);
            tmTimer.Start();
        }

        void tmTimer_Tick(object sender, EventArgs e)
        {
            this.label1.Text = strLabelString;
        }

        void engEngine_GenerationIsCompleteEvent(Statistics stsStatistics, BaseEngine sender)
        {
            this.strLabelString = "Generation number " + stsStatistics.GenerationNumber
                                + " : min fitness = " + stsStatistics.MinFitnessProgram.Fitness
                                + ", Total nodes = " + stsStatistics.TotalNodes;
            try
            {
                Graphics g = this.CreateGraphics();
                ((TreeProgram)stsStatistics.MinFitnessProgram).Draw(g, this.Width, this.Height, this);
            }
            catch (System.Exception ex)
            {  
            }
        }

        void engEngine_EvalFitnessForProgramEvent(TreeLanguageEvolute.TreeProgram progProgram, GeneticProgrammingEngine sender)
        {
            progProgram.Fitness = 0;
            Hashtable hsVariables = progProgram.GetVariables();
            Variable varX = (Variable)hsVariables["X"];

            for (int nCaseNumber = 0;
                 nCaseNumber < 100;
                 nCaseNumber++)
            {
                // y = 5x^3+x^2+x
                varX.Value = (float)GlobalRandom.m_rndRandom.NextDouble();
                float fExpectedResult = (5 * varX.Value * varX.Value * varX.Value)
                    + (3 * varX.Value * varX.Value) + varX.Value;
                progProgram.Run();
                float fActualResult = progProgram.Result;
                progProgram.Fitness += Math.Abs(fExpectedResult - fActualResult);
            }

             if (progProgram.Fitness < 0.1F)
             {
                 progProgram.Fitness =
                     0.00000001F * progProgram.Count; 
             }
        }
    }
}
