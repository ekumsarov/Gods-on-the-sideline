using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

namespace BattleEffects
{
    public class Defense : BattleEffect
    {
        public override void PrepareBattleEffect(CardActionTypes data)
        {
            this.ID = "Defense";
            this._effectType = EffectType.Defense;
            this._buffType = EffectBuffType.Positive;

            this._amount = data.dEffectAmount;
        }

        public override void PrepareBattleEffect(int amount)
        {
            this.ID = "Defense";
            this._effectType = EffectType.Defense;

            this._amount = amount;
        }

        public override bool CanApplyEffect()
        {
            return base.CanApplyEffect();
        }

        protected override void TurnEnd()
        {
            if (this.EffectDestroyed)
            {
                Complete();
                return;
            }
                
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