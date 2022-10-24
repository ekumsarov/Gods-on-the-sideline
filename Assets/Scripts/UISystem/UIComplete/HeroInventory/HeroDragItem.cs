using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragAndDropSystem;
using UnityEngine.EventSystems;

public class HeroDragItem : UIItem, iDraggableItem
{
    private static HeroDragItem _instance;

    [SerializeField] private UIImage Icon;

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

    public static void Setup(string id, string tag,  DragItem dragged, PointerEventData point, DragCell cell)
    {
        if (_instance == null)
        {
            _instance = GameObject.Instantiate(Resources.Load<HeroDragItem>("Prefabs/UIeX/Complete/HeroDragItem"));
            _instance.GetTransform.SetParent(UIM.Root.transform);
            _instance.MakeRaycast(false);
        }

        _instance._dataID = id;

        HeroData data = IOM.GetHeroData(id);
        _instance.Icon.Image = data.Icon;
        _instance.position = point.position;
        _instance._dragItemTag = tag;
        _instance.Visible = true;

        DragSystem.ActivateItem(_instance, dragged, cell);
    }
}
