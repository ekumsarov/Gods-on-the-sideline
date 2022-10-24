using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInterfaceMenu : MenuEx
{
    public override void Setting()
    {
        base.Setting();
    }

    public override void PressedItem(UIItem data)
    {
        if (data.ItemTag.Equals("Menu"))
            UIM.OpenMenu("HeroesMenu");
        else
            UIM.OpenMenu("MapMenu");
    }
}
