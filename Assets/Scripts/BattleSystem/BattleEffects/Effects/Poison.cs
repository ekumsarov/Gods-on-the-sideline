using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

namespace BattleEffects
{
    public class Poison : BattleEffect
    {
        public override void PrepareBattleEffect(CardActionTypes data)
        {
            this.ID = "Poison";
            this._effectType = EffectType.Poison;
            this._buffType = EffectBuffType.Negative;

            this._amount = data.dEffectAmount;
        }

        public override void PrepareBattleEffect(int data)
        {
            this.ID = "Poison";
            this._effectType = EffectType.Poison;

            this._amount = data;
        }

        public override bool CanApplyEffect()
        {
            return base.CanApplyEffect();
        }

        public override void SumEffects(BattleEffect effect)
        {
            base.SumEffects(effect);

            if (_amount > 5)
                _amount = 5;
        }


        public override int EffectOnDamage(int damage)
        {
            return Mathf.FloorToInt(damage*(1 - 0.1f*this._amount));
        }

        public override void DestroyEffect()
        {
            base.DestroyEffect();
        }
    }

}