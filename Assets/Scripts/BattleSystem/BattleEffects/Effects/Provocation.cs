using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

namespace BattleEffects
{
    public class Provocation : BattleEffect
    {
        public override void PrepareBattleEffect(CardActionTypes data)
        {
            this.ID = "Provocation";
            this._effectType = EffectType.Provocation;
            this._buffType = EffectBuffType.Positive;

            this._amount = data.dEffectAmount;
        }

        public override void PrepareBattleEffect(int data)
        {
            this.ID = "Provocation";
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

        public override void DestroyEffect()
        {
            base.DestroyEffect();
        }
    }

}