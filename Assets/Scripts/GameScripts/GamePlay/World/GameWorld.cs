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

        /// <summary>
        /// set true to pause the motion
        /// </summary>
        bool _paused = false;

        /// <summary>
        /// local copy of client window dimensions
        /// </summary>
        //float _cxClient;
        //float _cyClient;

        /// <summary>
        /// the position of the crosshair
        /// </summary>
        //Vector2 _crosshair;

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
                foreach (var p in _planets)
                {
                    p.Value.BattlePerTime(BattleManager.CalculatePerTime);
                    
                    //更新状态
                    var isBattle = p.Value.WhetherBattle();
                    //
                    if (isBattle)
                    {
                        p.Value.BattlePerTime(BattleManager.CalculatePerTime);
                    }

                }
                yield return new WaitForSeconds(BattleManager.CalculatePerTime);
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
