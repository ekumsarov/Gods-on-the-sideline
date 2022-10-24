using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleEffects;
using Lodkod;

namespace BattleEngine
{
    public class HabernicaFeature1 : UnitFeature
    {
        protected override void Setup()
        {
            base.Setup();

            BattleResponse.AddListener(Lodkod.BattleResponseActions.AppliedEffect, Notified);
        }

        public void Notified(ActionSourceData data)
        {
            if(data.SourceID.Equals(_parent.Name) && data.ActionID.Equals(EffectType.Blindless.ToString()))
            {
                BattleSystem.AddUnit(Unit.Make("HabernicaCopy"), false);
            }
        }
    }
}