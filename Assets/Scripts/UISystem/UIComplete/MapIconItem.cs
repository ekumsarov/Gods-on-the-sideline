using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class MapIconItem : UIItem
{
    public override void Setting()
    {
        base.Setting();

        GM.AddUIObject(this);

        this.MainEvent = "StandartAction";
        this.Activity.PushPack("StandartAction", new List<GameEvents.GameEvent>()
        {
            LoadScene.Create(this.ItemTag)
        });
    }

    public override void Pressed()
    {
        // Temporary part
        // TODO: rework how to load scene
        //
        this.Actioned(this.MainEvent);

        //CrossSceneLoadManager.Instance.LoadScene(this.ItemTag);
    }
}
