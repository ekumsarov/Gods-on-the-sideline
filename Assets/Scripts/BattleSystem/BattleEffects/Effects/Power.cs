using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using BattleEngine;

namespace BattleEffects
{
    public class Power : BattleEffect
    {
        public override void PrepareBattleEffect(CardActionTypes data)
        {
            this.ID = "Power";
            this._effectType = EffectType.Power;
            this._buffType = EffectBuffType.Positive;

            this._amount = data.dEffectAmount;
        }

        public override void PrepareBattleEffect(int data)
        {
            this.ID = "Power";
            this._effectType = EffectType.Power;

            this._amount = data;
        }

        public override bool CanApplyEffect()
        {
            return base.CanApplyEffect();
        }

        protected override void TurnStart()
        {
            ActionSourceData data = ActionSourceData.Create()
                .SetActionID("Power")
                .SetActionAmount(this._amount)
                .SetSourceID(this.ID)
                .SetSourceType(ActionSourceType.Effect);

            this.Object.Damage(data, TurnEnd);
        }

        public override void DestroyEffect()
        {
            base.DestroyEffect();
        }
    }

}