using UnityEngine;
using Utilities;
using System.Collections.Generic;

namespace Green
{
    public enum SoldierType
    {
        Player,
        Enemy
    }

    public enum GameState
    {
        Playing,
        Pause,
        GameOver
    }
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField, SetProperty("World")]
        private GameWorld _world;
        public GameWorld World
        {
            get
            {
                if (_world == null)
                {
                    Debug.LogError("GameManager.World not assign");
                }
                return _world;
            }
        }

        private GameState _state = GameState.Playing;
        public GameState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        public void ChangeState(GameState state)
        {
            _state = state;
            switch (state)
            {
                case GameState.Playing:
                    OnChangeStatePlay();
                    break;
                case GameState.Pause:
                    OnChangeStatePause();
                    break;
                case GameState.GameOver:
                    OnChangeStateGameOver();
                    break;
                default:
                    break;
            }
        }

        public void OnChangeStatePlay()
        {

        }

        public void OnChangeStatePause()
        {

        }
        public void OnChangeStateGameOver()
        {

        }
        protected override void Awake()
        {
            base.Awake();
            var obj = GameObject.Find(GameplayManager.World);
            _world = obj.GetComponent<GameWorld>();
        }

        public void OnSendSoldier()
        {
            var from = GameObject.Find("planet_1").GetComponent<Planet>();
            var to = GameObject.Find("planet_2").GetComponent<Planet>();
            
            SendSoldier(from, to, 5, SoldierType.Player);
        }

        public void OnSendSoldier2To3()
        {
            var from = GameObject.Find("planet_2").GetComponent<Planet>();
            var to = GameObject.Find("planet_3").GetComponent<Planet>();

            SendSoldier(from, to, 5, SoldierType.Player);
        }
        public void OnSendSoldier3to4()
        {
            var from = GameObject.Find("planet_3").GetComponent<Planet>();
            var to = GameObject.Find("planet_4").GetComponent<Planet>();

            SendSoldier(from, to, 5, SoldierType.Player);
        }
        public void SendSoldier(Planet from, Planet to, int soldierNum, SoldierType type)
        {
            List<Soldier> soldierInPlanetFrom;
            List<Soldier> soldierInPlanetTo;
            switch (type)
            {
                case SoldierType.Player:
                    soldierInPlanetFrom = from.PlayerSoldiers;
                    soldierInPlanetTo = to.PlayerSoldiers;
                    break;
                case SoldierType.Enemy:
                    soldierInPlanetFrom = from.EnemySoldiers;
                    soldierInPlanetTo = to.EnemySoldiers;
                    break;
                default:
                    soldierInPlanetFrom = new List<Soldier>();
                    soldierInPlanetTo = new List<Soldier>();
                    break;
            }
            if (soldierNum > soldierInPlanetFrom.Count)
            {
                soldierNum = soldierInPlanetFrom.Count;
            }
            var soldiers = soldierInPlanetFrom.GetRange(0, soldierNum);

            foreach (var s in soldiers)
            {
                s.UpdateState(Soldier.StateType.Move);
                s.SetSeekDestination(to,
                    () =>
                    {
                        to.AddSolider(s, s.Bloc);

                        from.RemoveSoldier(s, s.Bloc);
                    });
            }
        }
    }
}