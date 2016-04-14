using UnityEngine;
using System.Collections;
using Utilities;

namespace Green
{
    public enum GameplayType
    {
        Planet,
        Player,
        Enemy,
    }
    public class GameplayManager : Singleton<GameplayManager>
    {
        public const string Root = "Gameplay/";
        public const string Background = "Background/";
        public const string PlanetsRoot = "Planets/";
        public const string SoldierRoot = "Soldiers/";
        public const string World = "Gameplay/";

        public string GetRoot(GameplayType type)
        {
            string root = Root + Background;
            switch (type)
            {
                case GameplayType.Planet:
                    root += PlanetsRoot;
                    break;
                case GameplayType.Player:
                    root += SoldierRoot + "Players";
                    break;
                case GameplayType.Enemy:
                    root += SoldierRoot + "Enemies";
                    break;
                default:
                    Debug.LogError("AddToScene Error");
                    break;
            }
            return root;
        }

        public void AddToScene<T>(T entity, GameplayType type)
            where T : Base2DEntity
        {
            AddToScene(GetRoot(type), entity.transform);
        }

        public void AddToScene(string root, Transform go)
        {
            var rootObj = GameObject.Find(root);
            if (rootObj == null)
            {
                Debug.LogErrorFormat("Gameobject: {0} not found!", root);
            }
            go.SetParent(rootObj.transform);
        }
    }
}