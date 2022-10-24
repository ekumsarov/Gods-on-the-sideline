using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using BattleEngine;

namespace BattleEffects
{
    public class Ice : BattleEffect
    {
        public override void PrepareBattleEffect(CardActionTypes data)
        {
            this.ID = "Ice";
            this._effectType = EffectType.Ice;
            this._buffType = EffectBuffType.Negative;

            this._amount = data.dEffectAmount;
        }

        public override void PrepareBattleEffect(int data)
        {
            this.ID = "Ice";
            this._effectType = EffectType.Ice;

            this._amount = data;
        }

        public override bool CanApplyEffect()
        {
            return base.CanApplyEffect();
        }

        protected override void TurnStart()
        {
            ActionSourceData data = ActionSourceData.Create()
                .SetSourceID(this.Object.Name)
                .SetAdditionalSourceID(this.ID)
                .SetSourceType(ActionSourceType.Effect)
                .SetActionAmount(this._amount);

            this.Object.Damage(data, TurnEnd);
        }

        public override void DestroyEffect()
        {
            base.DestroyEffect();
        }
    }

}