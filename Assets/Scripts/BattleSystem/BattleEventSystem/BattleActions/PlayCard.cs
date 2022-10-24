using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BattleEngine;

namespace BattleActions
{
    public class PlayCard : BattleAction
    {
        private Card _parent;
        bool checkToPlay = true;
        bool canStart = false;
        public static void Create(Card parent)
        {
            PlayCard temp = new PlayCard();

            temp._responseType = Lodkod.BattleResponseActions.Empty;
            temp._parent = parent;
            temp.checkToPlay = true;
            temp.canStart = false;

            BattleSystem.AddFunctionToQuery(temp);
        }

        public override void StartFunction(Action callback)
        {
            base.StartFunction(callback);

            _parent.PlayCard();
            End();
        }

        public void End()
        {
            BattleSystem.AddFunctionToQuery(CallbackAction.Get(BattleSystem.CompletePlayerTurn));
            Complete();
        }
    }
}