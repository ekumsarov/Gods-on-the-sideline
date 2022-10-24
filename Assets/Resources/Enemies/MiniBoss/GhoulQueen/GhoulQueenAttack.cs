using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

namespace BattleEngine
{
    public class GhoulQueenAttack : UnitAction
    {
        private int targetMupltiplier = 0;
        private iBattleUnit targetToCheck = null;

        public override void Initialized(AIUnit parent)
        {
            base.Initialized(parent);
            _actionID = "GhoulQueenAttack";
        }

        protected override void StartAction()
        {
            if (_targets == null)
            {
                Done();
                return;
            }

            ActionSourceType sourcetype = ActionSourceType.Enemy;

            int damage = 2 + (3 * targetMupltiplier);

            damage = this._parent.EffectSystem.DamageBuffEffect(damage);
            damage = this._parent.EffectSystem.DamageDebuuffEffect(damage);

            for (int i = 0; i < _targets.Count; i++)
            {
                BattleLog.PushLog(this._parent.Name + " try to attack " + damage + " damage");
                ActionSourceData data = ActionSourceData.Create()
                .SetActionID("Attack")
                .SetActionAmount(damage)
                .SetSourceID(this._parent.Name)
                .SetSourceType(sourcetype);
                _targets[i].Damage(data, Done);
            }
        }

        public override void BindTargets(List<iBattleUnit> targets)
        {
            base.BindTargets(targets);

            if(targets[0] == null)
            {
                targetToCheck = null;
                targetMupltiplier = 0;
                return;
            }

            if(targets[0] != targetToCheck)
            {
                targetToCheck = targets[0];
                targetMupltiplier = 0;
                return;
            }

            targetMupltiplier += 1;
        }

        public override int GetActionAmount()
        {
            return 2;
        }
    }

}