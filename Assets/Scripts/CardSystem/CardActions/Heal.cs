using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using CardTargetSelection;

namespace BattleEngine
{
    public class Heal : CardAction
    {
        public override string ActionType => CardActionType.Heal.ToString();

        private int _healAmount;

        public override void Init(CardActionTypes data, string heroID)
        {
            this._healAmount = data.dHealAmount;
            this.HeroID = heroID;
            this.TargetSetup(data);
        }

        protected override void Play()
        {
            if (_target.IsTargetSelected() == false)
            {
                _target.SetTarget();
                return;
            }

            CurrentActionCount = 0;
            ActionCount = _target.GetTargets.Count;

            for (int i = 0; i < _target.GetTargets.Count; i++)
            {
                _target.GetTargets[i].Heal(this._healAmount, "Card", CompleteAction);
            }
        }

        public override string GetDescription(CardActionTypes data)
        {
            return LocalizationManager.Get("HealDescription", data.dHealAmount) + " " + CardActionTarget.GetTargetDescription(data.dTarget, data.dTargetSide, data.dTargetIncludes);
        }
    }
}

