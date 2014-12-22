using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ChaserAndRunner
{
    public class Vector
    {
        public float nX;
        public float nY;

        public Vector(float nX, float nY)
        {
            this.nX = nX;
            this.nY = nY;
        }

        public Vector(float nStartX, float nEndX, float nStartY, float nEndY)
        {
            nX = nEndX - nStartX;
            nY = nEndY - nStartY;
        }

        public Vector(PointF cPointStart, PointF cPointEnd)
        {
            nX = cPointEnd.X - cPointStart.X;
            nY = cPointEnd.Y - cPointStart.Y;
        }

        public static PointF GetEndPointFromStartPoint(PointF cStartPoint, Vector cDirectionVector)
        {
            PointF cResult = new PointF(cStartPoint.X + cDirectionVector.nX, cStartPoint.Y + cDirectionVector.nY);
            return cResult;
        }

        public Vector Rotate30Degrees()
        {
            Vector cNewVector = new Vector(0, 0);
            cNewVector.nX = (float)(0.866 * nX - 0.5 * nY);
            cNewVector.nY = (float)(0.5 * nX + 0.866 * nY);
            return cNewVector;
        }

        public Vector RotateMinus30Degrees()
        {
            Vector cNewVector = new Vector(0, 0);
            cNewVector.nX = (float)(0.866 * nX + 0.5 * nY);
            cNewVector.nY = (float)(0.866 * nY - 0.5 * nX);
            return cNewVector;
        }

        public Vector Rotate5Degrees()
        {
            Vector cNewVector = new Vector(0, 0);
            cNewVector.nX = (float)(0.966 * nX - 0.0872 * nY);
            cNewVector.nY = (float)(0.0872 * nX + 0.966 * nY);
            return cNewVector;
        }

        public Vector RotateMinus5Degrees()
        {
            Vector cNewVector = new Vector(0, 0);
            cNewVector.nX = (float)(0.966 * nX + 0.0872 * nY);
            cNewVector.nY = (float)(0.966 * nY - 0.0872 * nX);
            return cNewVector;
        }

        public void Add(Vector cOther)
        {
            nX += cOther.nX;
            nY += cOther.nY;
        }

        public void Substract(Vector cOther)
        {
            nX -= cOther.nX;
            nY -= cOther.nY;
        }

        public Vector Multiply(float dValue)
        {
            Vector cNewVector = new Vector(nX * dValue, nY * dValue);
            return cNewVector;
        }

        public float Distance()
        {
            return (float)(Math.Sqrt((nX * nX) + (nY * nY)));
        }

        public Vector Normalize()
        {
            float dDistance = Distance();
            Vector cNormalized = new Vector(0, 0);
            cNormalized.nX = nX / dDistance;
            cNormalized.nY = nY / dDistance;
            return cNormalized;
        }
    }
}
