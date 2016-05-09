using DG.Tweening;
using UnityEngine;
using Utilities;

namespace Green
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Planet))]
    public class Star : MonoBehaviour
    {
		
        void SetProperty(
            e_State state,
            int def,
            int vigour,
            int capacity,
            Vector2 location,
            float enemyTroops,
            float playerTroops,
            float schedule)
        {
            SetInitState(state);
            _DEF = def;
            _vigour = vigour;
            _capacity = capacity;
            _location = location;
            _enemyTroops = enemyTroops;
            _playerTroops = playerTroops;
            _schedule = schedule;
        }

        //星球所处状态
        public enum e_State
        {
            Player = 0,             //属于玩家的和平星球
            AI = 1,                 //属于电脑的和平星球
            NeutralityPeace = 2,    //中立
            NeutralityToPlayer = 3, //玩家正在占领的中立星球
            NeutralityToAI = 4,     //电脑正在占领的中立星球
        }

        private FSM _fsm;

        private FSMState _fsmState;

        [SerializeField, SetProperty("SelectedState")]
        private e_State _selectedState = e_State.Player;

        public FSM FiniteStateMachine
        {
            get
            {
                return _fsm;
            }
        }

        
        public e_State SelectedState
        {
            get
            {
                return _selectedState;
            }
            set
            {
                _selectedState = value;
            }
        }
        
        public e_State State
        {
            get
            {
                return _fsmState.EnumState;
            }
        }
        
        public FSMState FsmState
        {
            get
            {
                return _fsmState;
            }
        }

        [SerializeField, SetProperty("DEF")]
        [Range(0f, 10f)]
        private int _DEF;//防御力

        [SerializeField, SetProperty("Vigour")]
        [Range(0f, 10f)]
        private int _vigour;//活力，增长量

        [SerializeField, SetProperty("Capacity")]
        [Range(0f, 20f)]
        private int _capacity;//容量

        [SerializeField, SetProperty("Location")]
        private Vector2 _location;
        //private Tuple<int, int> _location;//坐标


        //private Tuple<double, double> _troops;//双方兵力，前者是玩家，后者是AI

        [SerializeField, SetProperty("PlayerTroops")]
        private float _playerTroops;

        [SerializeField, SetProperty("EnemyTroops")]
        private float _enemyTroops;

        [SerializeField, SetProperty("Schedule")]
        private float _schedule;//中立星球占领进度（中立星球的那四个状态有效）

        public float PlayerTroops
        {
            get
            {
                return _playerTroops;
            }
            set
            {
                if (value < 0f)
                    _playerTroops = 0f;
                else
                    _playerTroops = value;
            }
        }

        public float EnemyTroops
        {
            get
            {
                return _enemyTroops;
            }
            set
            {
                if (value < 0f)
                    _enemyTroops = 0f;
                else
                    _enemyTroops = value;
            }
        }
        public int DEF
        {
            get
            {
                return _DEF;
            }
            set
            {
                _DEF = value;
            }
        }
        public int Vigour
        {
            get
            {
                return _vigour;
            }
            set
            {
                _vigour = value;
            }
        }
        public int Capacity
        {
            get
            {
                return _capacity;
            }
            set
            {
                _capacity = value;
            }
        }
        public Vector2 Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        public float Schedule
        {
            get
            {
                return _schedule;
            }
            set
            {
                _schedule = value;
            }
        }

        public Star(): base()
        {
            _fsm = new FSM(this);
        }

        void Start()
        {
            SetProperty(_selectedState, DEF, Vigour, Capacity, Location, EnemyTroops, PlayerTroops, Schedule);
        }

        void SetInitState(e_State state)
        {
            _fsmState = _fsm.GetState(state);
            _fsmState.OnEnter();
        }

        public bool IsBattleInPlanet()
        {
            //被占领 和平
            if (_enemyTroops > 0 && _playerTroops > 0)
            {
                return true;
            }
            return false;
        }

        public void BattlePerTime(float perTime)
        {
            if (!IsBattleInPlanet()) return;

            float enemy = 0f;
            float player = 0f;

            if (State == e_State.AI)
            {
                enemy += Formula.CalculateDamageForDefOnePerTime(_enemyTroops, _playerTroops, DEF, perTime);
                player += Formula.CalculateDamageForAttackOnePerTime(_enemyTroops, _playerTroops, DEF, perTime);
            }
            else if (State == e_State.Player)
            {
                enemy += Formula.CalculateDamageForAttackOnePerTime(_playerTroops, _enemyTroops, DEF, perTime);
                player += Formula.CalculateDamageForDefOnePerTime(_playerTroops, _enemyTroops, DEF, perTime);
            }
            else
            {
                enemy += Formula.CalculateDamageForNeutralOnePerTime(_enemyTroops, _playerTroops, perTime);
                player += Formula.CalculateDamageForNeutralOnePerTime(_enemyTroops, _playerTroops, perTime);
            }
            EnemyTroops += enemy;
            PlayerTroops += player;
            DebugInConsole.LogFormat("我方损失兵力:{0} 总兵力:{1}", player, PlayerTroops);
            DebugInConsole.LogFormat("敌方损失兵力:{0} 总兵力:{1}", enemy, EnemyTroops);
            //Truncate(ref _enemyTroops, 0f);
            //Truncate(ref _playerTroops, 0f);
           
        }

        bool _isDuringCapture = false;
        
        public void StartCapture()
        {
            _schedule = 0;
            _isDuringCapture = true;
        }

        public void StopCapture()
        {
            _isDuringCapture = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnUpdateSituation()
        {
            //本状态执行更新
            _fsmState.OnUpdate();
            
            //战斗清算
            BattlePerTime(Formula.CalculatePerTime);
           
            //判断是否有到条件进入下一个状态
            _fsmState = _fsmState.NextState();
        }

        Planet m_planet = null;
        public Planet Planet_
        {
            get
            {
                if (m_planet == null)
                {
                    m_planet = GetComponent<Planet>();
                    if (m_planet == null)
                        Debug.LogError("Need Script: Planet");
                }
                return m_planet;
            }
        }

        /// <summary>
        /// 更新上层士兵动画：增加/删除
        /// </summary>
        public void OnUpdateSoldierAnimation()
        {
            int enemyCount = Mathf.FloorToInt(_enemyTroops);
            int playerCount = Mathf.FloorToInt(_playerTroops);
            
            Planet_.UpdateSoldiersToCount(enemyCount, SoldierType.Enemy);
            Planet_.UpdateSoldiersToCount(playerCount, SoldierType.Player);
        }

        void Truncate(ref float num, float min, float max)
        {
            if (num >= max) num = max;
            if (num <= min) num = min;
        }

        void Truncate(ref float num, float min)
        {
            if (num <= min) num = min;
        }

        #region AI Interface
        public void SendAISoldiers(Star to, int soldierNum)
        {
            GameWorld.Instance.SendSoldier(this.Planet_, to.Planet_, soldierNum, SoldierType.Enemy);
        }
        #endregion

        public void CrossFadeStarImage(Sprite nextImage)
        {
            var image = GetComponent<SpriteRenderer>();
            var sequence = DOTween.Sequence();

            sequence.Append(image.DOFade(0f, 0.5f))
                .AppendCallback(() =>
                {
                    Debug.LogFormat("{0} change from {1} to {2}", name, image.sprite.name, nextImage.name);
                    image.sprite = nextImage;
                })
                //.AppendInterval(0.1f)
                .Append(image.DOFade(1f, 0.5f));
        }
    }
}
