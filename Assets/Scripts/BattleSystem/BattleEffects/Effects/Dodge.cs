using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

namespace BattleEffects
{
    public class Dodge : BattleEffect
    {
        public override void PrepareBattleEffect(CardActionTypes data)
        {
            this.ID = "Dodge";
            this._effectType = EffectType.Dodge;
            this._buffType = EffectBuffType.Positive;

            this._amount = 3;
        }

        public override void PrepareBattleEffect(int amount)
        {
            this.ID = "Dodge";
            this._effectType = EffectType.Dodge;

            this._amount = amount;
        }

        public override bool CanApplyEffect()
        {
            return base.CanApplyEffect();
        }

        public override void SumEffects(BattleEffect effect)
        {
            this._amount = 3;
        }



        protected override void TurnEnd()
        {
            if (this.EffectDestroyed)
            {
                Complete();
                return;
            }

            this._amount -= 1;
            if (this._amount <= 0)
            {
                this.DestroyEffect();
            }
            Complete();
        }

        public override void DestroyEffect()
        {
            base.DestroyEffect();
        }
    }

}