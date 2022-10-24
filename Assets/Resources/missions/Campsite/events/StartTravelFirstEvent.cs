using UnityEngine;
using SimpleJSON;
using Lodkod;
using System.Collections;
using System.Collections.Generic;
using System;
using GameEvents;

namespace GameEventsCampsite
{
    public class StartTravelFirstEvent : GameEvent
    {

        public override void Init()
        {
            this.ID = "StartTravelFirstEvent";

            Simple = false;

            Object.MainEvent = "UlebReady";

            Actions act = Actions.Get("Context");
            act.ID = "UlebReady";
            Object.AddAction(act);
            act.list.Add(ActionButtonInfo.Create("PerunWhere").SetCallData("GoPLoc"));

            act = Actions.Get("Context");
            act.ID = "GoPLoc";
            Object.AddAction(act);
            act.list.Add(ActionButtonInfo.Create("WaitPati").SetCallData("MapTutorial").SetType(ActionType.Pack));

            this.Object.Activity.PushPack("MapTutorial", new List<GameEvent>()
            {
                OpenMenu.Create("MapMenu"),
                CallAction.Create("ButWait")
            });

            act = Actions.Get("Context");
            act.ID = "ButWait";
            Object.AddAction(act);
            act.list.Add(ActionButtonInfo.Create("WillGoIdol").SetCallData("OpenIdol").SetType(ActionType.Pack));

            this.Object.Activity.PushPack("OpenIdol", new List<GameEvent>()
            {
                CloseMenu.Create("MapMenu"),
                CreateObject.Create("MainIdol"),
                ActivateObject.Create("TravelStart", false)
            });

            initialized = true;
        }

        public void Play()
        {
            if (!initialized)
            {
                initialized = true;
                End();
                return;
            }
        }
    }
}
