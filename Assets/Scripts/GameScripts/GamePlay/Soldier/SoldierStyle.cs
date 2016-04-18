using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Green
{
    public enum Soldier_Style
    {
        None,
        A,
        B
    }

    public class SoldierStyle
    {
        Soldier _soldier;
        public Soldier GetSoldier()
        {
            return _soldier;
        }

        GameObject _go;

        //TODO
        public static string GetPrefabPath(Soldier_Style style, SoldierType type)
        {
            string path = Settings.SOLDIER_PREFAB_PATH;
            if (style == Soldier_Style.None)
                path += "plane";
            return path;
        }

        public SoldierStyle(Planet inPlanet, Soldier_Style style, SoldierType type)
        {
            _go = Resources.Load<GameObject>(GetPrefabPath(style, type));
            var go = _go;
            go = GameObject.Instantiate(go);

            _soldier = go.GetComponent<Soldier>();
            if (_soldier == null)
                Debug.LogError("Need Script: Soldier");

            _soldier.Style = this;
            _soldier.Bloc = type;
            _soldier.InPlanet = inPlanet;
            //set tag
            go.tag = Settings.SOLDIER_TAG;

            //set name
            go.name = GenerateSoldierName(inPlanet, type);

            var transform = go.transform;

            var z = transform.position.z;

            var pos = inPlanet.GetRandomPositionInPlanet();
            transform.position = new Vector3(pos.x, pos.y, z);
            //set parent
            transform.SetParent(GameObject.Find(GameplayManager.SoldierRoot).transform, false);

            go.SetActive(true);     
        }

        string GenerateSoldierName(Planet planet, SoldierType type)
        {
            return "Soldier" + "_in_" + planet.name + "_" + type.ToString();
        }
    }
}
