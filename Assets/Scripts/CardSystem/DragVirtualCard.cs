using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DragAndDropSystem;


public class DragVirtualCard : UIItem, iDraggableItem
{
    static DragVirtualCard _instance = null;
    public static void Setup(string id, string tag, CardData data, DragItem dragged, PointerEventData point, DragCell cell)
    {
        if(_instance == null)
        {
            _instance = GameObject.Instantiate(Resources.Load<DragVirtualCard>("Prefabs/UIeX/Complete/DragVirtualCard"));
            _instance.GetTransform.SetParent(UIM.Root.transform);
            _instance.MakeRaycast(false);
        }

        _instance._dataID = id;

        _instance.Name.Text = data.ID;
        _instance.CardsActionText.Text(CardActionFactory.GetCardDescription(data));
        _instance.CardsActionText.ShowComplete();
        _instance.position = point.position;
        _instance._dragItemTag = tag;
        _instance.Visible = true;

        DragSystem.ActivateItem(_instance, dragged, cell);
    }

    [SerializeField] private UIImage Icon;
    [SerializeField] private SimpleText Name;
    [SerializeField] private IconText CardsActionText;

    private string _cardID;

    public void ReadCard(string data)
    {
        CardData cardInfo = IOM.GetCard(data);
        if (cardInfo == null)
            return;

        Name.Text = LocalizationManager.Get(cardInfo.ID);
        _cardID = cardInfo.ID;
        _dataID = _cardID;

        CardsActionText.Text(CardActionFactory.GetCardDescription(cardInfo));
    }

    private string _dataID;
    public string DataID
    {
        get { return _dataID; }
    }

    public bool DestroyItem
    {
        set
        {
            this.Visible = value;
        }
    }
    public Vector3 SetPosition
    {
        set
        {
            this.position = value;
        }
    }

    private string _dragItemTag;
    public string DragItemTag
    {
        get { return _dragItemTag; }
    }
}