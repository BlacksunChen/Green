using UnityEngine;
using System.Collections;

namespace Green
{
    public abstract class Wall2D : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="A">LineAB</param>
        /// <param name="B"></param>
        /// <param name="dist">the distance the intersection occurs along AB</param>
        /// <param name="point">the point of intersection</param>
        public abstract bool IsIntersection(Vector2 A,
                                            Vector2 B,
                                        out float dist,
                                        out Vector2 point);

        /// <summary>
        /// Get Normal of the line of this point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public abstract Vector2 GetTangentNormal(Vector2 point);
    }
} 
