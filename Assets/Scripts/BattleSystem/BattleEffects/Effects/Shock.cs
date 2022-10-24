using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

namespace BattleEffects
{
    public class Shock : BattleEffect
    {
        public override void PrepareBattleEffect(CardActionTypes data)
        {
            this.ID = "Shock";
            this._effectType = EffectType.Shock;
            this._buffType = EffectBuffType.Negative;

            this._amount = data.dEffectAmount;
        }

        public override void PrepareBattleEffect(int data)
        {
            this.ID = "Shock";
            this._effectType = EffectType.Shock;

            this._amount = data;
        }

        public override int EffectOnDamage(int damage)
        {
            return Mathf.FloorToInt(damage * 0.5f);
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