using UnityEngine;
using System.Collections;

namespace Green
{
    public class ArrowRenderer : MonoBehaviour
    {
        [SerializeField] private LineRenderer BodyLineRenderer;
        [SerializeField] private LineRenderer HeadLineRenderer;

        private float zDepth = 10f;
        private float headWidth = 0.4f;
        private float bodyWidth = 0.3f;

        void Awake()
        {
            var lines = GetComponentsInChildren<LineRenderer>();
            if (lines.Length != 2)
            {
                Debug.LogError("Set Two GameObject with LineRenderer");
            }
            BodyLineRenderer = lines[0];
            HeadLineRenderer = lines[1];
        }

        public void Draw(Vector2 startPos, Vector2 endPos)
        {
            zDepth = transform.position.z;

            var StartPos = new Vector3(startPos.x, startPos.y, zDepth);
            var EndPos = new Vector3(endPos.x, endPos.y, zDepth);

            BodyLineRenderer.SetPosition(0, StartPos);
            BodyLineRenderer.SetPosition(1, EndPos);

            // 设置头部
            Vector3 HeadStart = EndPos;
            Vector3 HeadEnd = HeadStart;

            Vector3 direction = EndPos - StartPos;

            // 根据当前线的长短，适当的调整箭头的大小
            if (direction.magnitude > 2)
            {
                headWidth = 0.4f;
                bodyWidth = 0.2f;
            }
            else if (direction.magnitude > 1)
            {
                headWidth = 0.3f;
                bodyWidth = 0.15f;
            }
            else
            {
                headWidth = 0.2f;
                bodyWidth = 0.1f;
            }
            HeadLineRenderer.SetWidth(headWidth, 0);
            BodyLineRenderer.SetWidth(0, bodyWidth);

            direction.Normalize();
            HeadEnd = HeadEnd + direction*headWidth;

            HeadLineRenderer.SetPosition(0, HeadStart);
            HeadLineRenderer.SetPosition(1, HeadEnd);
        }

        public void Clear()
        {
            HeadLineRenderer.SetWidth(0f, 0f);
            BodyLineRenderer.SetWidth(0f, 0f);
        }
    }

}

