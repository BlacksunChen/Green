using UnityEngine;
using System.Collections;
using Generic;

namespace Green
{
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
        
        protected override void Awake()
        {
            base.Awake();
            var obj = GameObject.Find(GameplayManager.Instance.World);
            _world = obj.GetComponent<GameWorld>();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}