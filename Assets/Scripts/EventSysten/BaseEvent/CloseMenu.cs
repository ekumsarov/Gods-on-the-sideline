﻿using UnityEngine;
using SimpleJSON;
using Lodkod;
using System.Collections;
using System.Collections.Generic;
using System;

namespace GameEvents
{
    public class CloseMenu : GameEvent
    {
        string MenuID;

        public override void PrepareEvent(JSONNode node)
        {
            this.ID = "CloseMenu";

            MenuID = null;
            if (node["MenuID"] != null)
                MenuID = node["MenuID"].Value;
        }

        public override bool CanActive()
        {
            if (MenuID == null)
            {
                Debug.LogError("Not set the menu to open it");
                return false;
            }

            return true;
        }

        public override void Start()
        {
            UIM.CloseMenu(MenuID);

            End();
        }

        #region static
        public static CloseMenu Create(string menuID)
        {
            CloseMenu temp = new CloseMenu();
            temp.ID = "CloseMenu";

            temp.MenuID = menuID;

            return temp;
        }
        #endregion
    }
}

































































































































































































































































































































































