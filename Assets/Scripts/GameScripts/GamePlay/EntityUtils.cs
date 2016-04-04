using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Generic;

namespace Green
{
    public class EntityUtils
    {
        //------------------------- Overlapped -----------------------------------
        //
        //  tests to see if an entity is overlapping any of a number of entities
        //  stored in a std container
        //------------------------------------------------------------------------

        bool Overlapped<T, conT>(T ob, conT conOb, float MinDistBetweenObstacles = 40.0f)
                    where T : MovingEntity
                   where conT : IEnumerable<T>
        {

            foreach (var it in conOb)
            {
                if (ob == it) continue;
                if(ob.BoundingCollider.bounds.Intersects(it.BoundingCollider.bounds))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// tags any entities contained in a std container that are within the
        ///  radius of the single entity parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="conT"></typeparam>
        /// <param name="entity"></param>
        /// <param name="containerOfEntities"></param>
        /// <param name="radius"></param>
        public static void TagNeighbors<T, conT>(T entity, conT containerOfEntities, float radius)
            where    T : MovingEntity
            where conT : IEnumerable<MovingEntity>
        {
            //iterate through all entities checking for range
            foreach (var curEntity in containerOfEntities)
            {
                //first clear any current tag
                curEntity.UnTag();

                Vector2 to = curEntity.Position - entity.Position;

                //the bounding radius of the other is taken into account by adding it 
                //to the range
                float range = radius + Mathf.Max(curEntity.BoundingCollider.size.x, curEntity.BoundingCollider.size.y);

                //if entity within range, tag for further consideration. (working in
                //distance-squared space to avoid sqrts)
                if ((curEntity != entity) && (to.SqrMagnitude() < range * range))
                {
                    curEntity.Tag();
                }

            }//next entity
        }



        /// <summary>
        ///  Given a pointer to an entity and a std container of pointers to nearby
        ///  entities, this function checks to see if there is an overlap between
        ///  entities. If there is, then the entities are moved away from each
        ///  other
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="conT"></typeparam>
        /// <param name="entity"></param>
        /// <param name="containerOfEntities"></param>
        public static void EnforceNonPenetrationConstraint<T, conT>(T entity, conT containerOfEntities)
            where T : MovingEntity
            where conT : IEnumerable<T>
        {
            //iterate through all entities checking for any overlap of bounding radii
            foreach (var curEntity in containerOfEntities)
            {
                //make sure we don't check against the individual
                if (curEntity == entity) continue;

                //calculate the distance between the positions of the entities
                Vector2 ToEntity = entity.Position - curEntity.Position;

                float DistFromEachOther = ToEntity.magnitude;

                //if this distance is smaller than the sum of their radii then this
                //entity must be moved away in the direction parallel to the
                //ToEntity vector    
                Vector2 intersectPoint1;
                Vector2 intersectPoint2;
                bool isIntersect = Geometry.RectIntersection2D(new Rect(entity.BoundingCollider.bounds.center, entity.BoundingCollider.size),
                                    new Rect(curEntity.BoundingCollider.bounds.center, curEntity.BoundingSize),
                                    out intersectPoint1, out intersectPoint2);
                if (!isIntersect) continue;
                float AmountOfOverLap = Vector2.Distance(intersectPoint1, intersectPoint2);

                if (AmountOfOverLap >= 0)
                {
                    //move the entity a distance away equivalent to the amount of overlap.
                    entity.Position = entity.Position + (ToEntity / DistFromEachOther) *
                                   AmountOfOverLap;
                }
            }//next entity
        }

        /// <summary>
        ///  tests a line segment AB against a container of entities. First of all
        ///  a test is made to confirm that the entity is within a specified range of 
        ///  the one_to_ignore (positioned at A). If within range the intersection test
        ///  is made.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="conT"></typeparam>
        /// <param name="entities"></param>
        /// <param name="the_one_to_ignore"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="range"></param>
        /// <returns>
        /// a list of all the entities that tested positive for intersection
        /// </returns>
        public static List<T> GetEntityLineSegmentIntersections<T, conT>(conT entities,
                                                       int the_one_to_ignore,
                                                       Vector2 A,
                                                       Vector2 B,
                                                       float range = float.MaxValue)
                    where T : MovingEntity
                    where conT : IEnumerable<T>
        {
            List<T> hits = new List<T>();

            //iterate through all entities checking against the line segment AB
            foreach (var it in entities)
            {
                //if not within range or the entity being checked is the_one_to_ignore
                //just continue with the next entity
                if ((it.ID == the_one_to_ignore) ||
                     (it.Position.SqrDistance(A) > range * range))
                {
                    continue;
                }

                //if the distance to AB is less than the entities bounding radius then
                //there is an intersection so add it to hits
                var p = new Vector2();
                if(Geometry.LineRectIntersection2D(A, B, new Rect(it.BoundingCollider.bounds.center,it.BoundingSize), ref p))
                {
                    hits.Add(it);
                }

            }

            return hits;
        }

        /// <summary>
        /// tests a line segment AB against a container of entities. First of all
        /// a test is made to confirm that the entity is within a specified range of 
        /// the one_to_ignore (positioned at A). If within range the intersection test
        /// is made.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="conT"></typeparam>
        /// <param name="entities"></param>
        /// <param name="the_one_to_ignore"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="range"></param>
        /// <returns>
        /// returns the closest entity that tested positive for intersection or NULL
        /// if none found
        /// </returns>
        public static T GetClosestEntityLineSegmentIntersection<T, conT>(conT entities,
                                                  int the_one_to_ignore,
                                                  Vector2 A,
                                                  Vector2 B,
                                                  float range = float.MaxValue)
                    where    T : MovingEntity
                    where conT : IEnumerable<T>
        {
            T ClosestEntity = null;

            float ClosestDist = float.MaxValue;

            //iterate through all entities checking against the line segment AB
            foreach (var it in entities)
            {
                float distSq = it.Position.SqrDistance(A);

                //if not within range or the entity being checked is the_one_to_ignore
                //just continue with the next entity
                if ((it.ID == the_one_to_ignore) || (distSq > range * range))
                {
                    continue;
                }

                //if the distance to AB is less than the entities bounding radius then
                //there is an intersection so add it to hits
                var p = new Vector2();
                if (Geometry.LineRectIntersection2D(A, B, new Rect(it.BoundingCollider.bounds.center, it.BoundingSize), ref p))
                {
                    if (distSq < ClosestDist)
                    {
                        ClosestDist = distSq;

                        ClosestEntity = it;
                    }
                }

            }

            return ClosestEntity;
        }
    }
}
