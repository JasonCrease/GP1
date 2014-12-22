using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChaserAndRunner
{
    public partial class PlayTest : Form
    {
        PointF m_cPlayerPosition = new PointF(300, 100);
        Vector m_cPlayerRotation = new Vector(1, 1);
        Vector m_cDirectionVector = new Vector(0, 0);
        const float SPEED_ADDITION = 0.1f;
        Bitmap m_cBackgroundImage = new Bitmap("racetrack.png");
        FastBitmap m_cBackgroundImageFast;

        bool m_fPressingUp = false;
        bool m_fPressingDown = false;
        bool m_fPressingRight = false;
        bool m_fPressingLeft = false;

        public PlayTest()
        {
            InitializeComponent();

            m_cBackgroundImageFast = new FastBitmap(m_cBackgroundImage);

            Timer cTimer = new Timer();
            cTimer.Interval = 50;
            cTimer.Tick += new EventHandler(cTimer_Tick);
            cTimer.Start();
        }

        void cTimer_Tick(object sender, EventArgs e)
        {
            // Move the player in it's direction.
            m_cPlayerPosition = Vector.GetEndPointFromStartPoint(m_cPlayerPosition, m_cDirectionVector);

            // Make sure that the player doesn't exit walls.
            if (m_cPlayerPosition.X < 0)
                m_cPlayerPosition.X = 0;
            if (m_cPlayerPosition.X >= this.Width)
                m_cPlayerPosition.X = this.Width - 1;
            if (m_cPlayerPosition.Y < 0)
                m_cPlayerPosition.Y = 0;
            if (m_cPlayerPosition.Y >= this.Height)
                m_cPlayerPosition.Y = this.Height - 1;

            if (m_fPressingRight == true)
            {
                m_cPlayerRotation = m_cPlayerRotation.Rotate5Degrees().Normalize();
            }
            if (m_fPressingLeft == true)
            {
                m_cPlayerRotation = m_cPlayerRotation.RotateMinus5Degrees().Normalize();
            }
            if (m_fPressingUp == true)
            {
                m_cDirectionVector.Add(m_cPlayerRotation.Multiply(SPEED_ADDITION));
            }
            if (m_fPressingDown == true)
            {
                m_cDirectionVector.Substract(m_cPlayerRotation.Multiply(SPEED_ADDITION));
            }

            // Draw the player and his sensors.
            Graphics g = this.CreateGraphics();
            Bitmap cTempBitmap = new Bitmap(this.Width, this.Height);
            Graphics gGraphicsOnBitmap = Graphics.FromImage(cTempBitmap);
            gGraphicsOnBitmap.Clear(Color.Black);
            gGraphicsOnBitmap.DrawImageUnscaled(m_cBackgroundImage, 0, 0);
            gGraphicsOnBitmap.FillEllipse(Brushes.Red, m_cPlayerPosition.X - 5, m_cPlayerPosition.Y - 5, 10, 10);
            float dDistanceToIntersectionAhead = IntersectorFinder.FindIntersectionOnBitmap(m_cBackgroundImageFast, m_cPlayerPosition, m_cPlayerRotation);
            float dDistanceToIntersection30DegreesRight = IntersectorFinder.FindIntersectionOnBitmap(m_cBackgroundImageFast, m_cPlayerPosition, m_cPlayerRotation.Rotate30Degrees());
            float dDistanceToIntersection30DegreesLeft = IntersectorFinder.FindIntersectionOnBitmap(m_cBackgroundImageFast, m_cPlayerPosition, m_cPlayerRotation.RotateMinus30Degrees());
            gGraphicsOnBitmap.DrawLine(Pens.Red, m_cPlayerPosition, Vector.GetEndPointFromStartPoint(m_cPlayerPosition, m_cPlayerRotation.Multiply(dDistanceToIntersectionAhead)));
            gGraphicsOnBitmap.DrawLine(Pens.Red, m_cPlayerPosition, Vector.GetEndPointFromStartPoint(m_cPlayerPosition, m_cPlayerRotation.Rotate30Degrees().Multiply(dDistanceToIntersection30DegreesRight)));
            gGraphicsOnBitmap.DrawLine(Pens.Red, m_cPlayerPosition, Vector.GetEndPointFromStartPoint(m_cPlayerPosition, m_cPlayerRotation.RotateMinus30Degrees().Multiply(dDistanceToIntersection30DegreesLeft)));
            gGraphicsOnBitmap.DrawString("Distance ahead " + dDistanceToIntersectionAhead, new Font("Times New Roman", 12.0f), Brushes.Red, 0, 0);
            
            g.DrawImageUnscaled(cTempBitmap, 0, 0);
            cTempBitmap.Dispose();
            gGraphicsOnBitmap.Dispose();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
                m_fPressingRight = true;
            if (e.KeyCode == Keys.Left)
                m_fPressingLeft = true;
            if (e.KeyCode == Keys.Up)
                m_fPressingUp = true;
            if (e.KeyCode == Keys.Down)
                m_fPressingDown = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
                m_fPressingRight = false;
            if (e.KeyCode == Keys.Left)
                m_fPressingLeft = false;
            if (e.KeyCode == Keys.Up)
                m_fPressingUp = false;
            if (e.KeyCode == Keys.Down)
                m_fPressingDown = false;
        }
    }
}
