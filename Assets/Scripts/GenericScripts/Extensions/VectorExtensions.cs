/// <summary>
/// VectorExtensions v1.0.0 by Christian Chomiak, christianchomiak@gmail.com
/// 
/// Functions that facilitate the use of Vectors
/// </summary>

using UnityEngine;


namespace Generic
{

    public static class VectorExtensions
    {
        #region Vector2

        /// <summary>
        /// returns positive if v2 is clockwise of this vector,
        /// minus if anticlockwise (Y axis pointing down, X axis to right)
        /// </summary>
        enum ClockWise { Clockwise = 1, Anticlockwise = -1 };

        public static int Sign(this Vector2 v, Vector2 v2)
        {
            if (v.y * v2.x > v.x * v2.y)
            {
                return (int)ClockWise.Anticlockwise;
            }
            else
            {
                return (int)ClockWise.Clockwise;
            }
        }

        /// <summary>
        /// rotates a vector ang rads around the origin
        /// </summary>
        /// <param name="v"></param>
        /// <param name="ang"></param>
        public static void RotateAroundOrigin(this Vector2 v, float ang)
        {
            Quaternion rotate = Quaternion.AngleAxis(ang, Vector3.forward);

            var temp = rotate * v;
            v.x = temp.x;
            v.y = temp.y; 
        }

        /// <summary>
        /// Transforms a vector from the agent's local space into world space
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="AgentHeading"></param>
        /// <param name="AgentSide"></param>
        /// <returns></returns>
        public static Vector2 ToWorldSpace(this Vector2 vec,
                                    Vector2 AgentHeading)
        {
            //make a copy of the point
            Vector2 TransVec = vec;     

            //create a transformation matrix
            //Quaternion rotate = Quaternion.LookRotation()
            Quaternion rotate = Quaternion.LookRotation(AgentHeading, Vector3.forward);


            return rotate * vec;
        }

        /// <summary>
        /// Transforms a point from the agent's local space into world space
        /// </summary>
        public static Vector2 ToWorldSpace(this Vector2 point,
                                           Vector2 AgentHeading,
                                           Vector2 AgentPosition)
        {
            //make a copy of the point
            Vector2 TransPoint = point;

            Quaternion rotate = Quaternion.LookRotation(AgentHeading, Vector3.forward);

            Matrix4x4 transMatrix = Matrix4x4.TRS(AgentPosition, rotate, Vector3.one);


            return transMatrix * point;
        }

        public static Vector2 PointToLocalSpace(this Vector2 point,
                                                Vector2 AgentHeading,
                                                Vector2 AgentSide,
                                                Vector2 AgentPosition)
        {
            //make a copy of the point
            Vector2 TransPoint = point;

            //C2DMatrix matTransform;
            //Matrix4x4 matTransform = Matrix4x4.
            
            Quaternion rotate = Quaternion.LookRotation(AgentHeading, Vector3.Cross(AgentHeading, AgentSide));
            Matrix4x4 transMatrix = Matrix4x4.TRS(AgentPosition, rotate, Vector3.one);

            return transMatrix * point;
        }

        public static float SqrDistance(this Vector2 v1, Vector2 v2)
        {

            float ySeparation = v2.y - v1.y;
            float xSeparation = v2.x - v1.x;

            return ySeparation * ySeparation + xSeparation * xSeparation;
        }

        /// <summary>
        /// Return a perpendicular vector (90 degrees rotation) 逆时针
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 Perpendicular(this Vector2 v)
        {
            return new Vector2(-v.y, v.x);
        }

        public static bool IsZero(this Vector2 v)
        {
            return v.x * v.x + v.y * v.y < float.Epsilon;
        }

        public static void Zero(this Vector2 v)
        {
            v.x = 0f;
            v.y = 0f;
        }

        /// <summary>
        /// truncates a vector so that its length does not exceed max
        /// </summary>
        /// <param name="v"></param>
        /// <param name="max"></param>
        public static void Truncate(this Vector2 v, float max)
        {
            if (v.magnitude > max)
            {
                v.Normalize();

                v *= max;
            }
        }

        /// <summary>
        /// Return a perpendicular vector (-90 degrees rotation)
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 PerpendicularRight(this Vector2 v)
        {
            return new Vector2(v.y, -v.x);
        }


        /// <summary>
        /// Returns a copy of a vector with a new X field
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="column">X field of the new vector</param>
        /// <returns></returns>
        public static Vector2 WithX(this Vector2 v, float x)
        {
            return new Vector2(x, v.y);
        }

        /// <summary>
        /// Returns a copy of a vector with a new Y field
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="row">Y field of the new vector</param>
        public static Vector2 WithY(this Vector2 v, float y)
        {
            return new Vector2(v.x, y);
        }

        /// <summary>
        /// Returns a copy of a vector with the X field changed by delta
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="delta">Difference in the X field</param>
        /// <returns></returns>
        public static Vector2 AddX(this Vector2 v, float delta)
        {
            return new Vector2(v.x + delta, v.y);
        }

        /// <summary>
        /// Returns a copy of a vector with the Y field changed by delta
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="delta">Difference in the Y field</param>
        /// <returns></returns>
        public static Vector2 AddY(this Vector2 v, float delta)
        {
            return new Vector2(v.x, v.y + delta);
        }

        /// <summary>
        /// Returns a copy of a vector with the X field multiplied by delta
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="delta">New factor for the X field</param>
        /// <returns></returns>
        public static Vector2 MultiplyX(this Vector2 v, float delta)
        {
            return new Vector2(v.x * delta, v.y);
        }

        /// <summary>
        /// Returns a copy of a vector with the Y field multiplied by delta
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="delta">New factor forthe Y field</param>
        /// <returns></returns>
        public static Vector2 MultiplyY(this Vector2 v, float delta)
        {
            return new Vector3(v.x, v.y * delta);
        }

        /// <summary>
        /// Returns a Vector3 from a Vector2 by adding a Z field
        /// </summary>
        /// <param name="v"></param>
        /// <param name="z">Optional 'z' parameter</param>
        /// <returns></returns>
        public static Vector3 ToVector3(this Vector2 v, float z = 0f)
        {
            return new Vector3(v.x, v.y, z);
        }

        /// <summary>
        /// Returns a Vector4 from a Vector2 by adding a Z & W field
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector4 ToVector4(this Vector2 v)
        {
            return new Vector4(v.x, v.y, 0f, 0f);
        }

        /// <summary>
        /// Returns a Vector4 from a Vector2 by adding a Z & W field
        /// </summary>
        /// <param name="v"></param>
        /// <param name="z">Optional 'z' parameter</param>
        /// <param name="w">Optional 'w' parameter</param>
        /// <returns></returns>
        public static Vector4 ToVector4(this Vector2 v, float z, float w = 0f)
        {
            return new Vector4(v.x, v.y, z, w);
        }

        #endregion


        #region Vector3


        /// <summary>
        /// Returns a copy of a vector with a new X field
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="column">X value of the new vector</param>
        public static Vector3 WithX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }

        /// <summary>
        /// Returns a copy of a vector with a new Y field
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="row">Y value of the new vector</param>
        public static Vector3 WithY(this Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }

        /// <summary>
        /// Returns a copy of a vector with a new Z field
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="z">Z value of the new vector</param>
        public static Vector3 WithZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }

        /// <summary>
        /// Returns a copy of a vector with the X field changed by delta
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="delta">Difference in the X field</param>
        /// <returns></returns>
        public static Vector3 AddX(this Vector3 v, float delta)
        {
            return new Vector3(v.x + delta, v.y, v.z);
        }

        /// <summary>
        /// Returns a copy of a vector with the Y field changed by delta
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="delta">Difference in the Y field</param>
        /// <returns></returns>
        public static Vector3 AddY(this Vector3 v, float delta)
        {
            return new Vector3(v.x, v.y + delta, v.z);
        }

        /// <summary>
        /// Returns a copy of a vector with the Z field changed by delta
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="delta">Difference in the Z field</param>
        /// <returns></returns>
        public static Vector3 AddZ(this Vector3 v, float delta)
        {
            return new Vector3(v.x, v.y, v.z + delta);
        }


        /// <summary>
        /// Returns a copy of a vector with the X field multiplied by delta
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="delta">New factor for the X field</param>
        /// <returns></returns>
        public static Vector3 MultiplyX(this Vector3 v, float delta)
        {
            return new Vector3(v.x * delta, v.y, v.z);
        }

        /// <summary>
        /// Returns a copy of a vector with the Y field multiplied by delta
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="delta">New factor for the Y field</param>
        /// <returns></returns>
        public static Vector3 MultiplyY(this Vector3 v, float delta)
        {
            return new Vector3(v.x, v.y * delta, v.z);
        }

        /// <summary>
        /// Returns a copy of a vector with the Z field multiplied by delta
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="delta">New factor for Z field</param>
        /// <returns></returns>
        public static Vector3 MultiplyZ(this Vector3 v, float delta)
        {
            return new Vector3(v.x, v.y, v.z * delta);
        }

        /// <summary>
        /// Returns a Vector2 from a Vector3 by removing the Z field
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 ToVector2(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }

        /// <summary>
        /// Returns a Vector4 from a Vector3 by adding a W field
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w">Optional 'w' parameter</param>
        /// <returns></returns>
        public static Vector4 ToVector4(this Vector3 v, float w = 0f)
        {
            return new Vector4(v.x, v.y, v.z, w);
        }

        #endregion


        #region Vector4

        /// <summary>
        /// Returns a copy of a vector with a new X field
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="column">X field of the new vector</param>
        public static Vector4 WithX(this Vector4 v, float x)
        {
            return new Vector4(x, v.y, v.z);
        }

        /// <summary>
        /// Returns a copy of a vector with a new Y field
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="row">Y field of the new vector</param>
        public static Vector4 WithY(this Vector4 v, float y)
        {
            return new Vector4(v.x, y, v.z);
        }

        /// <summary>
        /// Returns a copy of a vector with a new Z field
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="z">Z field of the new vector</param>
        public static Vector4 WithZ(this Vector4 v, float z)
        {
            return new Vector4(v.x, v.y, z);
        }

        /// <summary>
        /// Returns a copy of a vector with a new W field
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="w">W field of the new vector</param>
        public static Vector4 WithW(this Vector4 v, float w)
        {
            return new Vector4(v.x, v.y, v.z, w);
        }

        /// <summary>
        /// Returns a copy of a vector with the X field changed by delta
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="delta">Difference in the X field</param>
        /// <returns></returns>
        public static Vector4 AddX(this Vector4 v, float delta)
        {
            return new Vector4(v.x + delta, v.y, v.z, v.w);
        }

        /// <summary>
        /// Returns a copy of a vector with the Y field changed by delta
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="delta">Difference in the Y field</param>
        /// <returns></returns>
        public static Vector4 AddY(this Vector4 v, float delta)
        {
            return new Vector4(v.x, v.y + delta, v.z, v.w);
        }

        /// <summary>
        /// Returns a copy of a vector with the Z field changed by delta
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="delta">Difference in the Z field</param>
        /// <returns></returns>
        public static Vector4 AddZ(this Vector4 v, float delta)
        {
            return new Vector4(v.x, v.y, v.z + delta, v.w);
        }

        /// <summary>
        /// Returns a copy of a vector with the W field changed by delta
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="delta">Difference in the W field</param>
        /// <returns></returns>
        public static Vector4 AddW(this Vector4 v, float delta)
        {
            return new Vector4(v.x, v.y, v.z, v.w + delta);
        }

        /// <summary>
        /// Returns a copy of a vector with the X field multiplied by delta
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="delta">New factor for the X field</param>
        /// <returns></returns>
        public static Vector4 MultiplyX(this Vector4 v, float delta)
        {
            return new Vector4(v.x * delta, v.y, v.z, v.w);
        }

        /// <summary>
        /// Returns a copy of a vector with the Y field multiplied by delta
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="delta">New factor for the Y field</param>
        /// <returns></returns>
        public static Vector4 MultiplyY(this Vector4 v, float delta)
        {
            return new Vector4(v.x, v.y * delta, v.z, v.w);
        }

        /// <summary>
        /// Returns a copy of a vector with the Z field multiplied by delta
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="delta">New factor for the Z field</param>
        /// <returns></returns>
        public static Vector4 MultiplyZ(this Vector4 v, float delta)
        {
            return new Vector4(v.x, v.y, v.z * delta, v.w);
        }

        /// <summary>
        /// Returns a copy of a vector with the W field multiplied by delta
        /// </summary>
        /// <param name="v">Original vector</param>
        /// <param name="delta">New factor for the W field</param>
        /// <returns></returns>
        public static Vector4 MultiplyW(this Vector4 v, float delta)
        {
            return new Vector4(v.x, v.y, v.z, v.w * delta);
        }

        /// <summary>
        /// Returns a Vector3 from a Vector2 by removing the W field
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 ToVector3(this Vector4 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        /// <summary>
        /// Returns a Vector3 from a Vector2 by removing the W field
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 ToVector2(this Vector4 v)
        {
            return new Vector2(v.x, v.y);
        }

        #endregion
    }


}