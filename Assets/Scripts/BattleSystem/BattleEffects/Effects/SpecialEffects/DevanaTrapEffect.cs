using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using BattleEngine;

namespace BattleEffects
{
    public class DevanaTrapEffect : BattleEffect
    {
        bool completeEffect = false;
        BattleListener listener;

        public override void PrepareBattleEffect(CardActionTypes data)
        {
            this.ID = "DevanaTrapEffect";
            this._effectType = EffectType.BySpecID;
            this._buffType = EffectBuffType.Positive;

            this._viewVisibility = false;
            BattleResponse.AddListener(BattleResponseActions.GetDamage, Notified);
        }

        public override void PrepareBattleEffect(int amount)
        {
            this.ID = "DevanaTrapEffect";
            this._effectType = EffectType.BySpecID;
        }

        public void Notified(ActionSourceData data)
        {
            if (completeEffect == true)
                return;

            Unit recognise = UnitRecognizer.RecogniseAndGetUnit(data.SourceID);

            if(recognise == null)
            {
                Debug.LogError("Not found unit");
                return;
            }

            if (recognise.UnitSide == CardTargetSide.Enemy)
            {
                completeEffect = true;
                this.DestroyEffect();

                ActionSourceData sdata = ActionSourceData.Create()
                    .SetActionAmount(5)
                    .SetActionID("DevenaTrap")
                    .SetSourceID("DevanaTrapEffect")
                    .SetSourceType(ActionSourceType.Effect)
                    .SetAdditionalSourceID("Devana");

                recognise.Damage(sdata, Complete);
            }

            recognise = null;
        }

        protected override void TurnEnd()
        {
            base.TurnEnd();
        }
    }

}
