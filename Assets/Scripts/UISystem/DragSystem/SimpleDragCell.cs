using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragAndDropSystem;
using System;

public class SimpleDragCell : DragCell
{
    private Action<string> _itemPlaced;
    public Action<string> ItemPlaced
    {
        set
        {
            _itemPlaced += value;
        }
    }

    public override void Setting()
    {
        base.Setting();
    }

    private string _itemID;
    public override void PlaceItem(string item)
    {
        _itemID = item;
        _itemPlaced?.Invoke(item);
    }

    public override string GetItemID => _itemID;
}
