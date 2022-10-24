using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using System;

namespace BattleEngine
{
    public class Familiar : AIUnit
    {
        private bool _activated = false;
        List<UnitAction> _activateActions;
        List<UnitAction> _dieActions;

        public static Familiar Make(FamiliarData data)
        {
            Familiar temp = new Familiar();

            temp._actions = data.Actions;
            temp._name = BattleSystem.CheckNames(data.Name);
            temp._icon = data.Icon;
            temp._class = data.Class;
            temp._hp = data.HP;
            temp._currentHP = temp._hp;
            temp._unitType = UnitType.Enemy;
            temp._activated = false;
            temp._logicType = data.LogicType;
            temp._unitSide = data.UnitSide;
            temp._selectUnitConditions = data.SelectUnitConditions;
            temp._damageRule = new DamageRule();
            temp._healRule = new HealRule();
            temp.ActivateEffect();

            temp._damageRule.SetupParent(temp);
            temp._healRule.SetupParent(temp);

            for (int i = 0; i < temp._actions.Count; i++)
            {
                temp._actions[i].Initialized(temp);
            }

            temp._activateActions = new List<UnitAction>();
            for (int i = 0; i < data.StartActions.Count; i++)
            {
                temp._activateActions.Add((UnitAction)data.StartActions[i].Clone());
            }

            temp._dieActions = new List<UnitAction>();
            for (int i = 0; i < data.EndActions.Count; i++)
            {
                temp._dieActions.Add((UnitAction)data.EndActions[i].Clone());
            }

            return temp;
        }

        public void ActivateFamiliar()
        {
            for(int i = 0; i < this._activateActions.Count; i++)
            {
                BattleSystem.AddFunctionToQuery(this._activateActions[i]);
            }
        }

        public void EndFamiliar()
        {
            for (int i = 0; i < this._activateActions.Count; i++)
            {
                BattleSystem.AddFunctionToQuery(this._dieActions[i]);
            }
        }
    }
}

