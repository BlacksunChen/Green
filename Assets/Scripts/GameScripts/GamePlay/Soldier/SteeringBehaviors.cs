using System;
using System.Collections.Generic;
using UnityEngine;
using Generic.Utilities;

    namespace Green
{
    class SteeringBehaviors
    {
        public enum summing_method
        {
            weighted_average,
            prioritized,
            dithered
        };

        [Flags]
        private enum behavior_type : int
        {
            none = 1 << 0,
            seek = 1 << 1,
            flee = 1 << 2,
            arrive = 1 << 3,
            wander = 1 << 4,
            cohesion = 1 << 5,
            separation = 1 << 6,
            allignment = 1 << 7,
            obstacle_avoidance = 1 << 8,
            wall_avoidance = 1 << 9,
            follow_path = 1 << 10,
            pursuit = 1 << 11,
            evade = 1 << 12,
            interpose = 1 << 13,
            hide = 1 << 14,
            flock = 1 << 15,
            offset_pursuit = 1 << 16,
        }


        //a pointer to the owner of this instance
        MovingEntity _movingEntity;

        //the steering force created by the combined effect of all
        //the selected behaviors
        Vector2 _steeringForce;

        //these can be used to keep track of friends, pursuers, or prey
        MovingEntity _targetAgent1;
        MovingEntity _targetAgent2;

        //the current target
        Vector2 _target;

        //length of the 'detection box' utilized in obstacle avoidance
        float _boxLength;


        //a vertex buffer to contain the feelers rqd for wall avoidance  
        List<Vector2> _feelers;

        //the length of the 'feeler/s' used in wall detection
        float _wallDetectionFeelerLength;



        //the current position on the wander circle the agent is
        //attempting to steer towards
        Vector2 _wanderTarget;

        //explained above
        float _wanderJitter;
        float _wanderRadius;
        float _wanderDistance;


        //multipliers. These can be adjusted to effect strength of the  
        //appropriate behavior. Useful to get flocking the way you require
        //for example.
        float _weightSeparation;
        float _weightCohesion;
        float _weightAlignment;
        float _weightWander;
        float _weightObstacleAvoidance;
        float _weightWallAvoidance;
        float _weightSeek;
        float _weightFlee;
        float _weightArrive;
        float _weightPursuit;
        float _weightOffsetPursuit;
        float _weightInterpose;
        float _weightHide;
        float _weightEvade;
        float _weightFollowPath;

        //how far the agent can 'see'
        float _viewDistance;

        //pointer to any current path
        //Path* m_pPath;

        //the distance (squared) a MovingEntity has to be from a path waypoint before
        //it starts seeking to the next waypoint
        float _waypointSeekDistSq;


        //any offset used for formations or offset pursuit
        Vector2 _offset;



        //binary flags to indicate whether or not a behavior should be active
        behavior_type _flags;


        //Arrive makes use of these to determine how quickly a MovingEntity
        //should decelerate to its target
        enum Deceleration
        {
            slow = 3,
            normal = 2,
            fast = 1
        };

        //default
        Deceleration _Deceleration;

        //is cell space partitioning to be used or not?
        bool _cellSpaceOn;

        //what type of method is used to sum any active behavior
        summing_method m_SummingMethod;


        //this function tests if a specific bit of m_iFlags is set
        bool On(behavior_type bt) { return (_flags & bt) == bt; }

        bool AccumulateForce(Vector2 runningTot, Vector2 forceToAdd)
        {
            //calculate how much steering force the MovingEntity has used so far
            float MagnitudeSoFar = runningTot.magnitude;

            //calculate how much steering force remains to be used by this MovingEntity
            float MagnitudeRemaining = _movingEntity.MaxForce - MagnitudeSoFar;

            //return false if there is no more force left to use
            if (MagnitudeRemaining <= 0.0) return false;

            //calculate the magnitude of the force we want to add
            float MagnitudeToAdd = forceToAdd.magnitude;

            //if the magnitude of the sum of ForceToAdd and the running total
            //does not exceed the maximum force available to this MovingEntity, just
            //add together. Otherwise add as much of the ForceToAdd vector is
            //possible without going over the max.
            if (MagnitudeToAdd < MagnitudeRemaining)
            {
                runningTot += forceToAdd;
            }

            else
            {
                //add it to the steering force
                runningTot += forceToAdd.normalized * MagnitudeRemaining;
            }

            return true;
        }

        //creates the antenna utilized by the wall avoidance behavior
        void CreateFeelers()
        {
            //feeler pointing straight in front
            _feelers[0] = _movingEntity.Position + _wallDetectionFeelerLength * _movingEntity.Heading;

            //feeler to left
            Vector2 temp = _movingEntity.Heading;
            temp.RotateAroundOrigin(Mathf.PI / 2 * 3.5f);
            _feelers[1] = _movingEntity.Position + _wallDetectionFeelerLength / 2.0f * temp;

            //feeler to right
            temp = _movingEntity.Heading;
            temp.RotateAroundOrigin(Mathf.PI / 2 * 0.5f);
            _feelers[2] = _movingEntity.Position + _wallDetectionFeelerLength / 2.0f * temp;
        }



        /* .......................................................

                         BEGIN BEHAVIOR DECLARATIONS

           .......................................................*/


        /// <summary>
        ///  Given a target, this behavior returns a steering force which will
        /// direct the agent towards the target
        /// </summary>
        /// <param name="TargetPos"></param>
        /// <returns></returns>
        Vector2 Seek(Vector2 TargetPos)
        {
            Vector2 DesiredVelocity = (TargetPos - _movingEntity.Position).normalized
                            * _movingEntity.MaxSpeed;

            return (DesiredVelocity - _movingEntity.Velocity);
        }

        /// <summary>
        /// Does the opposite of Seek
        /// </summary>
        /// <param name="TargetPos"></param>
        /// <returns></returns>
        Vector2 Flee(Vector2 TargetPos)
        {
            Vector2 DesiredVelocity = (_movingEntity.Position - TargetPos).normalized
                           * _movingEntity.MaxSpeed;

            return (DesiredVelocity - _movingEntity.Velocity);
        }

        //this behavior is similar to seek but it attempts to arrive 
        //at the target position with a zero velocity
        Vector2 Arrive(Vector2 TargetPos,
                        Deceleration deceleration)
        {
            Vector2 ToTarget = TargetPos - _movingEntity.Position;

            //calculate the distance to the target
            float dist = ToTarget.magnitude;

            if (dist > 0)
            {
                //because Deceleration is enumerated as an int, this value is required
                //to provide fine tweaking of the deceleration..
                const float DecelerationTweaker = 0.3f;

                //calculate the speed required to reach the target given the desired
                //deceleration
                float speed = dist / ((float)deceleration * DecelerationTweaker);

                //make sure the velocity does not exceed the max
                speed = Mathf.Min(speed, _movingEntity.MaxSpeed);

                //from here proceed just like Seek except we don't need to normalize 
                //the ToTarget vector because we have already gone to the trouble
                //of calculating its length: dist. 
                Vector2 DesiredVelocity = ToTarget * speed / dist;

                return (DesiredVelocity - _movingEntity.Velocity);
            }

            return new Vector2(0, 0);
        }

        /// <summary>
        /// this behavior creates a force that steers the agent towards the 
        ///evader
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        Vector2 Pursuit(MovingEntity evader)
        {
            //if the evader is ahead and facing the agent then we can just seek
            //for the evader's current position.
            Vector2 ToEvader = evader.Position - _movingEntity.Position;

            float RelativeHeading = Vector2.Dot(_movingEntity.Heading, evader.Heading);

            if ((Vector2.Dot(ToEvader, _movingEntity.Heading) > 0) &&
                 (RelativeHeading < -0.95f))  //acos(0.95)=18 degs
            {
                return Seek(evader.Position);
            }

            //Not considered ahead so we predict where the evader will be.

            //the lookahead time is propotional to the distance between the evader
            //and the pursuer; and is inversely proportional to the sum of the
            //agent's velocities
            float LookAheadTime = ToEvader.magnitude /
                                  (_movingEntity.MaxSpeed + evader.Speed);

            //now seek to the predicted future position of the evader
            return Seek(evader.Position + evader.Velocity * LookAheadTime);
        }

        /// <summary>
        /// this behavior maintains a position, in the direction of offset
        /// from the target MovingEntity
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="offset"></param>
        /// <returns></returns>

        Vector2 OffsetPursuit(MovingEntity leader, Vector2 offset)
        {
            //calculate the offset's position in world space
            Vector2 WorldOffsetPos = offset.ToWorldSpace(leader.Heading,
                                                         leader.Position);

            Vector2 ToOffset = WorldOffsetPos - _movingEntity.Position;

            //the lookahead time is propotional to the distance between the leader
            //and the pursuer; and is inversely proportional to the sum of both
            //agent's velocities
            float LookAheadTime = ToOffset.magnitude /
                                  (_movingEntity.MaxSpeed + leader.Speed);

            //now Arrive at the predicted future position of the offset
            return Arrive(WorldOffsetPos + leader.Velocity * LookAheadTime, Deceleration.fast);
        }

        /// <summary>
        /// this behavior attempts to evade a pursuer
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        Vector2 Evade(MovingEntity pursuer)
        {
            /* Not necessary to include the check for facing direction this time */

            Vector2 ToPursuer = pursuer.Position - _movingEntity.Position;

            //uncomment the following two lines to have Evade only consider pursuers 
            //within a 'threat range'
            const float ThreatRange = 100.0f;
            if (ToPursuer.sqrMagnitude > ThreatRange * ThreatRange) return new Vector2();

            //the lookahead time is propotional to the distance between the pursuer
            //and the pursuer; and is inversely proportional to the sum of the
            //agents' velocities
            float LookAheadTime = ToPursuer.magnitude /
                                   (_movingEntity.MaxSpeed + pursuer.Speed);

            //now flee away from predicted future position of the pursuer
            return Flee(pursuer.Position + pursuer.Velocity * LookAheadTime);
        }

        /// <summary>
        /// this behavior makes the agent wander about randomly
        /// </summary>
        /// <returns></returns>
        Vector2 Wander()
        {
            //this behavior is dependent on the update rate, so this line must
            //be included when using time independent framerate.
            float JitterThisTimeSlice = _wanderJitter * Time.deltaTime;

            //first, add a small random vector to the target's position
            _wanderTarget += new Vector2(UnityEngine.Random.Range(-1, 1) * JitterThisTimeSlice,
                                         UnityEngine.Random.Range(-1, 1) * JitterThisTimeSlice);

            //reproject this new vector back on to a unit circle
            _wanderTarget.Normalize();

            //increase the length of the vector to the same as the radius
            //of the wander circle
            _wanderTarget *= _wanderRadius;

            //move the target into a position WanderDist in front of the agent
            Vector2 target = _wanderTarget + new Vector2(_wanderDistance, 0);

            //project the target into world space
            Vector2 Target = target.ToWorldSpace(_movingEntity.Heading,
                                                 _movingEntity.Position);

            //and steer towards it
            return Target - _movingEntity.Position;
        }

        /// <summary>
        /// this returns a steering force which will attempt to keep the agent 
        /// away from any obstacles it may encounter
        /// </summary>
        /// <param name="obstacles"></param>
        /// <returns></returns>
        public float 
        Vector2 ObstacleAvoidance(List<MovingEntity> obstacles)
        {
            //the detection box length is proportional to the agent's velocity
            _dBoxLength = Prm.MinDetectionBoxLength +
                            (_movingEntity.Speed / _movingEntity.MaxSpeed) *
                            Prm.MinDetectionBoxLength;

            //tag all obstacles within range of the box for processing
            _movingEntity.World().TagObstaclesWithinViewRange(_movingEntity, _dBoxLength);

            //this will keep track of the closest intersecting obstacle (CIB)
            BaseGameEntity* ClosestIntersectingObstacle = NULL;

            //this will be used to track the distance to the CIB
            double DistToClosestIP = MaxDouble;

            //this will record the transformed local coordinates of the CIB
            Vector2D LocalPosOfClosestObstacle;

            std::vector<BaseGameEntity*>::const_iterator curOb = obstacles.begin();

            while (curOb != obstacles.end())
            {
                //if the obstacle has been tagged within range proceed
                if ((*curOb).IsTagged())
                {
                    //calculate this obstacle's position in local space
                    Vector2D LocalPos = PointToLocalSpace((*curOb).Pos(),
                                                           _movingEntity.Heading(),
                                                           _movingEntity.Side(),
                                                           _movingEntity.Pos());

                    //if the local position has a negative x value then it must lay
                    //behind the agent. (in which case it can be ignored)
                    if (LocalPos.x >= 0)
                    {
                        //if the distance from the x axis to the object's position is less
                        //than its radius + half the width of the detection box then there
                        //is a potential intersection.
                        double ExpandedRadius = (*curOb).BRadius() + _movingEntity.BRadius();

                        if (fabs(LocalPos.y) < ExpandedRadius)
                        {
                            //now to do a line/circle intersection test. The center of the 
                            //circle is represented by (cX, cY). The intersection points are 
                            //given by the formula x = cX +/-sqrt(r^2-cY^2) for y=0. 
                            //We only need to look at the smallest positive value of x because
                            //that will be the closest point of intersection.
                            double cX = LocalPos.x;
                            double cY = LocalPos.y;

                            //we only need to calculate the sqrt part of the above equation once
                            double SqrtPart = sqrt(ExpandedRadius * ExpandedRadius - cY * cY);

                            double ip = cX - SqrtPart;

                            if (ip <= 0.0)
                            {
                                ip = cX + SqrtPart;
                            }

                            //test to see if this is the closest so far. If it is keep a
                            //record of the obstacle and its local coordinates
                            if (ip < DistToClosestIP)
                            {
                                DistToClosestIP = ip;

                                ClosestIntersectingObstacle = *curOb;

                                LocalPosOfClosestObstacle = LocalPos;
                            }
                        }
                    }
                }

                ++curOb;
            }

            //if we have found an intersecting obstacle, calculate a steering 
            //force away from it
            Vector2D SteeringForce;

            if (ClosestIntersectingObstacle)
            {
                //the closer the agent is to an object, the stronger the 
                //steering force should be
                double multiplier = 1.0 + (_dBoxLength - LocalPosOfClosestObstacle.x) /
                                    _dBoxLength;

                //calculate the lateral force
                SteeringForce.y = (ClosestIntersectingObstacle.BRadius() -
                                   LocalPosOfClosestObstacle.y) * multiplier;

                //apply a braking force proportional to the obstacles distance from
                //the vehicle. 
                const double BrakingWeight = 0.2;

                SteeringForce.x = (ClosestIntersectingObstacle.BRadius() -
                                   LocalPosOfClosestObstacle.x) *
                                   BrakingWeight;
            }

            //finally, convert the steering vector from local to world space
            return VectorToWorldSpace(SteeringForce,
                                      _movingEntity.Heading(),
                                      _movingEntity.Side());
        }

  //this returns a steering force which will keep the agent away from any
  //walls it may encounter
  Vector2 WallAvoidance(const std::vector<Wall2D> &walls);

  
  //given a series of Vector2Ds, this method produces a force that will
  //move the agent along the waypoints in order
  Vector2 FollowPath();

        //this results in a steering force that attempts to steer the MovingEntity
        //to the center of the vector connecting two moving agents.
        Vector2 Interpose(const MovingEntity* VehicleA, const MovingEntity* VehicleB);

        //given another agent position to hide from and a list of BaseGameEntitys this
        //method attempts to put an obstacle between itself and its opponent
        Vector2 Hide(const MovingEntity* hunter, const std::vector<BaseGameEntity*>& obstacles);


  // -- Group Behaviors -- //

  Vector2 Cohesion(const std::vector<MovingEntity*> &agents);
  
  Vector2 Separation(const std::vector<MovingEntity*> &agents);

  Vector2 Alignment(const std::vector<MovingEntity*> &agents);

  //the following three are the same as above but they use cell-space
  //partitioning to find the neighbors
  Vector2 CohesionPlus(const std::vector<MovingEntity*> &agents);
  Vector2 SeparationPlus(const std::vector<MovingEntity*> &agents);
  Vector2 AlignmentPlus(const std::vector<MovingEntity*> &agents);

    /* .......................................................

                       END BEHAVIOR DECLARATIONS

      .......................................................*/

  //calculates and sums the steering forces from any active behaviors
  Vector2 CalculateWeightedSum();
        Vector2 CalculatePrioritized();
        Vector2 CalculateDithered();

        //helper method for Hide. Returns a position located on the other
        //side of an obstacle to the pursuer
        Vector2 GetHidingPosition(const Vector2& posOb,
                              const float radiusOb,
                              const Vector2& posHunter);



  
  
public:

  SteeringBehavior(MovingEntity* agent);

        virtual ~SteeringBehavior();

        //calculates and sums the steering forces from any active behaviors
        Vector2 Calculate();

        //calculates the component of the steering force that is parallel
        //with the MovingEntity heading
        float ForwardComponent();

        //calculates the component of the steering force that is perpendicuar
        //with the MovingEntity heading
        float SideComponent();



        //renders visual aids and info for seeing how each behavior is
        //calculated
        void RenderAids();

        void SetTarget(const Vector2 t){m_vTarget = t;}

    void SetTargetAgent1(MovingEntity* Agent) { m_pTargetAgent1 = Agent; }
    void SetTargetAgent2(MovingEntity* Agent) { m_pTargetAgent2 = Agent; }

    void SetOffset(const Vector2 offset) { m_vOffset = offset; }
    Vector2 GetOffset()const{return m_vOffset;}

void SetPath(std::list<Vector2> new_path) { m_pPath.Set(new_path); }
void CreateRandomPath(int num_waypoints, int mx, int my, int cx, int cy)const
            {m_pPath.CreateRandomPath(num_waypoints, mx, my, cx, cy);}

  Vector2 Force()const{return m_vSteeringForce;}

  void ToggleSpacePartitioningOnOff() { m_bCellSpaceOn = !m_bCellSpaceOn; }
bool isSpacePartitioningOn()const{return m_bCellSpaceOn;}

  void SetSummingMethod(summing_method sm) { m_SummingMethod = sm; }


void FleeOn() { m_iFlags |= flee; }
void SeekOn() { m_iFlags |= seek; }
void ArriveOn() { m_iFlags |= arrive; }
void WanderOn() { m_iFlags |= wander; }
void PursuitOn(MovingEntity* v) { m_iFlags |= pursuit; m_pTargetAgent1 = v; }
void EvadeOn(MovingEntity* v) { m_iFlags |= evade; m_pTargetAgent1 = v; }
void CohesionOn() { m_iFlags |= cohesion; }
void SeparationOn() { m_iFlags |= separation; }
void AlignmentOn() { m_iFlags |= allignment; }
void ObstacleAvoidanceOn() { m_iFlags |= obstacle_avoidance; }
void WallAvoidanceOn() { m_iFlags |= wall_avoidance; }
void FollowPathOn() { m_iFlags |= follow_path; }
void InterposeOn(MovingEntity* v1, MovingEntity* v2) { m_iFlags |= interpose; m_pTargetAgent1 = v1; m_pTargetAgent2 = v2; }
void HideOn(MovingEntity* v) { m_iFlags |= hide; m_pTargetAgent1 = v; }
void OffsetPursuitOn(MovingEntity* v1, const Vector2 offset) { m_iFlags |= offset_pursuit; m_vOffset = offset; m_pTargetAgent1 = v1; }
void FlockingOn() { CohesionOn(); AlignmentOn(); SeparationOn(); WanderOn(); }

void FleeOff() { if (On(flee)) m_iFlags ^= flee; }
void SeekOff() { if (On(seek)) m_iFlags ^= seek; }
void ArriveOff() { if (On(arrive)) m_iFlags ^= arrive; }
void WanderOff() { if (On(wander)) m_iFlags ^= wander; }
void PursuitOff() { if (On(pursuit)) m_iFlags ^= pursuit; }
void EvadeOff() { if (On(evade)) m_iFlags ^= evade; }
void CohesionOff() { if (On(cohesion)) m_iFlags ^= cohesion; }
void SeparationOff() { if (On(separation)) m_iFlags ^= separation; }
void AlignmentOff() { if (On(allignment)) m_iFlags ^= allignment; }
void ObstacleAvoidanceOff() { if (On(obstacle_avoidance)) m_iFlags ^= obstacle_avoidance; }
void WallAvoidanceOff() { if (On(wall_avoidance)) m_iFlags ^= wall_avoidance; }
void FollowPathOff() { if (On(follow_path)) m_iFlags ^= follow_path; }
void InterposeOff() { if (On(interpose)) m_iFlags ^= interpose; }
void HideOff() { if (On(hide)) m_iFlags ^= hide; }
void OffsetPursuitOff() { if (On(offset_pursuit)) m_iFlags ^= offset_pursuit; }
void FlockingOff() { CohesionOff(); AlignmentOff(); SeparationOff(); WanderOff(); }

bool isFleeOn() { return On(flee); }
bool isSeekOn() { return On(seek); }
bool isArriveOn() { return On(arrive); }
bool isWanderOn() { return On(wander); }
bool isPursuitOn() { return On(pursuit); }
bool isEvadeOn() { return On(evade); }
bool isCohesionOn() { return On(cohesion); }
bool isSeparationOn() { return On(separation); }
bool isAlignmentOn() { return On(allignment); }
bool isObstacleAvoidanceOn() { return On(obstacle_avoidance); }
bool isWallAvoidanceOn() { return On(wall_avoidance); }
bool isFollowPathOn() { return On(follow_path); }
bool isInterposeOn() { return On(interpose); }
bool isHideOn() { return On(hide); }
bool isOffsetPursuitOn() { return On(offset_pursuit); }

float DBoxLength()const{return _dBoxLength;}
  const std::vector<Vector2>& GetFeelers()const{return m_Feelers;}
  
  float WanderJitter()const{return _wanderJitter;}
  float WanderDistance()const{return _wanderDistance;}
  float WanderRadius()const{return _wanderRadius;}

  float SeparationWeight()const{return _weightSeparation;}
  float AlignmentWeight()const{return _weightAlignment;}
  float CohesionWeight()const{return _weightCohesion;}

};
    }
}
