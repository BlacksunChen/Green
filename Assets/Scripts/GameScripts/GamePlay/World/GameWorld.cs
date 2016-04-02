using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Generic;

namespace Green
{
    public class GameWorld : Singleton<GameWorld>
    {
        List<MovingEntity> _movingEntities;

       // List<Base2DEntity> _obstacles;

        List<Wall2D> _walls;

        CellSpacePartition<MovingEntity> _cellSpace;

        //Path _path;

        /// <summary>
        /// set true to pause the motion
        /// </summary>
        bool _paused = false;

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
        float _avFrameTime = 0f;

        /// <summary>
        /// //flags to turn aids and obstacles etc on/off
        /// </summary>
        bool _showWalls = false;
        bool _showObstacles = false;
        bool _showPath = false;
        bool _showDetectionBox = false;
        bool _showWanderCircle = false;
        bool _showFeelers = false;
        bool _showSteeringForce = false;
        //bool _showFPS = true;
        bool _renderNeighbors = false;
        //bool _viewKeys = false;
        bool _showCellSpaceInfo = false;

        void Init(int cx, int cy)
        {
            _cxClient =cx;
            _cyClient =cy;
            _crosshair =new Vector2(_cxClient / 2.0f, _cxClient / 2.0f);
            //_path =NULL;

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

        public List<Wall2D> Walls
        {
            get
            {
                return _walls;
            }
        }

        public Vector2 Crosshair
        {
            get
            {
                return _crosshair;
            }
            set
            {
                _crosshair = value;
            }
        }
        public CellSpacePartition<MovingEntity> CellSpace
        {
            get
            {
                return _cellSpace;
            }
        }

        public List<MovingEntity> Agents
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
