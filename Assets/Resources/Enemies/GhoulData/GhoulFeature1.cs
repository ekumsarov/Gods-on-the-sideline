using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleEffects;
using Lodkod;

namespace BattleEngine
{
    public class GhoulFeature1 : UnitFeature
    {
        protected override void Setup()
        {
            base.Setup();

            BattleResponse.AddListener(Lodkod.BattleResponseActions.GetDamage, Notified);
        }

        public void Notified(ActionSourceData data)
        {
            if (data.SourceID.Equals(_parent.Name))
            {
                _parent.Heal(data.ActionAmount, "GhoulFeature", null);
            }
        }
    }
}
