using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

namespace BattleEffects
{
    public class Blindless : BattleEffect
    {
        public override void PrepareBattleEffect(CardActionTypes data)
        {
            this.ID = "Blindless";
            this._effectType = EffectType.Blindless;
            this._buffType = EffectBuffType.Negative;

            this.SetCooldown = data.dEffectAmount;
        }

        public override void PrepareBattleEffect(int amount)
        {
            this.ID = "Blindless";
            this._effectType = EffectType.Blindless;

            this.SetCooldown = amount;
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
