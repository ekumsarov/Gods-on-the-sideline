using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using CardTargetSelection;
using System.Linq;

namespace BattleEngine
{
    public class Attack : CardAction
    {
        public override string ActionType => CardActionType.Attack.ToString();

        private int _damageAmount;
        public int Damage
        {
            get { return _damageAmount; }
        }

        public override void Init(CardActionTypes data, string cardID)
        {
            this._damageAmount = data.dDamageAmount;
            this.TargetSetup(data);
            this.CardID = cardID;
            this.HeroID = UnitRecognizer.GetHeroIDByCard(this.CardID);
        }

        public override bool CanStart()
        {
            Hero hero = BattleSystem.Heroes.FirstOrDefault(her => her.Name.Equals(this.HeroID));

            if (hero != null && hero.EffectSystem.HasEffect(EffectType.Blindless.ToString()))
            {
                int effectAmount = hero.EffectSystem.GetEffect(EffectType.Blindless.ToString()).AmountCount;

                if (Random.Range(0, 101) < effectAmount * 10)
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

            CurrentActionCount = 0;

            Hero hero = BattleSystem.Heroes.FirstOrDefault(her => her.Name.Equals(HeroID));

            if(hero != null)
            {
                this._damageAmount = hero.EffectSystem.DamageBuffEffect(this._damageAmount);
                this._damageAmount = hero.EffectSystem.DamageDebuuffEffect(this._damageAmount);
            }
            else
            {
                Debug.LogError("Not found parent for Hero " + HeroID);
            }

            ActionCount = _target.GetTargets.Count;

            ActionSourceData data = ActionSourceData.Create()
                .SetActionID("Attack")
                .SetActionAmount(this._damageAmount)
                .SetSourceID(this.CardID)
                .SetAdditionalSourceID(HeroID)
                .SetSourceType(ActionSourceType.Card);

            for(int i = 0; i < _target.GetTargets.Count; i++)
            {
                _target.GetTargets[i].Damage(data, CompleteAction);
            }
        }

        public override string GetDescription()
        {
            return LocalizationManager.Get("AttackDescription", this._damageAmount);
        }

        public override string GetDescription(CardActionTypes data)
        {
            return LocalizationManager.Get("AttackDescription", data.dDamageAmount) + " " + CardActionTarget.GetTargetDescription(data.dTarget, data.dTargetSide, data.dTargetIncludes);
        }
    }
}
