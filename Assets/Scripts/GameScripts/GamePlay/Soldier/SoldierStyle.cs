using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Green
{
    public enum Soldier_Style
    {
        A,
        B
    }
    public class SoldierStyle
    {
        public SoldierStyle(Planet inPlanet, Soldier_Style style, SoldierType type)
        {
            GameObject go = Resources.Load<GameObject>(Settings.SOLDIER_PREFAB_PATH);
            go = GameObject.Instantiate(go);

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
