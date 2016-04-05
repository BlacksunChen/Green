using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Generic;

namespace Green
{
    public class GameWorld : MonoBehaviour
    {
        List<MovingEntity> _movingEntities;

       // List<Base2DEntity> _obstacles;

        List<Planet> _planets;

        CellSpacePartition<MovingEntity> _cellSpace;

        //Path _path;

        /// <summary>
        /// set true to pause the motion
        /// </summary>
        bool _paused = false;

        /// <summary>
        /// local copy of client window dimensions
        /// </summary>
        float _cxClient;
        float _cyClient;

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

        void Awake()
        {
            var background = GameObject.Find(GameplayManager.Instance.Background);
            var meshSize = background.GetComponent<MeshRenderer>().bounds.size;
            _cxClient = meshSize.x;
            _cyClient = meshSize.y;
            Vector2 center = background.transform.position;
            Vector2 leftButtom = new Vector2(center.x - meshSize.x / 2, center.y - meshSize.y / 2);
            _crosshair = new Vector2(_cxClient / 2.0f, _cxClient / 2.0f);
            //_path =NULL;

            //setup the spatial subdivision class
            _cellSpace = new CellSpacePartition<MovingEntity>(leftButtom, _cxClient, _cyClient, SteeringParams.Instance.NumCellsX, SteeringParams.Instance.NumCellsY, SteeringParams.Instance.NumAgents);
        }

        void Start()
        {
            UpdateSoldiersInfo();
            UpdatePlanetsInfo();
        }
        public void UpdatePlanetsInfo()
        {
            var planetsRoot = GameObject.Find(GameplayManager.Instance.PlanetsRoot);
            var planets = planetsRoot.GetComponentsInChildren<Planet>();
            Debug.LogFormat("Update Planets: {0}", planets.Length);
            _planets = new List<Planet>();
            foreach(var p in planets)
            {
                _planets.Add(p);
            }
        }
        public void UpdateSoldiersInfo()
        {
            var soliderRoot = GameObject.Find(GameplayManager.Instance.SoldierRoot);
            var soldiers = GetComponentsInChildren<MovingEntity>();
            Debug.LogFormat("Update Soldiers: {0}", soldiers.Length);
            _movingEntities = new List<MovingEntity>();
            foreach (var s in soldiers)
            {
                _movingEntities.Add(s);
                _cellSpace.AddEntity(s);
            }
        }
        public void NonPenetrationContraint(MovingEntity v)
        {
            EntityUtils.EnforceNonPenetrationConstraint(v, _movingEntities);
        }

        public void TagVehiclesWithinViewRange(MovingEntity entity, float range)
        {
            EntityUtils.TagNeighbors(entity, _movingEntities, range);
        }

        public void TagObstaclesWithinViewRange(MovingEntity entity, float range)
        {
            EntityUtils.TagNeighbors(entity, _movingEntities, range);
        }

        public List<Planet> Planets
        {
            get
            {
                return _planets;
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
