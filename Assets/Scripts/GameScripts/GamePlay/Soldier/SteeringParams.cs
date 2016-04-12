using UnityEngine;
using Utilities;

namespace Green
{
    public class SteeringParams
    {
        static SteeringParams instance = null;

        static public SteeringParams Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new SteeringParams();
                }
                return instance;
            }
        }

        /// <summary>
        /// read param.ini
        /// </summary>
        public int NumAgents = 300;
        public int NumObstacles = 7;
        public float MinObstacleRadius = 10;
        public float MaxObstacleRadius = 30;

        //number of horizontal cells used for spatial partitioning
        public int NumCellsX = 7;
        //number of vertical cells used for spatial partitioning
        public int NumCellsY = 7;

        //how many samples the smoother will use to average a value
        public int NumSamplesForSmoothing = 10;

        //used to tweak the combined steering force (simply altering the MaxSteeringForce
        //will NOT work!This tweaker affects all the steering force multipliers
        //too).
        public float SteeringForceTweaker = 200f;

        public float MaxSteeringForce    = 14.97f;
        public float MaxSpeed = 15f;
        public float VehicleMass = 1f;

        public float VehicleScale = 3f;
        public float MaxTurnRatePerSecond;

        public float SeparationWeight = Mathf.PI;
        public float AlignmentWeight = 1f;
        public float CohesionWeight = 2f;
        public float ObstacleAvoidanceWeight = 10f;
        public float WallAvoidanceWeight = 10f;
        public float WanderWeight = 1f;
        public float SeekWeight = 1f;
        public float FleeWeight = 1f;
        public float ArriveWeight = 1f;
        public float PursuitWeight = 1f;
        public float OffsetPursuitWeight = 1f;
        public float InterposeWeight = 1f;
        public float HideWeight = 1f;
        public float EvadeWeight = 0.01f;
        public float FollowPathWeight = 0.05f;

        //how close a neighbour must be before an agent perceives it (considers it
        //to be within its neighborhood)
        public float ViewDistance = 50f;

        //used in obstacle avoidance
        public float MinDetectionBoxLength = 40f;

        //used in wall avoidance
        public float WallDetectionFeelerLength = 2f;

        //these are the probabilities that a steering behavior will be used
        //when the prioritized dither calculate method is used
        public float prWallAvoidance = 0.5f;
        public float prObstacleAvoidance = 0.5f;
        public float prSeparation = .2f;
        public float prAlignment = .3f;
        public float prCohesion = .6f;
        public float prWander = .8f;
        public float prSeek = .8f;
        public float prFlee = .6f;
        public float prEvade = 1f;
        public float prHide = .8f;
        public float prArrive = .5f;
    }
}