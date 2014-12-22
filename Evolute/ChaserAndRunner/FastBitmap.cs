using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ChaserAndRunner
{
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
                 nY < bmpBitmap.Height;
                 nY++)
            {
                for (int nX = 0;
                     nX < bmpBitmap.Width;
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
