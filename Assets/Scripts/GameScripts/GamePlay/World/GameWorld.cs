using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Generic.Framework;

namespace Green
{
    public class GameWorld : Singleton<GameWorld>
    {
        List<MovingEntity> _movingEntity;

        List<Base2DEntity> _obstacles;

        List<Wall2D> _walls;
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
