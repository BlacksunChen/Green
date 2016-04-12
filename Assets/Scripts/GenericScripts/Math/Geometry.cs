using System.Collections.Generic;
using UnityEngine;


namespace Utilities
{
    public class Geometry
    {
        public static Vector2 GetRamdomPointOnRing(float outRad, float inRad, Vector2 center)
        {
            float deg = UnityEngine.Random.Range(0f, 360f);
            float l = UnityEngine.Random.Range(inRad, outRad);
            float x = l * Mathf.Cos(Mathf.Deg2Rad * deg) + center.x;
            float y = l * Mathf.Sin(Mathf.Deg2Rad * deg) + center.y;
            return new Vector2(x, y);
        }
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

        public static bool PointInCircle(Vector2 point,
                                         Vector2 center,
                                         float radius)
        {
            return (point.x - center.x) * (point.x - center.x)
                 + (point.y - center.y) * (point.y - center.y)
                 <= radius * radius;
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
                                          ref float dist,
                                          ref Vector2 point)
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

        /// <summary>
        /// returns true if the line segemnt AB intersects with a circle at
        /// position P with radius radius
        /// </summary>
        public static bool LineSegmentCircleIntersection(Vector2 A,
                                                         Vector2 B,
                                                         Vector2 P,
                                                         float radius)
        {
            //first determine the distance from the center of the circle to
            //the line segment (working in distance squared space)
            double DistToLineSq = DistToLineSegmentSq(A, B, P);

            if (DistToLineSq < radius * radius)
            {
                return true;
            }

            else
            {
                return false;
            }

        }
        public static bool RectIntersection2D(Rect A,
                                              Rect B,
                                          out Vector2 IntersectPoint1,
                                          out Vector2 IntersectPoint2)
        {
            List<Vector2> tmpList = new List<Vector2>();
            Vector2 tmp = new Vector2();
            
            Vector2 leftbuttom = A.min;
            Vector2 leftTop = new Vector2(A.xMin, A.yMax);
            Vector2 rightButtom = new Vector2(A.xMax, A.yMin);
            Vector2 rightTop = A.max;

            int count = 0;
            if(LineRectIntersection2D(leftbuttom, leftTop, B, ref tmp)) tmpList.Add(new Vector2(tmp.x, tmp.y));
            if(LineRectIntersection2D(leftbuttom, leftTop, B, ref tmp)) tmpList.Add(new Vector2(tmp.x, tmp.y));
            if(LineRectIntersection2D(leftbuttom, leftTop, B, ref tmp)) tmpList.Add(new Vector2(tmp.x, tmp.y));
            if(LineRectIntersection2D(leftbuttom, leftTop, B, ref tmp)) tmpList.Add(new Vector2(tmp.x, tmp.y));
            if(count == 2)
            {
                IntersectPoint1 = tmpList[0];
                IntersectPoint2 = tmpList[1];
                return true;
            }
            IntersectPoint1 = new Vector2();
            IntersectPoint2 = new Vector2();
            return false;

        }
        public static bool LineRectIntersection2D(Vector2 A, Vector2 B,
                                                  Rect rect,
                                              ref Vector2 point)
        {
            Vector2 leftbuttom = rect.min;
            Vector2 leftTop = new Vector2(rect.xMin, rect.yMax);
            Vector2 rightButtom = new Vector2(rect.xMax, rect.yMin);
            Vector2 rightTop = rect.max;
            float dist = 0f;
            
            if(LineIntersection2D(A, B, leftbuttom, leftTop, ref dist, ref point))
            {
                return true;
            }
            else if(LineIntersection2D(A, B, leftbuttom, rightButtom, ref dist, ref point))
            {
                return true;
            }
            else if(LineIntersection2D(A, B, rightButtom, rightTop, ref dist, ref point))
            {
                return true;
            }
            else if(LineIntersection2D(A, B, leftTop, rightTop, ref dist, ref point))
            {
                return true;
            }
            return false;
        }
        public static float DistToLineSegmentSq(Vector2 A,
                                                Vector2 B,
                                                Vector2 P)
        {
            //if the angle is obtuse between PA and AB is obtuse then the closest
            //vertex must be A
            float dotA = (P.x - A.x) * (B.x - A.x) + (P.y - A.y) * (B.y - A.y);

            if (dotA <= 0) return A.SqrDistance(P);

            //if the angle is obtuse between PB and AB is obtuse then the closest
            //vertex must be B
            float dotB = (P.x - B.x) * (A.x - B.x) + (P.y - B.y) * (A.y - B.y);

            if (dotB <= 0) return B.SqrDistance(P);

            //calculate the point along AB that is the closest to P
            Vector2 Point = A + ((B - A) * dotA) / (dotA + dotB);

            //calculate the distance P-Point
            return P.SqrDistance(Point);
        }



    /// <summary>
    /// given a line segment AB and a circle position and radius, this function
    /// determines if there is an intersection and stores the position of the 
    /// closest intersection in the reference IntersectionPoint
    /// </summary>
    /// <returns>
    /// returns false if no intersection point is found
    /// </returns>
    public static bool GetLineSegmentCircleClosestIntersectionPoint(Vector2 A,
                                                                    Vector2 B,
                                                                    Vector2 center,
                                                                    float radius,
                                                                ref Vector2 IntersectionPoint)
        {
            Vector2 toBNorm = (B - A).normalized;

            //move the circle into the local space defined by the vector B-A with origin
            //at A
            Vector2 LocalPos = center.PointToLocalSpace(toBNorm, toBNorm.Perpendicular(), A);
            //Vector2 LocalPos = center;
            bool ipFound = false;

            //if the local position + the radius is negative then the circle lays behind
            //point A so there is no intersection possible. If the local x pos minus the 
            //radius is greater than length A-B then the circle cannot intersect the 
            //line segment
            if ((LocalPos.x + radius >= 0) &&
               ((LocalPos.x - radius) * (LocalPos.x - radius) <= B.SqrDistance(A)))
            {
                //if the distance from the x axis to the object's position is less
                //than its radius then there is a potential intersection.
                if (Mathf.Abs(LocalPos.y) < radius)
                {
                    //now to do a line/circle intersection test. The center of the 
                    //circle is represented by A, B. The intersection points are 
                    //given by the formulae x = A +/-sqrt(r^2-B^2), y=0. We only 
                    //need to look at the smallest positive value of x.
                    float a = LocalPos.x;
                    float b = LocalPos.y;

                    float ip = a - Mathf.Sqrt(radius * radius - b * b);

                    if (ip <= 0)
                    {
                        ip = a + Mathf.Sqrt(radius * radius - b * b);
                    }

                    ipFound = true;

                    IntersectionPoint = A + toBNorm * ip;
                }
            }

            return ipFound;
        }
    }
}
