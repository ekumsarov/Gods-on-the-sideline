using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

namespace BattleEngine
{
    public class UnitAttack : UnitAction
    {
        [SerializeField] private float _multiplier = 0.8f;
        [SerializeField] private int _critChance = 5;
        [SerializeField] private float _critMultiplier = 0.4f;
        [SerializeField] private int ConstValue = -1;

        public override void Initialized(AIUnit parent)
        {
            base.Initialized(parent);
            _actionID = "UnitAttack";
        }

        protected override void StartAction()
        {
            if (_targets == null)
            {
                Done();
                return;
            }

            ActionSourceType sourcetype = ActionSourceType.Enemy;
            if (this._parent.UnitType == UnitType.Familiar)
            {
                sourcetype = ActionSourceType.Familiar;
            }

            for (int i = 0; i < _targets.Count; i++)
            {
                int damage = 0;

                if (ConstValue == -1)
                {
                    damage = Mathf.FloorToInt(_parent.BaseDamage * _multiplier);
                    if (ld.CheckChance(_critChance))
                        damage = Mathf.FloorToInt(damage + (damage * _critMultiplier));

                    damage = this._parent.EffectSystem.DamageBuffEffect(damage);
                    damage = this._parent.EffectSystem.DamageDebuuffEffect(damage);
                }
                else
                {
                    damage = ConstValue;
                    if (ld.CheckChance(_critChance))
                        damage = Mathf.FloorToInt(damage + (damage * _critMultiplier));

                    damage = this._parent.EffectSystem.DamageBuffEffect(damage);
                    damage = this._parent.EffectSystem.DamageDebuuffEffect(damage);
                }

                BattleLog.PushLog(this._parent.Name + " try to attack " + damage + " damage");
                ActionSourceData data = ActionSourceData.Create()
                .SetActionID("Attack")
                .SetActionAmount(damage)
                .SetSourceID(this._parent.Name)
                .SetSourceType(sourcetype);
                _targets[i].Damage(data, Done);
            }
        }

        public override int GetActionAmount()
        {
            if (ConstValue == -1)
                return Mathf.FloorToInt(_parent.BaseDamage * _multiplier);
            else
                return ConstValue;
        }
    }

}