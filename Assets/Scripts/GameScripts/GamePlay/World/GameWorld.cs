using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utilities;

namespace Green
{
    public class GameWorld : Singleton<GameWorld>
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
        float _playerPopulation = 0;

        [SerializeField, SetProperty("EnemyPopulation")]
        float _enemyPopulation = 0;

        [SerializeField, SetProperty("PlayerMaxPopulation")]
        float _playerMaxPopulation = 0;

        [SerializeField, SetProperty("EnemyMaxPopulation")]
        float _enemyMaxPopulation = 0;

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

        public float PlayerMaxPopulation
        {
            get
            {
                return _playerMaxPopulation;
            }
        }

        public float EnemyMaxPopulation
        {
            get
            {
                return _enemyMaxPopulation;
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

        Timer _timer;

        void Update()
        {
            if(GameManager.Instance.State == GameState.Playing)
            {          
                UpdateSituationInEachPlanet();
            }
        }
        int updateCountIndex = 0;
        void UpdateSituationInEachPlanet()
        {
            _timer.Resume();
            _timer.Update();
            if (_timer.CurrentState == TimerState.FINISHED)
            {
                DebugInConsole.LogFormat("*******************第{0}秒*******************", updateCountIndex++);
                UpdatePopulation();
                foreach (var p in _planets)
                {
                    DebugInConsole.LogFormat("*****星球: {0}*****", p.Value.name);
                    p.Value.OnUpdateSituation();
                    p.Value.OnUpdateSoldierAnimation();
                    DebugInConsole.Log      ("*******************");
                }
                DebugInConsole.LogFormat("*********************************************");
                //yield return new WaitForSeconds(Formula.CalculatePerTime);
                //}
            }
        }
       
        /// <summary>
        /// 更新双方人口
        /// </summary>
        void UpdatePopulation()
        {
            _enemyPopulation = 0;
            _playerPopulation = 0;
            _playerMaxPopulation = 0;
            _enemyMaxPopulation = 0;
            foreach(var p in _planets)
            {
                _enemyPopulation += p.Value.EnemyTroops;
                _playerPopulation += p.Value.PlayerTroops;

                if (p.Value.State == Star.e_State.AI)
                {                  
                    _enemyMaxPopulation += p.Value.Capacity;
                }
                else if(p.Value.State == Star.e_State.Player)
                {
                    _playerMaxPopulation += p.Value.Capacity;
                }
            }
            DebugInConsole.LogFormat("敌方总人口：{0} 目前人口: {1}", _enemyMaxPopulation, _enemyPopulation);
            DebugInConsole.LogFormat("我方总人口：{0} 目前人口: {1}", _playerMaxPopulation, _playerPopulation);
        }

        /// </summary>
        void Start()
        {
            _timer = new Timer(Formula.CalculatePerTime, true, true);
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
