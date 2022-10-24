using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

namespace BattleEngine
{
    public class UnitApplyEffect : UnitAction
    {
        [SerializeField] private CardTargetSide _sideToCast = CardTargetSide.Player;
        [SerializeField] private EffectType _effectType = EffectType.Bleeding;
        [SerializeField] private int _amount = 1;

        public override void Initialized(AIUnit parent)
        {
            if (_sideToCast == CardTargetSide.Enemy)
                this._actionType = UnitActionType.ApplyOnEnemy;

            if (_sideToCast == CardTargetSide.Player)
                this._actionType = UnitActionType.ApplyOnPlayer;

            if (_sideToCast == CardTargetSide.Any)
                this._actionType = UnitActionType.ApplyEffectAny;

            _actionID = "UnitApplyEffect";
            base.Initialized(parent);
        }

        

        protected override void StartAction()
        {
            if(_parent.EffectSystem.HasEffect(EffectType.Ice))
            {
                Done();
                return;
            }

            ActionSourceData data = ActionSourceData.Create()
                    .SetActionAmount(_amount)
                    .SetActionID(_effectType.ToString())
                    .SetSourceID(_parent.Name)
                    .SetSourceType(ActionSourceType.Effect);

            foreach (var target in _targets)
            {
                target.EffectSystem.AddEffect(data);
                    
            }

            Done();
        }

        public override int GetActionAmount()
        {
            return this._amount;
        }

        public override string GetActionID()
        {
            return this._effectType.ToString();
        }
    }

}