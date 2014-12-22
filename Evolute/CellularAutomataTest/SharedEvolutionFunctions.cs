using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using TreeLanguageEvolute;

namespace CellularAutomataTest
{
    public class SharedEvolutionFunctions
    {
        const float ENERGY_NEEDED_FOR_CONSTRUCTION = 1F;
        const float ENERGY_GIVEN_BY_SELLING = 1F;
        public static readonly Direction DIRECTION_UP = new Direction(0, -1);
        public static readonly Direction DIRECTION_DOWN = new Direction(0, 1);
        public static readonly Direction DIRECTION_RIGHT = new Direction(1, 0);
        public static readonly Direction DIRECTION_LEFT = new Direction(-1, 0);

        public class Direction
        {
            public int xDir;
            public int yDir;

            public Direction(int xDir, int yDir)
            {
                this.xDir = xDir;
                this.yDir = yDir;
            }
        }

        public class Position
        {
            public int x;
            public int y;

            public Position(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public static float SendEnergy(CACell celCurrCell, Direction dirDirection, float fWantedEnergy)
        {
            float fTransferringEnergy = fWantedEnergy;

            // Check that the energy to transmit is above 0.
            if (fWantedEnergy <= 0)
            {
                return 3;
            }

            // If there is no energy left, then spend all the energy you can to send
            // The wanted energy.
            if (celCurrCell.fAvaliableEnergy < fWantedEnergy)
            {
                fTransferringEnergy = celCurrCell.fAvaliableEnergy;
            }

            int nNextCellX = celCurrCell.x + dirDirection.xDir;
            int nNextCellY = celCurrCell.y + dirDirection.yDir;

            // Test if the location is valid
            if (!(celCurrCell.matParentMatrix.IsLocationValid(nNextCellX, nNextCellY)))
            {
                return 2;
            }

            CACell celNeighbourCell = celCurrCell.matParentMatrix.GetCell(nNextCellX, nNextCellY);
            // Test if there's a cell there.
            if (celNeighbourCell == null)
            {
                return 0;
            }

            // Assign an energy transfer from cell one to the new cell
            EnergyTransfer trnTransfer = new EnergyTransfer();
            trnTransfer.celFrom = celCurrCell;
            trnTransfer.celTo = celNeighbourCell;
            trnTransfer.dEnergy = fTransferringEnergy;
            celCurrCell.matParentMatrix.lstCellEnergyTransfersOne.Add(trnTransfer);

            return 1;
        }

        public static float ConstructNewCell(CACell celCurrCell, Direction dirDirection)
        {
            // If there is no energy left, don't do it.
            if (celCurrCell.fAvaliableEnergy < ENERGY_NEEDED_FOR_CONSTRUCTION)
            {
                return 3;
            }

            int nNextCellX = celCurrCell.x + dirDirection.xDir;
            int nNextCellY = celCurrCell.y + dirDirection.yDir;

            // Test if the location is valid
            if (!(celCurrCell.matParentMatrix.IsLocationValid(nNextCellX, nNextCellY)))
            {
                return 2;
            }

            // Test if there's an already cell there :
            if (celCurrCell.matParentMatrix.GetCell(nNextCellX, nNextCellY) != null)
            {
                return 0;
            }

            // Clone the current program, assigning it the same "TeamID" :
            // The new cell would have zero energy.
            BaseProgram progClonedProgram = (BaseProgram)celCurrCell.progProgram.Clone();
            celCurrCell.matParentMatrix.CreateCell(nNextCellX, nNextCellY, progClonedProgram,
                celCurrCell.clrTeamColor, 0);

            // Substract the total energy the cell has.
            celCurrCell.fAvaliableEnergy -= ENERGY_NEEDED_FOR_CONSTRUCTION;

            return 1;
        }

        public static float SellExistingCell(CACell celCurrCell, Direction dirDirection)
        {
            int nNextCellX = celCurrCell.x + dirDirection.xDir;
            int nNextCellY = celCurrCell.y + dirDirection.yDir;

            // Test if the location is valid
            if (!(celCurrCell.matParentMatrix.IsLocationValid(nNextCellX, nNextCellY)))
            {
                return 2;
            }

            CACell celNeighbourCell = celCurrCell.matParentMatrix.GetCell(nNextCellX, nNextCellY);

            // Find out if there's a cell there
            if (celNeighbourCell == null)
            {
                return 0;
            }

            // Assign an energy transfer from deleted cell to the new cell
            EnergyTransfer trnTransfer = new EnergyTransfer();
            trnTransfer.celFrom = celNeighbourCell;
            trnTransfer.celTo = celCurrCell;
            trnTransfer.dEnergy = ENERGY_GIVEN_BY_SELLING;
            celCurrCell.matParentMatrix.lstCellEnergyTransfersOne.Add(trnTransfer);

            // Assign a cell delete
            celCurrCell.matParentMatrix.lstCellDeletesForNextTime.Add(
                new Point(nNextCellX, nNextCellY));

            return 1;
        }

        public static float Look(CACell celCurrCell, Direction dirDirection)
        {
            int nNextCellX = celCurrCell.x + dirDirection.xDir;
            int nNextCellY = celCurrCell.y + dirDirection.yDir;

            // Test if the location is valid
            if (!(celCurrCell.matParentMatrix.IsLocationValid(nNextCellX, nNextCellY)))
            {
                return 2;
            }

            // Find out if there's a cell there
            CACell celNeighbourCell = celCurrCell.matParentMatrix.GetCell(nNextCellX, nNextCellY);
            if (celNeighbourCell == null)
            {
                return 3;
            }

            // Is it the same team as mine? or is the neighbour cell on the other team?
            if (celCurrCell.clrTeamColor == celNeighbourCell.clrTeamColor)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
