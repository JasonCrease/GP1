using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace ChaserAndRunner
{
    public class Runner
    {
        PointF m_cPlayerPosition = new PointF(300, 100);
        Vector m_cPlayerRotation = new Vector(1, 1);
        Vector m_cDirectionVector = new Vector(0, 0);
        const float SPEED_ADDITION = 0.8f;

        bool m_fQueuedRotateRight = false;
        bool m_fQueuedRotateLeft = false;
        bool m_fQueuedAccelerate = false;
        bool m_fWasBlockedThisTurn = false;
        float m_dDistanceAhead = 100000;
        float m_dDistance30DegreesLeft = 100000;
        float m_dDistance30DegreesRight = 100000;

        static Stopwatch s_stopwatch = new Stopwatch();
        static int s_timesInvoked = 0;

        public Runner(PointF cInitialPosition)
        {
            m_cPlayerPosition = cInitialPosition;
        }

        public void QueueRotateRight()
        {
            m_fQueuedRotateRight = true;
        }

        public void QueueRotateLeft()
        {
            m_fQueuedRotateLeft = true;
        }

        public void QueueAccelerate()
        {
            m_fQueuedAccelerate = true;
        }

        public float GetDistanceAhead()
        {
            return m_dDistanceAhead;
        }

        public float GetDistance30DegreesLeft()
        {
            return m_dDistance30DegreesLeft;
        }

        public float GetDistance30DegreesRight()
        {
            return m_dDistance30DegreesRight;
        }

        public float GetSpeed()
        {
            return m_cDirectionVector.Distance();
        }

        private void RotateRight()
        {
            m_cPlayerRotation = m_cPlayerRotation.Rotate5Degrees().Normalize();
        }

        private void RotateLeft()
        {
            m_cPlayerRotation = m_cPlayerRotation.RotateMinus5Degrees().Normalize();
        }

        private void Accelerate()
        {
            m_cDirectionVector.Add(m_cPlayerRotation.Multiply(SPEED_ADDITION));
        }

        private void CalculateDistances(World cWorld)
        {
            m_dDistanceAhead = IntersectorFinder.FindIntersectionOnBitmap(cWorld.m_cBackgroundImageFast, m_cPlayerPosition, m_cPlayerRotation);
            m_dDistance30DegreesRight = IntersectorFinder.FindIntersectionOnBitmap(cWorld.m_cBackgroundImageFast, m_cPlayerPosition, m_cPlayerRotation.Rotate30Degrees());
            m_dDistance30DegreesLeft = IntersectorFinder.FindIntersectionOnBitmap(cWorld.m_cBackgroundImageFast, m_cPlayerPosition, m_cPlayerRotation.RotateMinus30Degrees());
        }

        public PointF GetPosition()
        {
            return m_cPlayerPosition;
        }

        public Vector GetRotation()
        {
            return m_cPlayerRotation;
        }

        public bool WasBlockedThisTurn()
        {
            return m_fWasBlockedThisTurn;
        }

        public void RunTimestep(World cWorld)
        {
            m_fWasBlockedThisTurn = false;

            if (m_fQueuedRotateRight == true)
            {
                RotateRight();
            }

            if (m_fQueuedRotateLeft == true)
            {
                RotateLeft();
            }

            PointF pntLastPosition = m_cPlayerPosition;

            if (m_fQueuedAccelerate == true)
            {
                Accelerate();
            }

            // Decelerate by default.
            Vector cDirectionVectorOpposite = new Vector(-m_cDirectionVector.nX, -m_cDirectionVector.nY);
            m_cDirectionVector.Add(cDirectionVectorOpposite.Multiply(SPEED_ADDITION / 8));
            
            // Move the player in it's direction.
            m_cPlayerPosition = Vector.GetEndPointFromStartPoint(m_cPlayerPosition, m_cDirectionVector);

            bool fNextPositionIsBlocked = false;

            // Make sure that the player doesn't exit walls.
            if (m_cPlayerPosition.X < 0)
                fNextPositionIsBlocked = true;
            if (m_cPlayerPosition.X >= cWorld.GetWorldWidth())
                fNextPositionIsBlocked = true;
            if (m_cPlayerPosition.Y < 0)
                fNextPositionIsBlocked = true;
            if (m_cPlayerPosition.Y >= cWorld.GetWorldHeight())
                fNextPositionIsBlocked = true;

            // Check if the player is travelling on blocked surface.
            if (cWorld.CheckIfPositionIsBlocked(m_cPlayerPosition) == true)
            {
                fNextPositionIsBlocked = true;
            }

            // If the current position of the player is blocked, reverse the last position and reset the player speed.
            if (fNextPositionIsBlocked == true)
            {
                m_cPlayerPosition = pntLastPosition;
                m_cDirectionVector = new Vector(0, 0);
                m_fWasBlockedThisTurn = true;
            }


            s_stopwatch.Start();
            CalculateDistances(cWorld);
            s_timesInvoked++;
            s_stopwatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = s_stopwatch.Elapsed;

            m_fQueuedRotateRight = false;
            m_fQueuedRotateLeft = false;
            m_fQueuedAccelerate = false;
        }
    }
}
