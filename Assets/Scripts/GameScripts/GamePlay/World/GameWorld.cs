using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Utilities;

namespace Green
{
    public class GameWorld : Singleton<GameWorld>
    {
        List<Star> _planets;

        public List<Star> Planets
        {
            get
            {
                return _planets;
            }
        }

       // Soldier[] _soldiers;

        public Soldier[] Soldiers
        {
            get
            {
                var rootGo = GameObject.Find(GameplayManager.SoldierRoot);
                if (rootGo == null) Debug.LogFormat("Can not find {0} in Scene!", GameplayManager.SoldierRoot);
                return rootGo.GetComponentsInChildren<Soldier>();
                //return _soldiers;
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

        protected override void Awake()
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
            _scoreBar = GameObject.FindObjectOfType<ScoreBar>();
            if (_scoreBar == null)
            {
                Debug.LogError("Need Progress Bar");
            }
            _game_UGui = GameObject.FindObjectOfType<game_uGUI>();
            if (_game_UGui == null)
            {
                Debug.LogError("Need game_uGUI");
            }
        }

        //战斗计算的定时器
        Timer _situationUpdateTimer;

        //ai计算的定时器
        Timer _aiUpdateTimer;

        void Update()
        {
            //if(GameManager.Instance.State == GameState.Playing)
            {          
                UpdateAI();
                UpdateSituationInEachPlanet();
                UpdateGameProgress();
            }
        }

        public void StartTimer()
        {
            _aiUpdateTimer.Resume();
            _situationUpdateTimer.Resume();
        }

        public void PauseTimer()
        {
            _aiUpdateTimer.Pause();
            _situationUpdateTimer.Pause();
        }

        ScoreBar _scoreBar;
        void UpdateGameProgress()
        {
            int playerCapture = 0;
            int enemyCapture = 0;
            foreach (var p in _planets)
            {
                if (p.State == Star.e_State.Player)
                {
                    ++playerCapture;
                }
                else if (p.State == Star.e_State.AI)
                {
                    ++enemyCapture;
                }
            }
            _scoreBar.SetLeftBarValue(playerCapture);
            _scoreBar.SetRightBarValue(enemyCapture);
            if (playerCapture == _planets.Count)
            {
                Victory();
            }
            else if(enemyCapture == _planets.Count)
            {
                GameOver();
            }
            
        }

        game_uGUI _game_UGui;
        public void Victory()
        {
            _game_UGui.Victory();
            GameManager.Instance.ChangeState(GameState.GameOver);
        }

        public void GameOver()
        {
            _game_UGui.Defeat();
            GameManager.Instance.ChangeState(GameState.GameOver);
        }
        void UpdateAI()
        {
           // _aiUpdateTimer.Resume();
            _aiUpdateTimer.Update();
            if (_aiUpdateTimer.CurrentState == TimerState.FINISHED)
            {
                AI.GetInstance().runAI();
            }
        }
        int _updateCountIndex = 0;

        void UpdateSituationInEachPlanet()
        {
           // _situationUpdateTimer.Resume();
            _situationUpdateTimer.Update();
            if (_situationUpdateTimer.CurrentState == TimerState.FINISHED)
            {
                DebugInConsole.LogFormat("*******************第{0}秒*******************", _updateCountIndex++);
                UpdateSoldiersInPlanets();
                UpdatePopulation();
                foreach (var p in _planets)
                {
                    DebugInConsole.LogFormat("*****星球: {0}*****", p.name);
                    p.OnUpdateSituation();
                    p.OnUpdateSoldierAnimation();
                    DebugInConsole.Log      ("*******************");
                }
                DebugInConsole.LogFormat("*********************************************");
                //yield return new WaitForSeconds(Formula.CalculatePerTime);
                //}
            }
        }
       
        void UpdateSoldiersInPlanets()
        {
            DebugInConsole.Log("刷新星球内士兵...");
            
            DebugInConsole.LogFormat("敌我士兵总数: {0}", Soldiers.Length);
            foreach (var p in Planets)
            {
                p.Planet_.PlayerSoldiers.Clear();
                p.Planet_.EnemySoldiers.Clear();
            }
            foreach(var s in Soldiers)
            {
                switch (s.Bloc)
                {
                    case SoldierType.Player:
                        if(s.InPlanet != null)
                            s.InPlanet.PlayerSoldiers.Add(s);
                        break;
                    case SoldierType.Enemy:
                        if(s.InPlanet != null)
                            s.InPlanet.EnemySoldiers.Add(s);
                        break;
                    default:
                        break;
                }
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
                _enemyPopulation += p.EnemyTroops;
                _playerPopulation += p.PlayerTroops;

                if (p.State == Star.e_State.AI)
                {                  
                    _enemyMaxPopulation += p.Capacity;
                }
                else if(p.State == Star.e_State.Player)
                {
                    _playerMaxPopulation += p.Capacity;
                }
            }
            DebugInConsole.LogFormat("敌方总人口：{0} 目前人口: {1}", _enemyMaxPopulation, _enemyPopulation);
            DebugInConsole.LogFormat("我方总人口：{0} 目前人口: {1}", _playerMaxPopulation, _playerPopulation);
        }

        void Start()
        {
            _situationUpdateTimer = new Timer(Formula.CalculatePerTime, true, true);
            _aiUpdateTimer = new Timer(0.5f, true, true);
            //UpdateSoldiersInfo();
            UpdatePlanetsInfo();
        }

        public void UpdatePlanetsInfo()
        {
            var planetsRoot = GameObject.Find(GameplayManager.PlanetsRoot);
            var planets = planetsRoot.GetComponentsInChildren<Star>();
            Debug.LogFormat("Update Planets: {0}", planets.Length);
            _planets = new List<Star>();
            foreach(var p in planets)
            {
                _planets.Add(p);
            }
            _scoreBar.MaxValue = _planets.Count;
        }
        #region Test Send Soldier
        public void OnSendSoldier()
        {
            var from = GameObject.Find("planet_1").GetComponent<Planet>();
            var to = GameObject.Find("planet_3").GetComponent<Planet>();

            SendSoldier(from, to, 5, SoldierType.Player);
        }

        public void OnSendSoldier2To3()
        {
            var from = GameObject.Find("planet_2").GetComponent<Planet>();
            var to = GameObject.Find("planet_3").GetComponent<Planet>();

            SendSoldier(from, to, 5, SoldierType.Player);
        }
        public void OnSendSoldier3To4()
        {
            var from = GameObject.Find("planet_3").GetComponent<Planet>();
            var to = GameObject.Find("planet_4").GetComponent<Planet>();

            SendSoldier(from, to, 5, SoldierType.Player);
        }
        #endregion

        #region Interface
        public void SendSoldier(Planet from, Planet to, int soldierNum, SoldierType type)
        {
            List<Soldier> soldierInPlanetFrom;
            switch (type)
            {
                case SoldierType.Player:
                    soldierInPlanetFrom = from.PlayerSoldiers;
                    break;
                case SoldierType.Enemy:
                    soldierInPlanetFrom = from.EnemySoldiers;
                    break;
                default:
                    soldierInPlanetFrom = new List<Soldier>();
                    break;
            }
            if (soldierNum > soldierInPlanetFrom.Count)
            {
                soldierNum = soldierInPlanetFrom.Count;
            }
            var soldiers = soldierInPlanetFrom.GetRange(0, soldierNum);
            Debug.LogFormat("{0} 派 {1}兵 从{2}到{3}", type.ToString(), soldierNum, from.name, to.name);
            foreach (var s in soldiers)
            {
                s.UpdateState(Soldier.StateType.Move);
                from.SoldierLeave(s, s.Bloc);
                s.SetSeekDestination(to,
                    () =>
                    {
                        to.SoldierArrive(s, s.Bloc);
                    });
            }
        }

        public int GetPlanetID(Star star)
        {
            if (star == null)
            {
                Debug.LogError("GetPlanetID Error: star == null");
                return -1;
            }
            for (int i = 0; i < _planets.Count; ++i)
            {
                if (_planets[i] == star)
                {
                    return i;
                }
            }
            Debug.LogError("GetPlanetID Error!");
            return -1;
        }
        #endregion
    }
}
