using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

namespace BattleEffects
{
    public class Stunn : BattleEffect
    {
        public override void PrepareBattleEffect(CardActionTypes data)
        {
            this.ID = "Stunn";
            this._effectType = EffectType.Stunn;
            this._buffType = EffectBuffType.Negative;

            this._amount = data.dEffectAmount;
        }

        public override void PrepareBattleEffect(int data)
        {
            this.ID = "Stunn";
            this._effectType = EffectType.Shock;

            this._amount = data;
        }

        public override bool CanApplyEffect()
        {
            return base.CanApplyEffect();
        }

        public override void DestroyEffect()
        {
            base.DestroyEffect();
        }
    }

}