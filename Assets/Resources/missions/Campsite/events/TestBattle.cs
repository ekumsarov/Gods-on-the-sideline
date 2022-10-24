using UnityEngine;
using SimpleJSON;
using Lodkod;
using System.Collections;
using System.Collections.Generic;
using System;
using GameEvents;

namespace GameEventsCampsite
{
    public class TestBattle : GameEvent
    {

        Subscriber subscriber;

        public override void Init()
        {
            this.ID = "TestBattle";

            Simple = false;

            subscriber = Subscriber.Create(this);

            Object.MainEvent = "StartBattle";
            Object.Group.RemoveAllHeroes();

            Object.Group.RemoveAllHeroes();
            Object.Group.AddNewHero("Imp");
            Object.Group.AddNewHero("Imp");

            Actions act = Actions.Get("Context");
            act.ID = "StartBattle";
            Object.AddAction(act);

            act.list.Add(ActionButtonInfo.Create("Start battle with 2 Imps").SetCallData("ImpBattlePack").SetType(ActionType.Pack));
            act.list.Add(ActionButtonInfo.Create("Start battle with 2 Imps and 1 Habernica").SetCallData("HrabrImpsPack").SetType(ActionType.Pack));
            act.list.Add(ActionButtonInfo.Create("Start battle with 2 Imps and 1 Ghoul").SetCallData("GhoulImpsPack").SetType(ActionType.Pack));
            act.list.Add(ActionButtonInfo.Create("Start battle with 3 Ghoul").SetCallData("GhoulsPack").SetType(ActionType.Pack));
            act.list.Add(ActionButtonInfo.Create("Boss fight").SetCallData("BossFight").SetType(ActionType.Pack));
            act.list.Add(ActionButtonInfo.Create("Close").SetType(ActionType.Close));

            this.Object.Activity.PushPack("ImpBattlePack", new List<GameEvent>()
            {
                BattleEvent.Create("Player", "Setup", ResultID.Create().SetSuccessCallback(PlayerWon).SetFailID("LooseToImp"), enemyStack: new List<string>() { "Imp", "Imp", "Imp" } )
            });

            this.Object.Activity.PushPack("HrabrImpsPack", new List<GameEvent>()
            {
                BattleEvent.Create("Player", "Setup", ResultID.Create().SetSuccessCallback(PlayerWon).SetFailID("LooseToHabrImp"), enemyStack: new List<string>() { "Habernica", "Imp", "Imp" } )
            });

            this.Object.Activity.PushPack("GhoulsPack", new List<GameEvent>()
            {
                BattleEvent.Create("Player", "Setup", ResultID.Create().SetSuccessCallback(PlayerWon).SetFailID("LooseToHabrImp"), enemyStack: new List<string>() { "Ghoul", "Ghoul", "Ghoul" } )
            });

            this.Object.Activity.PushPack("GhoulImpsPack", new List<GameEvent>()
            {
                BattleEvent.Create("Player", "Setup", ResultID.Create().SetSuccessCallback(PlayerWon).SetFailID("LooseToHabrImp"), enemyStack: new List<string>() { "Ghoul", "Imp", "Imp" } )
            });

            this.Object.Activity.PushPack("BossFight", new List<GameEvent>()
            {
                BattleEvent.Create("Player", "Setup", ResultID.Create().SetSuccessCallback(PlayerWon).SetFailID("LooseToHabrImp"), enemyStack: new List<string>() { "GhoulQueen" } )
            });


            /*Object.Activity.PushPack("StartBattle", new List<GameEvent>()
            {
                
                BattleEvent.Create("Player", Object.ID, ResultID.Create().SetFailCallback(PlayerLoose).SetSuccessCallback(PlayerWon), DrawDelegate:Draw)
            });*/

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

            this.Object.Actioned("StartBattle");

            End();
        }

        public void ImpDead()
        {

        }

        public void WolfDead()
        {
            QS.CompleteQuest("WolfQuest");
            IconObject temp = Object as IconObject;
            if (temp != null)
            {
                temp.Lock = true;
                temp.Visible = false;
                temp.RemoveIcon();
            }
            SM.Stats["Food"].Count += 20;
        }

        public void PlayerWon()
        {
        }

        public void Draw()
        {
            UIM.ShowTooltip(Object, TooltipFit.Auto, TooltipTimeMode.Tootip, TooltipFillMode.Type, TooltipObject.Game, "AllAnimalsGone", time: 1.5f);
        }
    }
}