using UnityEngine;
using System.Collections;

namespace Green
{
    public class ClickTouchScript : MonoBehaviour
    {

        private Vector2 downPos, upPos;

        public LevelSelectItem LevelItem;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;



                if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.name == this.gameObject.name)
                {
                    Debug.Log(this.gameObject.name + " touched");
                    GameObject touchObject = hit.transform.gameObject;

                    // Detect which of the Menu's object is this
                    //LevelMenu2D.Instance.autoActivateMenu(this.gameObject);

                    if (LevelMenu2D.Instance.isMoving)
                        return;

                    if (LevelMenu2D.Instance.isBounded == false)
                    {
                        if (LevelMenu2D.Instance.showNextBackButtons &&
                            touchObject.name.Equals(LevelMenu2D.Instance.nextButtonObject.name))
                        {
                            LevelMenu2D.Instance.gotoNextItem();
                        }
                        else if (LevelMenu2D.Instance.showNextBackButtons &&
                                 touchObject.name.Equals(LevelMenu2D.Instance.backButtonObject.name))
                        {
                            LevelMenu2D.Instance.gotoBackItem();
                        }
                        else
                        {
                            if (LevelMenu2D.Instance.CurrentItem == LevelMenu2D.Instance.indexOf(LevelItem))
                            {
                                LevelMenu2D.Instance.dispatchCurrentItemClick(LevelMenu2D.Instance.CurrentItem,
                                    touchObject);
                            }
                            else LevelMenu2D.Instance.gotoItem(LevelItem);
                        }
                    }
                    else
                    {
                        LevelMenu2D.Instance.dispatchCurrentItemClick(LevelMenu2D.Instance.indexOf(LevelItem),
                            touchObject);
                    }
                }
            }

        }

        void OnMouseDown()
        {
            //Debug.Log("MouseDown");
            downPos = (Vector2) Input.mousePosition;
        }

        void OnMouseUp()
        {
            upPos = (Vector2) Input.mousePosition;

            // Detect which of the Menu's object is this
            //LevelMenu2D.Instance.autoActivateMenu(this.gameObject);

            if (LevelMenu2D.Instance.isMoving)
                return;

            //Debug.Log("MouseUp");
            if (LevelMenu2D.Instance.isBounded == false)
            {
                if (LevelMenu2D.Instance.showNextBackButtons &&
                    this.name.Equals(LevelMenu2D.Instance.nextButtonObject.name))
                {
                    LevelMenu2D.Instance.gotoNextItem();
                    //LevelMenu2D.Instance.gotoItem(LevelMenu2D.Instance.CurrentItem+1);
                }
                else if (LevelMenu2D.Instance.showNextBackButtons &&
                         this.name.Equals(LevelMenu2D.Instance.backButtonObject.name))
                {
                    LevelMenu2D.Instance.gotoBackItem();
                    //LevelMenu2D.Instance.gotoItem(LevelMenu2D.Instance.CurrentItem-1);
                }
                else
                {
                    if (LevelMenu2D.Instance.CurrentItem == LevelMenu2D.Instance.indexOf(LevelItem) &&
                        upPos == downPos)
                    {
                        LevelMenu2D.Instance.dispatchCurrentItemClick(LevelMenu2D.Instance.CurrentItem,
                            this.gameObject);
                    }
                    //LevelMenu2D.Instance.gotoItem(LevelMenu2D.Instance.itemsList.IndexOf(this.gameObject));
                    else LevelMenu2D.Instance.gotoItem(LevelItem);
                }
            }
            else
            {
                if (upPos == downPos)
                {
                    LevelMenu2D.Instance.dispatchCurrentItemClick(LevelMenu2D.Instance.indexOf(LevelItem),
                        this.gameObject);
                }
            }
        }
    }
}