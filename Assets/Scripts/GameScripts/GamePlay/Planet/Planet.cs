﻿using System;
using UnityEngine;
using System.Collections.Generic;
using Utilities;

namespace Green
{
    /// <summary>
    /// 星球类
    /// </summary>
    public class Planet : MonoBehaviour
    {
        public CircleBorder2D OutCircle;

        public CircleBorder2D InCircle;

        Star _star;

        public Star GetStar()
        {
            return _star;
        }

        public List<Soldier> PlayerSoldiers;
        public List<Soldier> EnemySoldiers;

        public Star GetProperty()
        {
            if (_star == null)  Debug.LogError("Lose Script: Star");
            return _star;
        }

        public float InCircleRad
        {
            get { return InCircle.Radius; }
        }

        public float OutCircleRad
        {
            get { return OutCircle.Radius; }
        }

        public bool IsIntersection(Vector2 A,
                                   Vector2 B,
                               out float dist,
                               out Vector2 point,
                               out CircleBorder2D.Area type)
        {
            bool isOut = false, isIn = false;
            isOut = OutCircle.IsIntersection(A, B, out dist, out point);
            if (isOut)
            {
                type = CircleBorder2D.Area.OutCircle;
                return isOut;
            }
            isIn = InCircle.IsIntersection(A, B, out dist, out point);
            if(isIn)
            {
                type = CircleBorder2D.Area.InCircle;
                return isIn;
            }
            type = CircleBorder2D.Area.None;
            return false;
            
        }

        /// <summary>
        /// 切线的法线
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual Vector2 GetTangentNormal(Vector2 point)
        {
            //反正是同心圆
            var center = InCircle.CenterInWorldSpace;
            return new Vector2(point.x - center.x, point.y - center.y).normalized;
        }

        /// <summary>
        /// 根据星球类型读取配置文件
        /// 创建prefab
        /// </summary>
        public void Init()
        {

        }
        void Awake()
        {
            PlayerSoldiers = new List<Soldier>();
            EnemySoldiers = new List<Soldier>();
        }

       
        // Use this for initialization
        void Start()
        {
            SetCircleWall();
            _star = GetComponent<Star>();
            if (_star == null) Debug.LogError("Need Script: Star");

        }

        
        public void SoldierArrive(Soldier s, SoldierType type)
        {
            switch (type)
            {
                case SoldierType.Player:
                    PlayerSoldiers.Add(s);
                    _star.PlayerTroops++;
                    break;
                case SoldierType.Enemy:
                    EnemySoldiers.Add(s);
                    _star.EnemyTroops++;
                    break;
                default:
                    break;
            }
        }

        public void AddSoliders(List<Soldier> s, SoldierType type)
        {
            switch (type)
            {
                case SoldierType.Player:
                    PlayerSoldiers.AddRange(s);
                    _star.PlayerTroops += s.Count;
                    break;
                case SoldierType.Enemy:
                    EnemySoldiers.AddRange(s);
                    _star.EnemyTroops += s.Count;
                    break;
                default:
                    break;
            }
        }

        public void SoldierLeave(Soldier s, SoldierType type)
        {
            switch (type)
            {
                case SoldierType.Player:
                    PlayerSoldiers.Remove(s);
                    _star.PlayerTroops--;
                    break;
                case SoldierType.Enemy:
                    EnemySoldiers.Remove(s);
                    _star.EnemyTroops--;
                    break;
                default:
                    break;
            }
        }

        void SetCircleWall()
        {
            var walls = GetComponentsInChildren<CircleBorder2D>();
            bool inExist = false;
            bool onExist = false;
            foreach(var wall in walls)
            {
                if(wall.gameObject.name == "In")
                {
                    inExist = true;
                    InCircle = wall;
                }
                if(wall.gameObject.name == "Out")
                {
                    onExist = true;
                    OutCircle = wall;
                }
            }
            if(!InCircle && !OutCircle)
            {
                Debug.LogError("Planet need both in and out border!");
            }
            //内圈
            
        }
        // Update is called once per frame
        void Update()
        {

        }

        public Vector2 GetRandomPositionInPlanet()
        {
            return Geometry.GetRamdomPointOnRing(OutCircleRad, InCircleRad, transform.position);
        }

        public void UpdateSoldiersToCount(int to, SoldierType type)
        {
            int detla = 0;
            switch (type)
            {
                case SoldierType.Player:
                    detla = to - PlayerSoldiers.Count;
                    DebugInConsole.LogFormat("我方增加兵力:{0} 之前兵力{1} 目前兵力{2}", detla, PlayerSoldiers.Count, to);
                    break;
                case SoldierType.Enemy:
                    detla = to - EnemySoldiers.Count;
                    DebugInConsole.LogFormat("敌方增加兵力:{0} 之前兵力{1} 目前兵力{2}", detla, EnemySoldiers.Count, to);
                    break;
                default:
                    break;
            }

            //加兵
            if(detla > 0)
            {
                CreateSoldiers(detla, type);
            }
            else if(detla < 0)
            {
                RandomRemoveSoldiers(-detla, type);
            }
        }

        public void CreateSoldiers(int count, SoldierType type)
        {
            for(int i = 0; i < count; ++i)
            {
                Soldier_Style style;
                switch (type)
                {
                    case SoldierType.Enemy:
                        style = Soldier_Style.Enemy_1;
                        break;
                    case SoldierType.Player:
                    default:
                        style = Soldier_Style.Player_1;
                        break;
                    
                }
                var soldier = new SoldierStyle(this, style, type);
                if (soldier == null)
                    Debug.LogErrorFormat("CreateSoldier Failed in Planet: {0}", name);
                AddSolider(soldier.GetSoldier(), type);
            }
        }

        public void AddSolider(Soldier s, SoldierType type)
        {
            switch (type)
            {
                case SoldierType.Player:
                    PlayerSoldiers.Add(s);
                    //_star.PlayerTroops++;
                    break;
                case SoldierType.Enemy:
                    EnemySoldiers.Add(s);
                    //_star.EnemyTroops++;
                    break;
                default:
                    break;
            }
        }

        public void RandomRemoveSoldiers(int count, SoldierType type)
        {
            List<Soldier> list = new List<Soldier>();
            switch (type)
            {
                case SoldierType.Player:
                    list = PlayerSoldiers;
                    //_star.PlayerTroops -= count;
                    break;
                case SoldierType.Enemy:
                    list = EnemySoldiers;
                    //_star.EnemyTroops -= count;
                    break;
                default:
                    break;
            }
            while(count-- > 0)
            {
                var idx = UnityEngine.Random.Range(0, Math.Min(0, list.Count-1));
                
                var go = list[idx];
                list.Remove(go);
                go.Destory();                 
            }
        }


    }
}