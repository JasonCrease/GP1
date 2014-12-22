using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ChaserAndRunner
{
    public class World
    {
        public Bitmap m_cBackgroundImage;
        public FastBitmap m_cBackgroundImageFast;

        public World(Bitmap cBackgroundImage)
        {
            m_cBackgroundImage = cBackgroundImage;
            m_cBackgroundImageFast = new FastBitmap(m_cBackgroundImage);
        }

        public int GetWorldWidth()
        {
            lock (m_cBackgroundImage)
            {
                return m_cBackgroundImage.Width;
            }
        }

        public int GetWorldHeight()
        {
            lock (m_cBackgroundImage)
            {
                return m_cBackgroundImage.Height;
            }
        }

        public Bitmap GetBackground()
        {
            return m_cBackgroundImage;
        }

        public bool CheckIfPositionIsBlocked(PointF pntPosition)
        {
            // Check if the player is within bounds. If not, then return that the player is blocked.
            if ((pntPosition.X < 0) ||
                (pntPosition.X >= m_cBackgroundImageFast.Width) ||
                (pntPosition.Y < 0) ||
                (pntPosition.Y >= m_cBackgroundImageFast.Height))
            {
                return true;
            }

            Color clrColorInPosition = m_cBackgroundImageFast.GetPixel((int)pntPosition.X, (int)pntPosition.Y);
            return ((clrColorInPosition.R == 0) && (clrColorInPosition.G == 0) && (clrColorInPosition.B == 0));
        }
    }
}
