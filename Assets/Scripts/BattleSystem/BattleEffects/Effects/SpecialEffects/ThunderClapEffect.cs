using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using BattleEngine;

namespace BattleEffects
{
    public class ThunderClapEffect : BattleEffect
    {
        public override void PrepareBattleEffect(CardActionTypes data)
        {
            this.ID = "ThunderClapEffect";
            this._effectType = EffectType.BySpecID;
            this._buffType = EffectBuffType.Positive;
            this._amount = data.dEffectAmount;

            this._viewVisibility = false;
        }

        public override void PrepareBattleEffect(int amount)
        {
            this.ID = "ThunderClapEffect";
            this._effectType = EffectType.UpSkill;

            this._amount = amount;
        }

        public override bool CanApplyEffect()
        {
            return base.CanApplyEffect();
        }

        protected override void TurnStart()
        {
            List<AIUnit> temp = BattleSystem.GetAliveEnemies();

            ActionSourceData data = ActionSourceData.Create()
                .SetActionID("Effect")
                .SetActionAmount(5)
                .SetSourceID("ThunderClapEffect")
                .SetAdditionalSourceID("Perun")
                .SetSourceType(ActionSourceType.Effect);

            temp[Random.Range(0, temp.Count)].Damage(data, TurnEnd);
        }
    }

}
