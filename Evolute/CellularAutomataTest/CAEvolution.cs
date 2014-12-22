using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System;
using System.Threading;
using TreeLanguageEvolute;

namespace CellularAutomataTest
{
    public class CAEvolution
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
                    return ("MOV");
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

        public class GetEnergyAmount : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (1);
                }
            }

            public override string Name
            {
                get
                {
                    return ("MOV");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                float fDummy = arrNodes[0].ReturnValue(progOwner);

                CACell caCurrCell = ((CACell)progOwner.AdditionalInformation);
                return (caCurrCell.fAvaliableEnergy);
            }
        }

        public class SendEnergyRight : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (1);
                }
            }

            public override string Name
            {
                get
                {
                    return ("SendEnergyRight");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                CACell caCurrCell = ((CACell)progOwner.AdditionalInformation);
                float dEnergyToSubstract = (int)arrNodes[0].ReturnValue(progOwner);
                
                // Substract from the current cell and give to the other one.
                float dResult = SharedEvolutionFunctions.SendEnergy(caCurrCell, 
                    SharedEvolutionFunctions.DIRECTION_RIGHT, dEnergyToSubstract);

                return (dResult);
            }
        }

        public class SendEnergyLeft : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (1);
                }
            }

            public override string Name
            {
                get
                {
                    return ("SendEnergyLeft");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                CACell caCurrCell = ((CACell)progOwner.AdditionalInformation);
                float dEnergyToSubstract = (int)arrNodes[0].ReturnValue(progOwner);

                // Substract from the current cell and give to the other one.
                float dResult = SharedEvolutionFunctions.SendEnergy(caCurrCell,
                    SharedEvolutionFunctions.DIRECTION_LEFT, dEnergyToSubstract);

                return (dResult);
            }
        }

        public class SendEnergyUp : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (1);
                }
            }

            public override string Name
            {
                get
                {
                    return ("SendEnergyUp");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                CACell caCurrCell = ((CACell)progOwner.AdditionalInformation);
                float dEnergyToSubstract = (int)arrNodes[0].ReturnValue(progOwner);

                // Substract from the current cell and give to the other one.
                float dResult = SharedEvolutionFunctions.SendEnergy(caCurrCell,
                    SharedEvolutionFunctions.DIRECTION_UP, dEnergyToSubstract);

                return (dResult);
            }
        }

        public class SendEnergyDown : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (1);
                }
            }

            public override string Name
            {
                get
                {
                    return ("SendEnergyDown");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                CACell caCurrCell = ((CACell)progOwner.AdditionalInformation);
                float dEnergyToSubstract = (int)arrNodes[0].ReturnValue(progOwner);

                // Substract from the current cell and give to the other one.
                float dResult = SharedEvolutionFunctions.SendEnergy(caCurrCell,
                    SharedEvolutionFunctions.DIRECTION_DOWN, dEnergyToSubstract);

                return (dResult);
            }
        }

        public class ConstructCellRight : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (1);
                }
            }

            public override string Name
            {
                get
                {
                    return ("ConstructCellRight");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                CACell caCurrCell = ((CACell)progOwner.AdditionalInformation);
                float dEnergyToSubstract = (int)arrNodes[0].ReturnValue(progOwner);

                // Substract from the current cell and give to the other one.
                float dResult = SharedEvolutionFunctions.ConstructNewCell(caCurrCell,
                    SharedEvolutionFunctions.DIRECTION_RIGHT);

                return (dResult);
            }
        }

        public class ConstructCellLeft : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (1);
                }
            }

            public override string Name
            {
                get
                {
                    return ("ConstructCellLeft");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                CACell caCurrCell = ((CACell)progOwner.AdditionalInformation);
                float dEnergyToSubstract = (int)arrNodes[0].ReturnValue(progOwner);

                // Substract from the current cell and give to the other one.
                float dResult = SharedEvolutionFunctions.ConstructNewCell(caCurrCell,
                    SharedEvolutionFunctions.DIRECTION_LEFT);

                return (dResult);
            }
        }

        public class ConstructCellUp : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (1);
                }
            }

            public override string Name
            {
                get
                {
                    return ("ConstructCellUp");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                CACell caCurrCell = ((CACell)progOwner.AdditionalInformation);
                float dEnergyToSubstract = (int)arrNodes[0].ReturnValue(progOwner);

                // Substract from the current cell and give to the other one.
                float dResult = SharedEvolutionFunctions.ConstructNewCell(caCurrCell,
                    SharedEvolutionFunctions.DIRECTION_UP);

                return (dResult);
            }
        }

        public class ConstructCellDown : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (1);
                }
            }

            public override string Name
            {
                get
                {
                    return ("ConstructCellDown");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                CACell caCurrCell = ((CACell)progOwner.AdditionalInformation);
                float dEnergyToSubstract = (int)arrNodes[0].ReturnValue(progOwner);

                // Substract from the current cell and give to the other one.
                float dResult = SharedEvolutionFunctions.ConstructNewCell(caCurrCell,
                    SharedEvolutionFunctions.DIRECTION_DOWN);

                return (dResult);
            }
        }

        public class SellExistingCellRight : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (1);
                }
            }

            public override string Name
            {
                get
                {
                    return ("SellExistingCellRight");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                CACell caCurrCell = ((CACell)progOwner.AdditionalInformation);
                float dEnergyToSubstract = (int)arrNodes[0].ReturnValue(progOwner);

                // Substract from the current cell and give to the other one.
                float dResult = SharedEvolutionFunctions.SellExistingCell(caCurrCell,
                    SharedEvolutionFunctions.DIRECTION_RIGHT);

                return (dResult);
            }
        }

        public class SellExistingCellLeft : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (1);
                }
            }

            public override string Name
            {
                get
                {
                    return ("SellExistingCellLeft");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                CACell caCurrCell = ((CACell)progOwner.AdditionalInformation);
                float dEnergyToSubstract = (int)arrNodes[0].ReturnValue(progOwner);

                // Substract from the current cell and give to the other one.
                float dResult = SharedEvolutionFunctions.SellExistingCell(caCurrCell,
                    SharedEvolutionFunctions.DIRECTION_LEFT);

                return (dResult);
            }
        }

        public class SellExistingCellUp : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (1);
                }
            }

            public override string Name
            {
                get
                {
                    return ("SellExistingCellUp");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                CACell caCurrCell = ((CACell)progOwner.AdditionalInformation);
                float dEnergyToSubstract = (int)arrNodes[0].ReturnValue(progOwner);

                // Substract from the current cell and give to the other one.
                float dResult = SharedEvolutionFunctions.SellExistingCell(caCurrCell,
                    SharedEvolutionFunctions.DIRECTION_UP);

                return (dResult);
            }
        }

        public class SellExistingCellDown : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (1);
                }
            }

            public override string Name
            {
                get
                {
                    return ("SellExistingCellDown");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                CACell caCurrCell = ((CACell)progOwner.AdditionalInformation);
                float dEnergyToSubstract = (int)arrNodes[0].ReturnValue(progOwner);

                // Substract from the current cell and give to the other one.
                float dResult = SharedEvolutionFunctions.SellExistingCell(caCurrCell,
                    SharedEvolutionFunctions.DIRECTION_DOWN);

                return (dResult);
            }
        }

        public class LookRight : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (1);
                }
            }

            public override string Name
            {
                get
                {
                    return ("LookRight");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                CACell caCurrCell = ((CACell)progOwner.AdditionalInformation);
                float dEnergyToSubstract = (int)arrNodes[0].ReturnValue(progOwner);

                // Substract from the current cell and give to the other one.
                float dResult = SharedEvolutionFunctions.Look(caCurrCell,
                    SharedEvolutionFunctions.DIRECTION_RIGHT);

                return (dResult);
            }
        }

        public class LookLeft : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (1);
                }
            }

            public override string Name
            {
                get
                {
                    return ("LookLeft");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                CACell caCurrCell = ((CACell)progOwner.AdditionalInformation);
                float dEnergyToSubstract = (int)arrNodes[0].ReturnValue(progOwner);

                // Substract from the current cell and give to the other one.
                float dResult = SharedEvolutionFunctions.Look(caCurrCell,
                    SharedEvolutionFunctions.DIRECTION_LEFT);

                return (dResult);
            }
        }

        public class LookUp : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (1);
                }
            }

            public override string Name
            {
                get
                {
                    return ("LookUp");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                CACell caCurrCell = ((CACell)progOwner.AdditionalInformation);
                float dEnergyToSubstract = (int)arrNodes[0].ReturnValue(progOwner);

                // Substract from the current cell and give to the other one.
                float dResult = SharedEvolutionFunctions.Look(caCurrCell,
                    SharedEvolutionFunctions.DIRECTION_UP);

                return (dResult);
            }
        }

        public class LookDown : Function
        {
            public override int NumberOfArguments
            {
                get
                {
                    return (1);
                }
            }

            public override string Name
            {
                get
                {
                    return ("LookDown");
                }
            }

            public override float action(Node[] arrNodes, TreeLanguageEvolute.TreeProgram progOwner)
            {
                CACell caCurrCell = ((CACell)progOwner.AdditionalInformation);
                float dEnergyToSubstract = (int)arrNodes[0].ReturnValue(progOwner);

                // Substract from the current cell and give to the other one.
                float dResult = SharedEvolutionFunctions.Look(caCurrCell,
                    SharedEvolutionFunctions.DIRECTION_DOWN);

                return (dResult);
            }
        }

        public class SetVar : Function
        {

            public override float action(Node[] arrNodes, TreeProgram progOwner)
            {
                float dResult = arrNodes[0].ReturnValue(progOwner);
                int nWantedVariable = (int)arrNodes[1].ReturnValue(progOwner);

                // Set the wanted variable number, as long as variable index exists.
                if ((nWantedVariable > 0) && (nWantedVariable < progOwner.Variables.Length))
                {
                    progOwner.Variables[nWantedVariable].Value = dResult;
                    return dResult;
                }
                else
                {
                    return 0;
                }
            }

            public override int NumberOfArguments
            {
                get { return 2; }
            }

            public override string Name
            {
                get { return "SetVar"; }
            }
        }

        #endregion

        #region Evaulation functions

        static readonly Color PLAYER_COLOR = Color.Red;
        const int STARTING_ENERGY = 500;
        const int NUM_OTHER_PLAYERS = 2;

        public static CAMatrix InitialiseMatrix(BaseProgram progEvaulateProgram,
            BaseProgram progOpponents)
        {
            CAMatrix matMatrix = new CAMatrix();

            // Position player 0 on board
            ImprovedRandom rndRandom = GlobalRandom.m_rndRandom;
            int nMinXToGenerate = matMatrix.Width * 4 / 10;
            int nMaxXToGenerate = matMatrix.Width * 6 / 10;
            int nMinYToGenerate = matMatrix.Height * 4 / 10;
            int nMaxYToGenerate = matMatrix.Height * 6 / 10;
            matMatrix.CreateCell(rndRandom.Next(nMinXToGenerate, nMaxXToGenerate),
                rndRandom.Next(nMinYToGenerate, nMaxYToGenerate),
                progEvaulateProgram,
                PLAYER_COLOR, STARTING_ENERGY);

            // Create possible colors
            List<Color> arrColors = new List<Color>();
            arrColors.Add(Color.RoyalBlue);
            arrColors.Add(Color.Silver);
            arrColors.Add(Color.Salmon);

            // Position other players on board
            BaseProgram progOpponent0 = (BaseProgram)progOpponents.Clone();
            matMatrix.CreateCell(rndRandom.Next(nMinXToGenerate, nMaxXToGenerate),
                rndRandom.Next(nMinYToGenerate, nMaxYToGenerate),
                progOpponent0, arrColors[0], STARTING_ENERGY);
            BaseProgram progOpponent1 = (BaseProgram)progOpponents.Clone();
            matMatrix.CreateCell(rndRandom.Next(nMinXToGenerate, nMaxXToGenerate),
                rndRandom.Next(nMinYToGenerate, nMaxYToGenerate),
                progOpponent1, arrColors[1], STARTING_ENERGY);

            return matMatrix;
        }

        public static void RunTimeStep(CAMatrix matMatrix)
        {
            int nWidth = matMatrix.Width;
            int nHeight = matMatrix.Height;
            for (int x = 0; x < nWidth; x++)
            {
                for (int y = 0; y < nHeight; y++)
                {
                    CACell celCurrCell = matMatrix.GetCell(x, y);

                    // If the cell has nothing in it, skip it..
                    if (celCurrCell == null)
                    {
                        continue;
                    }

                    // The cell has a program in it. run it.
                    celCurrCell.progProgram.Run(celCurrCell);
                }
            }

            // Deleting cells
            for (int i = 0; i < matMatrix.lstCellDeletesForNextTime.Count; i++)
            {
                matMatrix.DestroyCell(matMatrix.lstCellDeletesForNextTime[i].X, 
                    matMatrix.lstCellDeletesForNextTime[i].Y);
            }

            matMatrix.lstCellDeletesForNextTime.Clear();

            // Applying new cells waiting to creation
            for (int i = 0; i < matMatrix.lstNewCellsForNextTime.Count; i++)
            {
                matMatrix.PutCell(matMatrix.lstNewCellsForNextTime[i]);
            }

            matMatrix.lstNewCellsForNextTime.Clear();

            // Transferring energies
            for (int i = 0; i < matMatrix.lstCellEnergyTransfersOne.Count; i++)
            {
                EnergyTransfer trnTransfer = matMatrix.lstCellEnergyTransfersOne[i];
                if (trnTransfer.celFrom.fAvaliableEnergy >= trnTransfer.dEnergy)
                {
                    trnTransfer.celFrom.fAvaliableEnergy -= trnTransfer.dEnergy;
                    trnTransfer.celTo.fAvaliableEnergy += trnTransfer.dEnergy;
                }
            }

            matMatrix.lstCellEnergyTransfersOne.Clear();
            
        }

        public static float GetFitnessForMatrix(CAMatrix matMatrix)
        {
            float dTotalFitness = 0;

            for (int x = 0; x < matMatrix.Width; x++)
            {
                for (int y = 0; y < matMatrix.Width; y++)
                {
                    CACell celCurrCell = matMatrix.GetCell(x, y);

                    // If the cell has nothing in it, skip it..
                    if (celCurrCell == null)
                    {
                        continue;
                    }

                    if (celCurrCell.clrTeamColor == PLAYER_COLOR)
                    {
                        dTotalFitness += 10 + celCurrCell.fAvaliableEnergy;
                    }
                }
            }

            return dTotalFitness;
        }

        public static void DisplayMatrix(CAMatrix matMatrix, Graphics grpGraphics)
        {

            for (int x = 0; x < matMatrix.Width; x++)
            {
                for (int y = 0; y < matMatrix.Width; y++)
                {
                    CACell celCurrCell = matMatrix.GetCell(x, y);

                    // If the cell has nothing in it, skip it..
                    if (celCurrCell == null)
                    {
                        grpGraphics.FillRectangle(new SolidBrush(Color.Black),
                            x * 10, y * 10, 10, 10);
                        continue;
                    }

                    Color clrTeamColor = celCurrCell.clrTeamColor;
                    float fEnergy = celCurrCell.fAvaliableEnergy;
                    int nWantedRed = (int)((clrTeamColor.R / 4) + (fEnergy * 30));
                    int nWantedGreen = (int)((clrTeamColor.G / 4) + (fEnergy * 30));
                    int nWantedBlue = (int)((clrTeamColor.B / 4) + (fEnergy * 30));
                    int nRed = Math.Min(nWantedRed, 255);
                    int nGreen = Math.Min(nWantedGreen, 255);
                    int nBlue = Math.Min(nWantedBlue, 255);
                    Color clrCombinedWithEnergy = Color.FromArgb(nRed, nGreen, nBlue);
                    grpGraphics.FillRectangle(new SolidBrush(clrCombinedWithEnergy),
                        x * 10, y * 10, 10, 10);
                }
            }
        }

        public static float ProgramRunner(BaseProgram progEvaulateProgram,
            BaseProgram progOpponents, Graphics grpGraphics, bool fDisplayProgress)
        {
            // Reset the variables of all the programs.
            for (int i = 0; i < progEvaulateProgram.Variables.Length; i++)
            {
                progEvaulateProgram.Variables[i].Value = 0;
            }

            CAMatrix matMatrix = InitialiseMatrix(progEvaulateProgram, progOpponents);

            // Running the matrix players.
            for (int i = 0; i < 200; i++)
            {
                if (fDisplayProgress == true)
                {
                    DisplayMatrix(matMatrix, grpGraphics);
                    Thread.Sleep(100);
                }

                RunTimeStep(matMatrix);
            }

            return GetFitnessForMatrix(matMatrix);
        }

        #endregion
    }
}