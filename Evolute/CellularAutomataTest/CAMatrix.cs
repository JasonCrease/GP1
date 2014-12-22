using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreeLanguageEvolute;
using System.Collections;
using System.Drawing;

namespace CellularAutomataTest
{
    public class CACell
    {
        public BaseProgram progProgram;
        public Hashtable hsPreviousParameters;
        public float fAvaliableEnergy;
        public CAMatrix matParentMatrix;
        public int x;
        public int y;
        public Color clrTeamColor;
    }

    public struct EnergyTransfer
    {
        public CACell celFrom;
        public CACell celTo;
        public float dEnergy;
    }

    public class CAMatrix
    {
        private const int WIDTH = 30;
        private const int HEIGHT = 30;
        private CACell[][] matMatrix;
        public List<CACell> lstNewCellsForNextTime = new List<CACell>();
        public List<Point> lstCellDeletesForNextTime = new List<Point>();
        public List<EnergyTransfer> lstCellEnergyTransfersOne = new List<EnergyTransfer>();

        public CAMatrix()
        {
            matMatrix = new CACell[WIDTH][];
            for (int i = 0; i < WIDTH; i++)
            {
                matMatrix[i] = new CACell[HEIGHT];
            }
        }

        public CACell GetCell(int x, int y)
        {
            return matMatrix[x][y];
        }

        public void CreateCell(int x, int y, BaseProgram progProgram, Color clrTeamColor, float fAvaliableEnergy)
        {
            CACell celCell = new CACell();
            celCell.progProgram = progProgram;
            celCell.clrTeamColor = clrTeamColor;
            celCell.fAvaliableEnergy = fAvaliableEnergy;
            celCell.x = x;
            celCell.y = y;
            celCell.matParentMatrix = this;
            lstNewCellsForNextTime.Add(celCell);
        }

        public void PutCell(CACell celCell)
        {
            celCell.matParentMatrix = this;
            matMatrix[celCell.x][celCell.y] = celCell;
        }

        public void DestroyCell(int x, int y)
        {
            matMatrix[x][y] = null;
        }

        public bool IsLocationValid(int x, int y)
        {
            return (
                    (x >= 0) && (x < this.Width) &&
                    (y >= 0) && (y < this.Height)
                    );
        }

        public int Width
        {
            get
            {
                return WIDTH;
            }
        }

        public int Height
        {
            get
            {
                return HEIGHT;
            }
        }
    }
}
