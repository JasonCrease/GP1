using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreeLanguageEvolute;
using System.Collections;
using System.Drawing;
using System.Threading;

namespace ChaserAndRunner
{
    class RunnerEvolution
    {
        #region Functions classes

        public class Set : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (2);
                }
            }

            public override string Name
            {
                get
                {
                    return ("SET");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                Hashtable hsVariables = progOwner.GetVariables();
                float fWantedVariable = arrNodes[0].ReturnValue(progOwner);
                float fWantedValue = arrNodes[1].ReturnValue(progOwner);
                uint nWantedVariableIndex = ((uint)fWantedVariable) % (uint)hsVariables.Count;

                Variable varWantedVariable = progOwner.Variables[nWantedVariableIndex];
                varWantedVariable.Value = fWantedValue;

                return (1);
            }
        }

        public class RotateRight : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (0);
                }
            }

            public override string Name
            {
                get
                {
                    return ("RGT");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                Runner cRunner = ((Runner)progOwner.AdditionalInformation);
                cRunner.QueueRotateRight();
                return (0);
            }
        }

        public class RotateLeft : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (0);
                }
            }

            public override string Name
            {
                get
                {
                    return ("LFT");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                Runner cRunner = ((Runner)progOwner.AdditionalInformation);
                cRunner.QueueRotateLeft();
                return (0);
            }
        }

        public class Accelerate : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (0);
                }
            }

            public override string Name
            {
                get
                {
                    return ("ACT");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                Runner cRunner = ((Runner)progOwner.AdditionalInformation);
                cRunner.QueueAccelerate();
                return (0);
            }
        }

        public class DistanceAhead : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (0);
                }
            }

            public override string Name
            {
                get
                {
                    return ("SN0");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                Runner cRunner = ((Runner)progOwner.AdditionalInformation);
                return cRunner.GetDistanceAhead();
            }
        }

        public class Distance30DegreesLeft : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (0);
                }
            }

            public override string Name
            {
                get
                {
                    return ("SN-30");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                Runner cRunner = ((Runner)progOwner.AdditionalInformation);
                return cRunner.GetDistance30DegreesLeft();
            }
        }

        public class Distance30DegreesRight : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (0);
                }
            }

            public override string Name
            {
                get
                {
                    return ("SN+30");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                Runner cRunner = ((Runner)progOwner.AdditionalInformation);
                return cRunner.GetDistance30DegreesRight();
            }
        }

        public class GetSpeed : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (0);
                }
            }

            public override string Name
            {
                get
                {
                    return ("SPD");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                Runner cRunner = ((Runner)progOwner.AdditionalInformation);
                return cRunner.GetSpeed();
            }
        }
        
        #endregion

        #region Evaluation Functions

        public static void DisplayWorld(World cWorld, Runner cRunner, Graphics g)
        {
             // Draw the player and his sensors.
            Bitmap cTempBitmap = new Bitmap(cWorld.GetWorldWidth(), cWorld.GetWorldHeight());
            Graphics gGraphicsOnBitmap = Graphics.FromImage(cTempBitmap);
            gGraphicsOnBitmap.Clear(Color.Black);
            gGraphicsOnBitmap.DrawImageUnscaled(cWorld.GetBackground(), 0, 0);
            PointF cPlayerPosition = cRunner.GetPosition();
            Vector cPlayerRotation = cRunner.GetRotation();
            gGraphicsOnBitmap.FillEllipse(Brushes.Red, cPlayerPosition.X - 5, cPlayerPosition.Y - 5, 10, 10);
            float dDistanceToIntersection = IntersectorFinder.FindIntersectionOnBitmap(cWorld.m_cBackgroundImageFast, cPlayerPosition, cPlayerRotation);
            gGraphicsOnBitmap.DrawLine(Pens.Red, cPlayerPosition, Vector.GetEndPointFromStartPoint(cPlayerPosition, cPlayerRotation.Multiply(dDistanceToIntersection)));
            gGraphicsOnBitmap.DrawLine(Pens.Red, cPlayerPosition, Vector.GetEndPointFromStartPoint(cPlayerPosition, cPlayerRotation.Rotate30Degrees().Multiply(30)));
            gGraphicsOnBitmap.DrawLine(Pens.Red, cPlayerPosition, Vector.GetEndPointFromStartPoint(cPlayerPosition, cPlayerRotation.RotateMinus30Degrees().Multiply(30)));
            
            g.DrawImageUnscaled(cTempBitmap, 0, 0);
            cTempBitmap.Dispose();
            gGraphicsOnBitmap.Dispose();
        }

        public static float ProgramRunner(World cWorld, BaseProgram progEvaulateProgram,
           Graphics grpGraphics, bool fDisplayProgress)
        {
            // Reset the variables of all the programs.
            for (int i = 0; i < progEvaulateProgram.Variables.Length; i++)
            {
                progEvaulateProgram.Variables[i].Value = 0;
            }

            ImprovedRandom rndRandom = GlobalRandom.m_rndRandom;
            bool fFoundGoodPlayerPosition = false;
            float dPlayerInitialPositionX = 0;
            float dPlayerInitialPositionY = 0;
            while (fFoundGoodPlayerPosition == false)
            {
                dPlayerInitialPositionX = rndRandom.Next(0, cWorld.GetWorldWidth());
                dPlayerInitialPositionY = rndRandom.Next(0, cWorld.GetWorldHeight());
                if (cWorld.CheckIfPositionIsBlocked(new PointF(dPlayerInitialPositionX, dPlayerInitialPositionY)) == false)
                {
                    fFoundGoodPlayerPosition = true;
                }
            }

            Runner cRunner = new Runner(new PointF(dPlayerInitialPositionX, dPlayerInitialPositionY));
            float dTotalPassedDistnace = 0;

            // Running the matrix players.
            int nNumberOfIterations = 100;
            if (fDisplayProgress == true)
                nNumberOfIterations = 200;

            int nNumOfTimesPlayerHadDistanceZero = 0;
            int nNumOfTimesPlayerWasBlocked = 1;

            for (int i = 0; i < nNumberOfIterations; i++)
            {
                if (fDisplayProgress == true)
                {
                    DisplayWorld(cWorld, cRunner, grpGraphics);
                    Thread.Sleep(30);
                }

                PointF pntLastRunnerPosition = cRunner.GetPosition();

                progEvaulateProgram.Run(cRunner);
                cRunner.RunTimestep(cWorld);
                PointF pntCurrRunnerPosition = cRunner.GetPosition();
                float dPassedDistnace = new Vector(pntLastRunnerPosition, pntCurrRunnerPosition).Distance();

                // If the player did not move, it's gonna cost him.
                if (dPassedDistnace == 0f)
                {
                    nNumOfTimesPlayerHadDistanceZero++;
                }

                if (cRunner.WasBlockedThisTurn() == true)
                {
                    nNumOfTimesPlayerWasBlocked++;
                }

                dTotalPassedDistnace += dPassedDistnace;
            }

            //dTotalPassedDistnace *= (1 - ((float)nNumOfTimesPlayerHadDistanceZero / nNumberOfIterations));
            float dDivision = nNumOfTimesPlayerWasBlocked;
            dTotalPassedDistnace /= dDivision;

            return dTotalPassedDistnace;
        }

        #endregion
    }
}
