using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Lodkod;
using System;
using System.Linq;
using GameEvents;
using SimpleJSON;

public class MapMenu : MenuEx
{
    List<MapIconItem> _mapIcons;
    public override void Setting()
    {
        base.Setting();

        this._mapIcons = new List<MapIconItem>();

        foreach(var item in this._allItems)
        {
            MapIconItem icon = item.Value as MapIconItem;
            if(icon != null)
            {
                _mapIcons.Add(icon);

                if (IOM.OpenedMapIcons.Any(iconID => iconID.Equals(icon.ID)))
                    icon.Visible = true;
                else
                    icon.Visible = false;
            }
        }
    }

    public override void PressedItem(UIItem data)
    {
        if(data.ItemTag.Equals(GM.missionPath))
        {
            this.Close();
            return;
        }

        CrossSceneLoadManager.Instance.LoadScene(data.ItemTag);
    }
}