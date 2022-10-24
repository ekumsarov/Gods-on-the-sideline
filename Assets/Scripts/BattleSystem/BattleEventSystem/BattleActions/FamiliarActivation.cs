using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BattleEngine;

namespace BattleActions
{
    public class FamiliarActivation : BattleAction
    {
        private Familiar _parent;
        public static void Create(Familiar parent)
        {
            FamiliarActivation temp = new FamiliarActivation();

            temp._responseType = Lodkod.BattleResponseActions.Empty;
            temp._parent = parent;

            BattleSystem.AddFunctionToQuery(temp);
        }

        public override void StartFunction(Action callback)
        {
            base.StartFunction(callback);

            _parent.ActivateFamiliar();
            End();
        }

        public void End()
        {
            Complete();
        }
    }
}