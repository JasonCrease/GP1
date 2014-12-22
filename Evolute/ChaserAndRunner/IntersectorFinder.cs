using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ChaserAndRunner
{
    public class IntersectorFinder
    {
        // NOTE THAT cDirection MUST BE NORMALIZED.
        public static float FindIntersectionOnBitmap(FastBitmap cBitmap, PointF pntStartPoint, Vector cDirection)
        {
            float MAXIMAL_SEARCH_AREA = Math.Max(cBitmap.Width, cBitmap.Height);
            int nSearchMultiplier = 0;
            PointF pntCurrSearchPoint = pntStartPoint;
            float dSearchAreaEnd = MAXIMAL_SEARCH_AREA;
            PointF pntSearchAreaStart = pntStartPoint;
            PointF pntSearchAreaEnd = Vector.GetEndPointFromStartPoint(pntStartPoint, cDirection.Multiply(dSearchAreaEnd));
            float dApproximatedDistnace = MAXIMAL_SEARCH_AREA;
            float dCurrentDistanceToBlackPoint;

            while ((pntCurrSearchPoint.X >= 0) &&
                (pntCurrSearchPoint.X < cBitmap.Width) &&
                (pntCurrSearchPoint.Y >= 0) &&
                (pntCurrSearchPoint.Y < cBitmap.Height))
            {
                Color clrColor = cBitmap.GetPixel((int)pntCurrSearchPoint.X, (int)pntCurrSearchPoint.Y);
                if ((clrColor.R == 0) && (clrColor.G == 0) && (clrColor.B == 0))
                {
                    dCurrentDistanceToBlackPoint = new Vector(pntStartPoint, pntCurrSearchPoint).Distance();
                    if (dCurrentDistanceToBlackPoint < dApproximatedDistnace)
                    {
                        dApproximatedDistnace = dCurrentDistanceToBlackPoint;
                    }
                }

                nSearchMultiplier += 10;
                pntCurrSearchPoint = Vector.GetEndPointFromStartPoint(pntSearchAreaStart, cDirection.Multiply(nSearchMultiplier));
            }

            dCurrentDistanceToBlackPoint = new Vector(pntStartPoint, pntCurrSearchPoint).Distance();
            if (dCurrentDistanceToBlackPoint < dApproximatedDistnace)
            {
                dApproximatedDistnace = dCurrentDistanceToBlackPoint;
            }

            return dApproximatedDistnace;
        }

        public static float FindIntersectionOnBitmapOld(FastBitmap cBitmap, PointF pntStartPoint, Vector cDirection)
        {
            float MAXIMAL_SEARCH_AREA = Math.Max(cBitmap.Width, cBitmap.Height);
            Point pntResult;
            int nSearchMultiplier = 1;
            PointF pntCurrSearchPoint = pntStartPoint;
            float dSearchAreaStart = 0;
            float dSearchAreaEnd = MAXIMAL_SEARCH_AREA;
            PointF pntSearchAreaStart = pntStartPoint;
            PointF pntSearchAreaEnd = Vector.GetEndPointFromStartPoint(pntStartPoint, cDirection.Multiply(dSearchAreaEnd));
            float dApproximatedDistnace = MAXIMAL_SEARCH_AREA;
            bool fSearchingBottomBlock = false;

            while ((pntCurrSearchPoint.X >= 0) &&
                (pntCurrSearchPoint.X < cBitmap.Width) &&
                (pntCurrSearchPoint.Y >= 0) &&
                (pntCurrSearchPoint.Y < cBitmap.Height) &&
                (nSearchMultiplier < (dSearchAreaEnd - dSearchAreaStart)))
            {

                Color clrColor = cBitmap.GetPixel((int)pntCurrSearchPoint.X, (int)pntCurrSearchPoint.Y);
                if ((clrColor.R == 0) && (clrColor.G == 0) && (clrColor.B == 0))
                {
                    float dCurrentDistanceToBlackPoint = new Vector(pntStartPoint, pntCurrSearchPoint).Distance();
                    if (dCurrentDistanceToBlackPoint < dApproximatedDistnace)
                    {
                        dApproximatedDistnace = dCurrentDistanceToBlackPoint;
                    }

                    if ((dSearchAreaEnd - dSearchAreaStart) == 1)
                    {
                        return (new Vector(pntStartPoint, pntCurrSearchPoint)).Distance();
                    }
                    else
                    {
                        float dTotalSearchAreaDistnaceHalfed = (dSearchAreaEnd - dSearchAreaStart) / 2;
                        dSearchAreaStart = Math.Max(0, nSearchMultiplier - dTotalSearchAreaDistnaceHalfed);
                        dSearchAreaEnd = dSearchAreaStart + dTotalSearchAreaDistnaceHalfed;
                        pntSearchAreaStart = Vector.GetEndPointFromStartPoint(pntStartPoint, cDirection.Multiply(dSearchAreaStart));
                        nSearchMultiplier = 1;
                        fSearchingBottomBlock = true;
                    }
                }

                nSearchMultiplier *= 2;
                pntCurrSearchPoint = Vector.GetEndPointFromStartPoint(pntSearchAreaStart, cDirection.Multiply(nSearchMultiplier));

                if (!(((pntCurrSearchPoint.X >= 0) &&
(pntCurrSearchPoint.X < cBitmap.Width) &&
(pntCurrSearchPoint.Y >= 0) &&
(pntCurrSearchPoint.Y < cBitmap.Height) &&
(nSearchMultiplier < (dSearchAreaEnd - dSearchAreaStart)))))
                {
                    if (fSearchingBottomBlock == true)
                    {
                        fSearchingBottomBlock = false;
                        float dTotalSearchArea = dSearchAreaEnd - dSearchAreaStart;
                        dSearchAreaStart += dTotalSearchArea;
                        dSearchAreaEnd += dTotalSearchArea;
                        pntSearchAreaStart = Vector.GetEndPointFromStartPoint(pntStartPoint, cDirection.Multiply(dSearchAreaStart));
                        nSearchMultiplier = 1;
                    }
                }
            }

            return dApproximatedDistnace;
        }
    }
}
