using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using BattleEffects;
using System.Linq;
using BattleEngine;

namespace BattleEngine
{
    public class GhoulQueenUnit : AIUnit, IBattleFunction
    {
        private bool _isInCoffin = false;
        private Unit _ghoulTarget = null;

        public static new AIUnit Make(UnitData data)
        {
            GhoulQueenUnit temp = new GhoulQueenUnit();

            temp._actions = new List<UnitAction>();
            for (int i = 0; i < data.Actions.Count; i++)
            {
                temp._actions.Add((UnitAction)data.Actions[i].Clone());
            }

            temp._features = new List<UnitFeature>();
            for (int i = 0; i < data.Features.Count; i++)
            {
                temp._features.Add((UnitFeature)data.Features[i].Clone());
            }

            temp._logic = data.Logic;
            temp._logicType = data.LogicType;
            temp._unitSide = data.Side;
            temp._name = data.Name;
            temp._icon = data.Icon;
            temp._class = data.Class;
            temp._hp = data.HP;
            temp._currentHP = temp._hp;
            temp._unitType = UnitType.Enemy;
            temp._baseDamage = data.BaseDamage;
            temp._selectUnitConditions = data.SelectUnitConditions;
            temp._susceptibility = data.Susceptibility;
            temp._sustainability = data.Sustainability;
            temp._damageRule = data.DamageRule;
            temp._healRule = data.HealRule;
            temp.ActivateEffect();

            if (temp._damageRule == null)
                temp._damageRule = new DamageRule();

            if (temp._healRule == null)
                temp._healRule = new HealRule();

            if (temp._logic == null)
                temp._logic = new UnitLogic();

            temp._damageRule.SetupParent(temp);
            temp._healRule.SetupParent(temp);

            for (int i = 0; i < temp._actions.Count; i++)
            {
                temp._actions[i].Initialized(temp);
            }

            for (int i = 0; i < temp._features.Count; i++)
            {
                temp._features[i].Initialized(temp);
            }


            return temp;
        } 

        protected override void StartAction()
        {
            // ForTest 
            //UnitLogicResult result = UnitLogic.DoLogic(this.LogicType, this._actions, this.UnitSide);

            float value1 = CurrentHP;
            float value2 = HP;

            float check = value1 / value2;

            if (check <= 0.3f && _isInCoffin == false)
            {
                _isInCoffin = true;

                for (int i = 0; i < this._actions.Count; i++)
                {
                    if (this._actions[i].ActionID.Equals("GhoulQueenAttack"))
                    {
                        this._actions[i].BindTargets(new List<iBattleUnit>() { null });
                    }
                    else if(this._actions[i].ActionID.Equals("UnitApplyEffect"))
                    {
                        this._actions[i].ReduceCoolDownToZero();
                    }
                }
                _ghoulTarget = null;

                ActionSourceData data = ActionSourceData.Create()
                    .SetActionAmount(100)
                    .SetActionID(EffectType.Dodge.ToString())
                    .SetSourceType(ActionSourceType.Effect)
                    .SetSourceID("GhoulQueen");

                this.EffectSystem.AddEffect(data);


                BattleSystem.AddUnit(Unit.Make("Ghoul"), false);
                BattleSystem.AddUnit(Unit.Make("Ghoul"), false);
                BattleSystem.AddUnit(Unit.Make("Ghoul"), false);

                EndAction();
                return;
            }

            if(_isInCoffin)
            {
                if(BattleSystem.GetAliveEnemies().Count() == 1)
                {
                    _isInCoffin = false;
                    this.EffectSystem.RemoveEffect(EffectType.Dodge.ToString());
                    EndAction();
                    return;
                }

                for(int i = 0; i < this._actions.Count; i++)
                {
                    if(this._actions[i].ActionID.Equals("UnitHeal"))
                    {
                        this._actions[i].BindTargets(new List<iBattleUnit>() { this });
                        this._actions[i].StartFunction(EndAction);
                        return;
                    }
                }
            }

            List<UnitAction> avaliavleActions = new List<UnitAction>();
            for (int i = 0; i < _actions.Count; i++)
            {
                if (_actions[i].OnCooldown == false)
                {
                    avaliavleActions.Add(_actions[i]);
                }
            }

            UnitAction selectedAction = avaliavleActions[Random.Range(0, avaliavleActions.Count)];

            if(selectedAction.ActionID.Equals("GhoulQueenAttack"))
            {
                List<Unit> units = UnitLogicUtility.GetUnitGroup(CardTargetSide.Enemy);

                if (_ghoulTarget == null)
                {
                    _ghoulTarget = units[Random.Range(0, units.Count)];
                }
                else
                {
                    if(units.Any(unit => unit.EffectSystem.HasEffect(EffectType.Provocation)))
                    {
                        selectedAction.BindTargets(new List<iBattleUnit>() { null });
                        _ghoulTarget = units.FirstOrDefault(unit => unit.EffectSystem.HasEffect(EffectType.Provocation));
                    }
                }

                selectedAction.BindTargets(new List<iBattleUnit>() { _ghoulTarget });
                selectedAction.StartFunction(EndAction);
                return;
            }


            UnitLogicResult result = _logic.DoLogic(new List<UnitAction>() { selectedAction }, this.UnitSide);

            if (result.Action.ActionType == UnitActionType.Summon)
            {
                result.Action.BindTargets(result.Targets);
                result.Action.StartFunction(EndAction);
                result = null;
                return;
            }

            if (result == null || result.Action == null || result.Targets == null || result.Targets.Count == 0)
            {
                Debug.LogError("Enemy did not found units or hero");
                EndAction();
                return;
            }

            result.Action.BindTargets(result.Targets);
            result.Action.StartFunction(EndAction);
            result = null;
        }

        public override void NextRound()
        {
            for (int i = 0; i < this._actions.Count; i++)
            {
                this._actions[i].ReduceCoolDown();
            }
            base.NextRound();
        }
    }
}