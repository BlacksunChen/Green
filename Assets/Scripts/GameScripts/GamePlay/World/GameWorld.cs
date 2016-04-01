using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Generic.Framework;

namespace Green
{
    public class GameWorld : Singleton<GameWorld>
    {
        List<MovingEntity> _movingEntities;

        List<Base2DEntity> _obstacles;

        List<Wall2D> _walls;

        CellSpacePartition<MovingEntity> _cellSpace;

        //Path _path;

        /// <summary>
        /// set true to pause the motion
        /// </summary>
        bool _paused;

        /// <summary>
        /// local copy of client window dimensions
        /// </summary>
        int _cxClient;
        int _cyClient;

        /// <summary>
        /// the position of the crosshair
        /// </summary>
        Vector2 _crosshair;

        /// <summary>
        /// keeps track of the average FPS
        /// </summary>
        float _avFrameTime;

        /// <summary>
        /// //flags to turn aids and obstacles etc on/off
        /// </summary>
        bool _showWalls;
        bool _showObstacles;
        bool _showPath;
        bool _showDetectionBox;
        bool _showWanderCircle;
        bool _showFeelers;
        bool _showSteeringForce;
        bool _showFPS;
        bool _renderNeighbors;
        bool _viewKeys;
        bool _showCellSpaceInfo;

        void Init(int cx, int cy)
        {
            _cxClient =cx;
            _cyClient =cy;
            _paused =false;
            _crosshair =new Vector2(_cxClient / 2.0f, _cxClient / 2.0f);
            _showWalls =false;
            _showObstacles =false;
            _showPath =false;
            _showWanderCircle =false;
            _showSteeringForce =false;
            _showFeelers =false;
            _showDetectionBox =false;
            _showFPS =true;
            _avFrameTime =0;
            //_path =NULL;
            _renderNeighbors =false;
            _viewKeys =false;
            _showCellSpaceInfo = false;

            //setup the spatial subdivision class
            _cellSpace = new CellSpacePartition<MovingEntity>((float)cx, (float)cy, SteeringParams.Instance.NumCellsX, SteeringParams.Instance.NumCellsY, SteeringParams.Instance.NumAgents);
        }
        public void NonPenetrationContraint(MovingEntity v)
        {
            EntityUtils.EnforceNonPenetrationConstraint(v, _movingEntities);
        }

        public void TagVehiclesWithinViewRange(Base2DEntity entity, float range)
        {
            EntityUtils.TagNeighbors(entity, _movingEntities, range);
        }

        public void TagObstaclesWithinViewRange(Base2DEntity entity, float range)
        {
            EntityUtils.TagNeighbors(entity, _movingEntities, range);
        }

        public List<Wall2D> Wall
        {
            get
            {
                return _walls;
            }
        }

        public CellSpacePartition<MovingEntity> CellSpace
        {
            get
            {
                return _cellSpace;
            }
        }

        public List<MovingEntity> Agent
        {
            get
            {
                return _movingEntities;
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_paused)
            {
                Time.timeScale = 0;
            }

        }
    }
}
