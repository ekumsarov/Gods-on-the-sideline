using UnityEngine;
using SimpleJSON;
using Lodkod;
using System.Collections;
using System.Collections.Generic;
using System;

namespace GameEvents
{
    public class OpenMenu : GameEvent
    {
        string MenuID;

        public override void PrepareEvent(JSONNode node)
        {
            this.ID = "OpenMenu";

            MenuID = null;
            if (node["MenuID"] != null)
                MenuID = node["MenuID"].Value;
        }

        public override bool CanActive()
        {
            if(MenuID == null)
            {
                Debug.LogError("Not set the menu to open it");
                return false;
            }

            return true;
        }

        public override void Start()
        {
            UIM.OpenMenu(MenuID);

            End();
        }

        #region static
        public static OpenMenu Create(string menuID)
        {
            OpenMenu temp = new OpenMenu();
            temp.ID = "OpenMenu";

            temp.MenuID = menuID;

            return temp;
        }
        #endregion
    }
}