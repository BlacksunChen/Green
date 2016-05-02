using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Green
{
    public class ScoreBar : MonoBehaviour
    {
        public Slider _centerImage;

        public Slider _leftSilder;
        public Slider _rightSilder;

       
        void Awake()
        {
            foreach (Transform t in transform)
            {
                switch (t.name)
                {
                    case "left_bar":
                        _leftSilder = t.GetComponent<Slider>();
                        break;
                    case "right_bar":
                        _rightSilder = t.GetComponent<Slider>();
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
            _leftSilder.wholeNumbers = true;
            _rightSilder.wholeNumbers = true;
            _leftSilder.minValue = 0;
            _rightSilder.minValue = 0;
            _centerImage.gameObject.SetActive(false);
        }

        public int MaxValue
        {
            get
            {
                Debug.Assert((int)_leftSilder.maxValue == (int)_rightSilder.maxValue);
                return (int)_leftSilder.maxValue;               
            }
            set
            {
                _leftSilder.maxValue = value;
                _rightSilder.maxValue = value;
                _centerImage.maxValue = value;
            }
        }

        void CheckIfStarShow()
        {
            if ((int) _leftSilder.value + (int) _rightSilder.value == MaxValue)
            {
                _centerImage.gameObject.SetActive(true);
                _centerImage.value = _leftSilder.value;
            }
            else
            {
                _centerImage.gameObject.SetActive(false);
            }
        }
        public void SetLeftBarValue(int value)
        {
            _leftSilder.value = value;
            CheckIfStarShow();
        }

        public void SetRightBarValue(int value)
        {
            _rightSilder.value = value;
            CheckIfStarShow();
        }
    }
}