using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Generic.Util
{
    public class Geometry
    {
        /// <summary>
        /// Returns true if the two circles overlap
        /// </summary>
        public static bool TwoCirclesOverlapped(Vector2 c1, float r1,
                                  Vector2 c2, float r2)
        {
            float DistBetweenCenters = Mathf.Sqrt((c1.x - c2.x) * (c1.x - c2.x) +
                                              (c1.y - c2.y) * (c1.y - c2.y));

            if ((DistBetweenCenters < (r1 + r2)) || (DistBetweenCenters < Mathf.Abs(r1 - r2)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// given a line segment AB and a point P, this function calculates the 
        ///  perpendicular distance between them
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        public static float DistToLineSegment(Vector2 A,
                                              Vector2 B,
                                              Vector2 P)
        {
            //if the angle is obtuse between PA and AB is obtuse then the closest
            //vertex must be A
            float dotA = (P.x - A.x) * (B.x - A.x) + (P.y - A.y) * (B.y - A.y);

            if (dotA <= 0) return Vector2.Distance(A, P);

            //if the angle is obtuse between PB and AB is obtuse then the closest
            //vertex must be B
            float dotB = (P.x - B.x) * (A.x - B.x) + (P.y - B.y) * (A.y - B.y);

            if (dotB <= 0) return Vector2.Distance(B, P);

            //calculate the point along AB that is the closest to P
            Vector2 Point = A + ((B - A) * dotA) / (dotA + dotB);

            //calculate the distance P-Point
            return Vector2.Distance(P, Point);
        }

        /// <summary>
        /// Given 2 lines in 2D space AB, CD this returns true if an 
        ///	intersection occurs
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <param name="dist">
        /// sets dist to the distance the intersection
        /// occurs along AB
        /// </param>
        /// <param name="point">
        /// sets the 2d vector point to the point of intersection
        /// </param>
        /// <returns></returns>
        public static bool LineIntersection2D(Vector2 A,
                                              Vector2 B,
                                              Vector2 C,
                                              Vector2 D,
                                          ref float     dist,
                                          ref Vector2  point)
        {

            float rTop = (A.y - C.y) * (D.x - C.x) - (A.x - C.x) * (D.y - C.y);
            float rBot = (B.x - A.x) * (D.y - C.y) - (B.y - A.y) * (D.x - C.x);

            float sTop = (A.y - C.y) * (B.x - A.x) - (A.x - C.x) * (B.y - A.y);
            float sBot = (B.x - A.x) * (D.y - C.y) - (B.y - A.y) * (D.x - C.x);

            if ((rBot == 0) || (sBot == 0))
            {
                //lines are parallel
                return false;
            }

            float r = rTop / rBot;
            float s = sTop / sBot;

            if ((r > 0) && (r < 1) && (s > 0) && (s < 1))
            {
                dist = Vector2.Distance(A, B) * r;

                point = A + r * (B - A);

                return true;
            }

            else
            {
                dist = 0;

                return false;
            }
        }

    }
}
