using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Green
{
    public class ScoreBar : MonoBehaviour
    {
        public Slider _centerImage;

        public Image _leftSilder;
        public Image _rightSilder;

       
        void Awake()
        {
            foreach (Transform t in transform)
            {
                switch (t.name)
                {
                    case "left_bar":
                        _leftSilder = t.GetComponent<Image>();
                        break;
                    case "right_bar":
                        _rightSilder = t.GetComponent<Image>();
                        break;
                    case "center_image":
                        _centerImage = t.GetComponent<Slider>();
                        break;
                }
            }
            if (!_leftSilder || !_rightSilder || !_centerImage)
            {
                Debug.LogError("Component in ScoreBar missing!");
            }
            
        }

        void Start()
        {
            _centerImage.gameObject.SetActive(false);
            CheckIfStarShow();
            // MaxValue = GameWorld.Instance.Planets.Count;
        }

        public int MaxValue
        {
            get { return (int) _centerImage.maxValue; }
            set { _centerImage.maxValue = value; }
        }

        void CheckIfStarShow()
        {
            float value = _leftSilder.fillAmount + _rightSilder.fillAmount - 1f - 0.01f;
            if (  value <= float.Epsilon)
            {
                _centerImage.gameObject.SetActive(true);
                _centerImage.value = _leftSilder.fillAmount * MaxValue;
            }
            else
            {
                _centerImage.gameObject.SetActive(false);
            }
        }

        public void SetLeftBarValue(int value)
        {
            SetBarValue(_leftSilder, value);
            CheckIfStarShow();
        }

        void SetBarValue(Image i, int value)
        {
            i.fillAmount = (float)value / MaxValue;
        }

        public void SetRightBarValue(int value)
        {
            SetBarValue(_rightSilder, value);
            CheckIfStarShow();
        }
    }
}