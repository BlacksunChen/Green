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

    //游戏管理类 相当于game_master 不删除，管理用户设定数据， 当前关卡   
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

        [SerializeField, SetProperty("State")]
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
            Time.timeScale = 1;
        }

        public void OnChangeStatePause()
        {
            Time.timeScale = 0;
        }

        public void OnChangeStateGameOver()
        {

        }

        protected override void Awake()
        {
            IsPersistent = true;
            base.Awake();
            var obj = GameObject.Find(GameplayManager.World);
            _world = obj.GetComponent<GameWorld>();
        }


    }
}