using UnityEngine;
using System.Collections;

namespace Green
{
    public class PlanetStyle
    {
        public PlanetStyle()
        {
            /*
            GameObject go = Resources.Load<GameObject>(Settings.PLANET_PREFAB_PATH + "Image");
            go = GameObject.Instantiate(go);

            //set tag

            go.layer = Settings.Instance.ACTOR_LAYER;

            //set name
            go.name = info.ObjName;

            //add Image
            _image = go.GetComponent<Image>();
            Sprite sp = Resources.Load<Sprite>(info.Path + info.Name);

            if (sp)
            {
                _image.sprite = sp;
                _image.SetNativeSize();
            }
            else
            {
                Debug.LogFormat("Actor: {0} not found", info.Path + info.Name);
            }

            _transform = go.GetComponent<RectTransform>();

            //set local position
            _transform.anchorMin = Vector2.zero;
            _transform.anchorMax = Vector2.zero;

            //set parent
            _transform.SetParent(Settings.ActorRoot, true);

            //set position and scale
            _transform.anchoredPosition = info.Position;

            _transform.localScale = new Vector3(info.Scale, info.Scale, info.Scale);


            go.SetActive(false);
            this.Go = go;
            */
        }
    }
}