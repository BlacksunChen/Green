using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utilities;

namespace Green
{
    public class GameWorld : MonoBehaviour
    {
        //List<MovingEntity> _movingEntities;
       // List<Base2DEntity> _obstacles;

        Dictionary<string, Star> _planets;

        public Dictionary<string, Star> Planets
        {
            get
            {
                return _planets;
            }
        }

        [SerializeField, SetProperty("PlayerPopulation")]
        float _playerPopulation = 0f;

        [SerializeField, SetProperty("EnemyPopulation")]
        float _enemyPopulation = 0f;


        public float PlayerPopulation
        {
            get
            {
                return _playerPopulation;
            }
        }

        public float EnemyPopulation
        {
            get
            {
                return _enemyPopulation;
            }
        }
        /// <summary>
        /// set true to pause the motion
        /// </summary>
        bool _paused = false;

        /// <summary>
        /// keeps track of the average FPS
        /// </summary>
        float _avFrameTime = 0f;

        void Awake()
        {
            /*
            var background = GameObject.Find(GameplayManager.Instance.Background);
            var meshSize = background.GetComponent<MeshRenderer>().bounds.size;
            _cxClient = meshSize.x;
            _cyClient = meshSize.y;
            Vector2 center = background.transform.position;
            Vector2 leftButtom = new Vector2(center.x - meshSize.x / 2, center.y - meshSize.y / 2);
            //_crosshair = new Vector2(_cxClient / 2.0f, _cxClient / 2.0f);
            //_path =NULL;

            //setup the spatial subdivision class
            //_cellSpace = new CellSpacePartition<MovingEntity>(leftButtom, _cxClient, _cyClient, SteeringParams.Instance.NumCellsX, SteeringParams.Instance.NumCellsY, SteeringParams.Instance.NumAgents);
            */
        }

        void Update()
        {
            if(GameManager.Instance.State == GameState.Playing)
            {
                StartCoroutine(UpdateSituationInEachPlanet());
            }
        }

        IEnumerator UpdateSituationInEachPlanet()
        {
            while (true)
            {
                UpdatePopulation();
                foreach (var p in _planets)
                {
                    p.Value.OnUpdateSituation();
                    p.Value.OnUpdateSoldierAnimation();
                }

                yield return new WaitForSeconds(Formula.CalculatePerTime);
            }
        }
       
        /// <summary>
        /// 更新双方人口
        /// </summary>
        void UpdatePopulation()
        {
            _enemyPopulation = 0f;
            _playerPopulation = 0f;
            foreach(var p in _planets)
            {
                if(p.Value.State == Star.e_State.AI)
                {
                    _enemyPopulation += p.Value.Capacity;
                }
                else if(p.Value.State == Star.e_State.Player)
                {
                    _playerPopulation += p.Value.Capacity;
                }
            }
        }

        /// </summary>
        void Start()
        {
            //UpdateSoldiersInfo();
            UpdatePlanetsInfo();
        }

        public void UpdatePlanetsInfo()
        {
            var planetsRoot = GameObject.Find(GameplayManager.PlanetsRoot);
            var planets = planetsRoot.GetComponentsInChildren<Star>();
            Debug.LogFormat("Update Planets: {0}", planets.Length);
            _planets = new Dictionary<string, Star>();
            foreach(var p in planets)
            {
                _planets.Add(p.name, p);
            }
        }
    }
}
