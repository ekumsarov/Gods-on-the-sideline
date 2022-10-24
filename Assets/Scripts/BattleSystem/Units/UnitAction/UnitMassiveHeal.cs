using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

namespace BattleEngine
{
    public class UnitMassiveHeal : UnitAction
    {
        [SerializeField] private float _multiplier = 0.8f;
        [SerializeField] private int _critChance = 0;
        [SerializeField] private float _critMultiplier = 0.4f;
        [SerializeField] private int ConstValue = -1;

        public override void Initialized(AIUnit parent)
        {
            _actionID = "UnitMassiveHeal";
            this._actionType = UnitActionType.MassiveHeal;
            base.Initialized(parent);
        }

        protected override void StartAction()
        {
            for (int i = 0; i < _targets.Count; i++)
            {
                int damage = 0;

                if (ConstValue == -1)
                {
                    damage = Mathf.FloorToInt(_parent.BaseDamage * _multiplier);
                    if (ld.CheckChance(_critChance))
                        damage = Mathf.FloorToInt(damage + (damage * _critMultiplier));
                }
                else
                {
                    damage = ConstValue;
                    if (ld.CheckChance(_critChance))
                        damage = Mathf.FloorToInt(damage + (damage * _critMultiplier));
                }

                _targets[i].Heal(damage, this._parent.Name, Done);
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