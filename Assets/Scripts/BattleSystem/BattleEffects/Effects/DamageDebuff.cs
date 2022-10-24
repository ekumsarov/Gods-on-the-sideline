using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

namespace BattleEffects
{
    public class DamageDebuff : BattleEffect
    {
        public override void PrepareBattleEffect(CardActionTypes data)
        {
            this.ID = "DamageDebuff";
            this._effectType = EffectType.DamageBuff;
            this._buffType = EffectBuffType.Positive;

            this.SetCooldown = data.dEffectroundCount;
            this._amount = data.dEffectAmount;
        }

        public override void PrepareBattleEffect(int amount)
        {
            this.ID = "DamageDebuff";
            this._effectType = EffectType.DamageDebuff;

            this._amount = amount;
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

