using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using BattleEngine;

namespace BattleEffects
{
    public class Bleeding : BattleEffect
    {
        public override void PrepareBattleEffect(CardActionTypes data)
        {
            this.ID = "Bleeding";
            this._effectType = EffectType.Bleeding;
            this._buffType = EffectBuffType.Negative;

            this._amount = data.dEffectAmount;
        }

        public override void PrepareBattleEffect(int amount)
        {
            this.ID = "Bleeding";
            this._effectType = EffectType.Bleeding;

            this._amount = amount;
        }

        public override bool CanApplyEffect()
        {
            return base.CanApplyEffect();
        }

        protected override void TurnStart()
        {
            ActionSourceData data = ActionSourceData.Create()
                .SetActionID("Bleeding")
                .SetActionAmount(this._amount)
                .SetSourceID(this.ID)
                .SetSourceType(ActionSourceType.Effect);

            this.Object.Damage(data, TurnEnd);
        }

        protected override void TurnEnd()
        {
            if(this.EffectDestroyed)
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

