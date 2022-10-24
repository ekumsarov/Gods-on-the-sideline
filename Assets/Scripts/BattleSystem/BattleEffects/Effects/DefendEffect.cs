using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using BattleEngine;

namespace BattleEffects
{
    public class DefendEffect : BattleEffect
    {
        private string _effect = string.Empty;
        BattleListener listener;

        public override void PrepareBattleEffect(CardActionTypes data)
        {
            this.ID = "DefendEffect";
            this._effectType = EffectType.DefendEffect;
            this._buffType = EffectBuffType.Positive;

            this._amount = data.dEffectAmount;
            this._effect = data.dEffectSpecID;

            BattleResponse.AddListener(BattleResponseActions.GetDamage, Notified);
        }

        public override void PrepareBattleEffect(int amount)
        {
            this.ID = "DefendEffect";
            this._effectType = EffectType.DefendEffect;

            this._amount = amount;
        }

        public void Notified(ActionSourceData data)
        {
            Unit recognise = UnitRecognizer.RecogniseAndGetUnit(data.SourceID);
            if (recognise != null && recognise.UnitSide == CardTargetSide.Enemy)
            {
                ActionSourceData actDdata = ActionSourceData.Create()
                    .SetSourceID(data.SourceID)
                    .SetActionID(_effectType.ToString())
                    .SetSourceType(ActionSourceType.Effect)
                    .SetActionAmount(_amount);

                recognise.EffectSystem.AddEffect(actDdata);
            }

            recognise = null;
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