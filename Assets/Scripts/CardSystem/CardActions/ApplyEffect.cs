using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using BattleEffects;
using System.Linq;

namespace BattleEngine
{
    public class ApplyEffect : CardAction
    {
        public override string ActionType => CardActionType.ApplyEffect.ToString();

        private int _amount;
        private int _roundCount;
        private EffectType _effectType;
        private BattleEffect _effect;

        public override void Init(CardActionTypes data, string cardID)
        {
            this._amount = data.dEffectAmount;
            this._roundCount = data.dEffectroundCount;
            this._effectType = data.dEffectType;
            this.CardID = cardID;
            this.HeroID = IOM.GetHeroIDByCard(cardID);
            this.TargetSetup(data);
        }

        public override bool CanStart()
        {

            Hero hero = BattleSystem.Heroes.FirstOrDefault(her => her.Name.Equals(this.HeroID));

            if (hero != null && hero.EffectSystem.HasEffect(EffectType.Ice))
            {
                return false;
            }

            return base.CanStart();
        }

        protected override void Play()
        {
            if (_target.IsTargetSelected() == false)
            {
                _target.SetTarget();
                return;
            }

            ActionCount = 0;

            ActionSourceData data = ActionSourceData.Create()
                .SetSourceID(this.CardID)
                .SetAdditionalSourceID(this.HeroID)
                .SetActionID(_effectType.ToString())
                .SetSourceType(ActionSourceType.Effect)
                .SetActionAmount(_amount);

            for (int i = 0; i < _target.GetTargets.Count; i++)
            {
                
                _target.GetTargets[i].EffectSystem.AddEffect(data);
            }
            CompleteAction();
        }

        public override string GetDescription()
        {
            return LocalizationManager.Get("ApplyEffectDescription", this._effectType.ToString());
        }

        public override string GetDescription(CardActionTypes data)
        {
            return LocalizationManager.Get("ApplyEffectDescription", LocalizationManager.Get(data.dEffectType.ToString() + "Des"));
        }
    }
}