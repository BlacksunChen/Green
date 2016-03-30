using UnityEngine;
using System.Collections;
using Generic.Framework;

public class SteeringParams : Singleton<SteeringParams>
{
    /// <summary>
    /// read param.ini
    /// </summary>
    void Start()
    {

    }

    public int NumAgents;
    public int NumObstacles;
    public float MinObstacleRadius;
    public float MaxObstacleRadius;

    //number of horizontal cells used for spatial partitioning
    public int NumCellsX;
    //number of vertical cells used for spatial partitioning
    public int NumCellsY;

    //how many samples the smoother will use to average a value
    public int NumSamplesForSmoothing;

    //used to tweak the combined steering force (simply altering the MaxSteeringForce
    //will NOT work!This tweaker affects all the steering force multipliers
    //too).
    public float SteeringForceTweaker;

    public float MaxSteeringForce;
    public float MaxSpeed;
    public float VehicleMass;

    public float VehicleScale;
    public float MaxTurnRatePerSecond;

    public float SeparationWeight;
    public float AlignmentWeight;
    public float CohesionWeight;
    public float ObstacleAvoidanceWeight;
    public float WallAvoidanceWeight;
    public float WanderWeight;
    public float SeekWeight;
    public float FleeWeight;
    public float ArriveWeight;
    public float PursuitWeight;
    public float OffsetPursuitWeight;
    public float InterposeWeight;
    public float HideWeight;
    public float EvadeWeight;
    public float FollowPathWeight;

    //how close a neighbour must be before an agent perceives it (considers it
    //to be within its neighborhood)
    public float ViewDistance;

    //used in obstacle avoidance
    public float MinDetectionBoxLength;

    //used in wall avoidance
    public float WallDetectionFeelerLength;

    //these are the probabilities that a steering behavior will be used
    //when the prioritized dither calculate method is used
    public float prWallAvoidance;
    public float prObstacleAvoidance;
    public float prSeparation;
    public float prAlignment;
    public float prCohesion;
    public float prWander;
    public float prSeek;
    public float prFlee;
    public float prEvade;
    public float prHide;
    public float prArrive;

    

}
