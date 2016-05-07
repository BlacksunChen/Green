using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Utilities;
using UnityEngine.UI;


namespace Green
{
    //[ExecuteInEditMode]
    public class LevelSelectMenu : MonoBehaviour
    {
        /// <summary>
        /// Current Displayed Item Index
        /// </summary>
        private int _currentItemIndex = 0;

        /// <summary>
        /// True means LevelMenu2D is moving/animating, false means it is stopped. 
        /// While animating/moving, LevelMenu2D cannot be further navigated or interacted.
        /// </summary>
        private bool _isMoving = false;

        /// <summary>
        /// The List of Items in the LevelMenu. This is list of GameObjects.
        /// You can put nested objects, 2D, or 3D objects without any problems.
        /// </summary>
        public List<LevelSelectItem> itemsList = new List<LevelSelectItem>();

        public List<RectTransform> LevelItemsPositionSample;
        /// <summary>
        /// Initial Item Index to be load at start of LevelMenu2D.
        /// </summary>
        public int initialItemNumber = 0;

        /// <summary>
        /// Scale of center item to highlight it.
        /// 1.0 means no difference This is represented in percents. 1.2 means item will be increased by 20% in scale. 
        /// </summary>
        public float scaleOfCenterItem = 0.5f;

        /// <summary>
        /// Original Scale of center item for scaling purposes.
        /// 1.0 means no difference. 
        /// </summary>
        private Vector3 originalScaleOfCenterItem = Vector3.zero;

        /// <summary>
        /// The First Item of the LevelMenu2D.
        /// This is set from items list automatically.
        /// </summary>
        private LevelSelectItem firstItem;

        /// <summary>
        /// Whether Next/Back buttons to be shown in menu or not.
        /// </summary>
        public bool showNextBackButtons = true;

        /// <summary>
        /// Next Button Game Object
        /// </summary>
        public GameObject nextButtonObject;

        /// <summary>
        /// Back Button Game Object
        /// </summary>
        public GameObject backButtonObject;

        /// <summary>
        /// Should Increase the Order
        /// </summary>
        bool shouldIncreaseOrder = true;

        /// <summary>
        /// Whether to put items automatically based on Orientatation.
        /// This would be true in case of Horizontal or Vertical Orientation.
        /// </summary>
        public bool autoOffset = true;

        /// <summary>
        /// If itemOffset of the LevelMenu2D in case of autoOffset is false.
        /// </summary>
        public Vector2 itemOffset = new Vector3(0f, 0f);

        /// <summary>
        /// Space between items for Horizontal or Vertical Orientations. 
        /// </summary>
        public float spacingBetweenItems = 0f;

        /// <summary>
        /// EaseType of the animation to navigate items.
        /// </summary>
        public iTween.EaseType easeType;

        /// <summary>
        /// Animation Time for Navigation.
        /// </summary>
        float animationTime = 1f;

        /// <summary>
        /// False means only one item is current at a time and true otherwise.
        /// </summary>
        public bool isBounded = false;

        /// <summary>
        /// Minimum Bound Index from the item list.
        /// </summary>
        public int minimumBoundIndex = 0;

        /// <summary>
        /// Maximum Bound Index from the item list.
        /// </summary>
        public int maximumBoundIndex = 0;

        /// <summary>
        /// A true/false indicator telling whether menu is being created or not.
        /// </summary>
        private bool isMenuCreating = false;

        /// <summary>
        /// The delegate when item is clicked.
        /// </summary>
        public delegate void OnItemClickedDelegate(int itemIndex, GameObject itemObject);

        /// <summary>
        /// The Click Event to listen for item clicks.
        /// </summary>
        public event OnItemClickedDelegate OnItemClicked;

        /// <summary>
        /// Current Item's Index
        /// </summary>
        public int CurrentItem
        {
            get { return _currentItemIndex; }
        }

        public Animation ItemPosAnim;

        /// <summary>
        /// Returns whether LevelMenu2D is moving or stopped.
        /// </summary>
        public bool isMoving
        {
            get { return _isMoving; }
        }

        //每一个item的位置
        public Dictionary<int, Vector2> ItemPosition;
        // Use this for initialization
        void Start()
        {
            itemsList = GetComponentsInChildren<LevelSelectItem>().ToList();
            itemsList.ForEach((LevelSelectItem item) => item.Menu = this);
            var obj = GameObject.Find("LevelItemsPositonSample");
            LevelItemsPositionSample = obj.GetComponentsInChildren<RectTransform>().ToList();
            ItemPosition = new Dictionary<int, Vector2>();
            collectPosition();
            firstItem = itemsList[0];
            isMenuCreating = true;
            createMenu();
           // gotoItem(initialItemNumber);
            isMenuCreating = false;
            _chapter = GetComponent<LevelSelectChapter>();
            SetChapterInfoToItem();
        }

        void collectPosition()
        {
            foreach (RectTransform t in LevelItemsPositionSample)
            {
                int num;
                if (int.TryParse(t.gameObject.name, out num))
                {
                    ItemPosition.Add(int.Parse(t.gameObject.name), t.anchoredPosition);
                }
                    
            }
        }

        //left-right level_1->10
        void SortMenu()
        {
            itemsList.Sort((item, selectItem) => {
                var thisNum = int.Parse(item.gameObject.name);
                var otherNum = int.Parse(selectItem.gameObject.name);
                if (thisNum > otherNum)
                {
                    return 1;
                }
                return -1;
            });
        }

        void InitItemsPosition()
        {
            for (int i = 0; i < itemsList.Count; ++i)
            {
                itemsList[i].PositionInItems = i+6;
            }
        }
        // Update is called once per frame
        void Update()
        {
            //if (autoUpdateAtRuntime)
             //   createMenu();
        }

        public const float AnimationInterval = 0.5f;        /// <summary>
        /// Recreates the LevelMenu2D from scratch.
        /// </summary>
        public void recreateMenu()
        {
            initialItemNumber = CurrentItem;
            firstItem = itemsList[CurrentItem];
            _currentItemIndex = 0;
            createMenu();
            gotoItem(initialItemNumber);
        }

        /// <summary>
        /// Creates the LevelMenu2D from scratch.
        /// </summary>
        public void createMenu()
        {
            // If autoOffset is true, then set itemOffset to zero.
            if (autoOffset)
                itemOffset = Vector3.zero;

            if (itemsList.Count > 0)
            {
                SortMenu();

                InitItemsPosition();
               // itemsList[0].transform.position = firstItem.transform.position;

                foreach (var item in itemsList)
                {
                    var num = int.Parse(item.gameObject.name);
                    item.GetComponent<RectTransform>().anchoredPosition = ItemPosition[item.PositionInItems];
                }
            }
            //doScaleTheCenterItem(true);
        }

        /// <summary>
        /// Navigates to the item object passed in parameter.
        /// </summary>
        /// <param name="itemObject">A GameObject of any item from the item list.</param>
        public void gotoItem(LevelSelectItem itemObject)
        {
            if (indexOf(itemObject) < 0)
                return;

            gotoItem(indexOf(itemObject));
        }

        /// <summary>
        /// Navigates to the item index passed in parameter.
        /// </summary>
        /// <param name="itemNum">Index of of any item from the item list.</param>
        public void gotoItem(int itemNum)
        {

            if (itemNum < 0 || itemNum >= itemsList.Count || itemsList.Count <= 0 || CurrentItem >= itemsList.Count ||
                CurrentItem < 0)
                return;
            if (initialItemNumber == 0 && isMenuCreating)
            {
                doScaleTheCenterItem(true);
                return;
            }
            if (_isMoving) return;
            doScaleTheCenterItem(false);
            var offset = itemNum - _currentItemIndex;
            _isMoving = true;
            foreach (var item in itemsList)
            {
                MoveItemToTargetPosition(item, offset);
            }
            _currentItemIndex = itemNum;
            SortOrderOfItems();
        }

        void MoveItemToNextPosition(LevelSelectItem item)
        {
            var tr = item.GetComponent<RectTransform>();
            tr.DOAnchorPos(ItemPosition[item.PositionInItems], animationTime, false)
                .OnComplete(moveComplete).SetEase(Ease.Linear);
        }

        void MoveItemToTargetPosition(LevelSelectItem item, int offset)
        {
            var seq = DOTween.Sequence();
            float timeDetla = animationTime / (float)Mathf.Abs(offset);
            for (int i = 0; i < Mathf.Abs(offset); ++i)
            {
                if (offset > 0)
                    item.PositionInItems--;
                else
                    item.PositionInItems++;
                var tr = item.GetComponent<RectTransform>();
                seq.Append(tr.DOAnchorPos(ItemPosition[item.PositionInItems], timeDetla, true));
            }
            seq.OnComplete(moveComplete);
        }
        LevelSelectItem GetCenterItem()
        {
            foreach (var i in itemsList)
            {
                if (i.PositionInItems == 6)
                {
                    return i;
                }
            }
            return null;
        }

        public void doScaleTheCenterItem(bool isUp)
        {
            if (scaleOfCenterItem <= 0f || (isMenuCreating && initialItemNumber != 0))
                return;
            var center = GetCenterItem();
            if (isUp)
            {
                center.DoScaleLarge(1.5f);
            }
            else
            {
                center.DoScaleOrigin();
            }
        }

        /// <summary>
        /// Returns index of the GameObject of item passed in parameter
        /// </summary>
        /// <param name="itemObject">GameObject of any item from the item list.</param>
        public int indexOf(LevelSelectItem itemObject)
        {
            return itemsList.IndexOf(itemObject);
        }

        /// <summary>
        /// Navigates to Next Item
        /// </summary>
        public void gotoNextItem()
        {
            doScaleTheCenterItem(false);
            if (CurrentItem == maximumBoundIndex && isBounded) return;
            if (_currentItemIndex >= itemsList.Count - 1 || _isMoving)
                return;

            foreach (var item in  itemsList)
            {
                item.PositionInItems--;
                MoveItemToNextPosition(item);
            }
            _currentItemIndex++;
          //  doScaleTheCenterItem(true);
          //  ShowLevelName();
        }


        /// <summary>
        /// Navigates to Back Item
        /// </summary>
        public void gotoBackItem()
        {
            doScaleTheCenterItem(false);
            if (CurrentItem == minimumBoundIndex && isBounded) return;

            if (_currentItemIndex <= 0 || _isMoving)
                return;

            foreach (var item in itemsList)
            {
                item.PositionInItems++;
                MoveItemToNextPosition(item);
            }
            _currentItemIndex--;
            //doScaleTheCenterItem(true);
            //ShowLevelName();
        }


        /// <summary>
        /// Dispatches Any Item's Click. This is used to auto-click on items.
        /// </summary>
        public void dispatchCurrentItemClick(int itemIndex, GameObject itemObject)
        {
            if (OnItemClicked != null)
                OnItemClicked(itemIndex, itemObject);
        }

        void moveComplete()
        {
            _isMoving = false;
            //itemsList[CurrentItem].GetImage().sortingOrder = itemsList.Count - 1;
            doScaleTheCenterItem(true);
        }

        public void SortOrderOfItems()
        {
            /*
            for (int i = CurrentItem - 1; i >= 0; i--)
            {
                itemsList[i].GetComponent<RectTransform>().SetSiblingIndex(i - CurrentItem);
                //itemsList[i].GetComponent<ClickTouchScript>().enabled = true;
            }
            for (int i = itemsList.Count - 1; i > CurrentItem; i--)
            {
                itemsList[i].GetComponent<RectTransform>().SetSiblingIndex(CurrentItem - i);
                //itemsList[i].GetComponent<ClickTouchScript>().enabled = true;
            }
            itemsList[CurrentItem].GetComponent<RectTransform>().SetSiblingIndex(0);
            //itemsList[CurrentItem].GetComponent<ClickTouchScript>().enabled = false;
            */
            itemsList[CurrentItem].GetComponent<RectTransform>().SetAsFirstSibling();
            for (int i = CurrentItem - 1; i >= 0; i--)
            {
                itemsList[i].GetComponent<RectTransform>().SetAsLastSibling();
            }
            for (int i = itemsList.Count - 1; i > CurrentItem; i--)
            {
                itemsList[i].GetComponent<RectTransform>().SetAsLastSibling();
            }
            
        }

        LevelSelectChapter _chapter;

        void SetChapterInfoToItem()
        {
            foreach (var item in itemsList)
            {
                item.ChapterNum = _chapter.ChapterNumber;
            }
        }
    }
}