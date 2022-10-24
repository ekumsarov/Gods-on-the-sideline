using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using BattleEngine;
using System.Linq;
using System;

namespace CardTargetSelection
{
    public class CardActionTarget
    {
        private CardTarget _targetType = CardTarget.FirstSelected;

        private List<iBattleUnit> _targets;
        public List<iBattleUnit> Targets => _targets;

        private List<iBattleUnit> _avaliables;
        public List<iBattleUnit> Avaliables => _avaliables;

        private string targetID = string.Empty;

        private iTargetSelection _selection;

        private Action _callback = null;

        private List<Condition> _conditions;

        public static CardActionTarget Create(CardTarget target, CardTargetSide side, CardTargetType type)
        {
            return new CardActionTarget()
            {
                _targets = new List<iBattleUnit>(),
                _targetType = target
            };
        }

        public static CardActionTarget Create(CardActionTypes data)
        {
            CardActionTarget temp = new CardActionTarget();
            temp._targets = new List<iBattleUnit>();
            temp._avaliables = new List<iBattleUnit>();
            temp._selection = TargetSelectSide.Get(data, TargetSelectType.Get(data, TargetSelect.Get(data)));
            temp._conditions = Condition.GetConditions(data.dConditions);
            temp._targetType = data.dTarget;
            return temp;

            /*return new CardActionTarget()
            {
                _targets = new List<iBattleUnit>(),
                _targetType = data.dTarget,
                _type = data.dTargetIncludes,
                _side = data.dTargetSide,
                targetID = data.dTargetID
            };*/
        }

        public bool CanChooseTarget(Action callback)
        {
            if (IsTargetSelected())
                return true;

            for (int i = 0; i < _conditions.Count; i++)
            {
                if (_conditions[i].Available == false)
                    return false;
            }

            _avaliables = BattleSystem.GetAllAlive();
            _avaliables = _selection.CanChooseTarget(_avaliables);

            if (_avaliables.Count == 0)
                return false;

            if (_selection.NeedSelection())
            {
                BattleSystem.SubscribeOnUnitPoint(SelectedTarget);
                _callback = callback;
            }
            else
                SetTarget();

            return true;
        }

        public bool IsTargetSelected()
        {
            return _targets.Count > 0;
        }

        public void SetTarget()
        {
            if (_avaliables.Any(unit => unit.EffectSystem.HasEffect("Provocation")))
            {
                _targets.Add(_avaliables.FirstOrDefault(uni => uni.EffectSystem.HasEffect("Provocation")));
                foreach (var unit in _targets)
                {
                    unit.EffectSystem.ReduceEffectAmount("Provocation", 3);
                    BattleSystem.AddSelectedTargets(unit);
                }
                BattleSystem.UnsubscribeOnUnitPoint(SelectedTarget);
                return;
            }

            List<iBattleUnit> temp = _selection.SetTarget(_avaliables);

            if (temp != null)
            {
                if(_targetType == CardTarget.FromCondition)
                {
                    for (int i = 0; i < _conditions.Count; i++)
                    {
                        temp = _conditions[i].GetUnitsFromCondition(temp);
                    }
                }

                for (int i = 0; i < temp.Count; i++)
                {
                    if (_targets.Any(unit => unit == temp[i]) == false)
                        _targets.Add(temp[i]);
                }

            }


            foreach (var unit in _targets)
            {
                BattleSystem.AddSelectedTargets(unit);
            }

            if (_targets.Count > 0)
            {
                BattleSystem.UnsubscribeOnUnitPoint(SelectedTarget);
                _callback?.Invoke();
            }

        }

        // Need Checks
        public void SelectedTarget(iBattleUnit unit)
        {
            iBattleUnit check = _selection.SelectedTarget(unit);

            if (check != null)
            {
                _targets.Add(unit);
                BattleSystem.UnsubscribeOnUnitPoint(SelectedTarget);
                BattleSystem.AddSelectedTargets(unit);
                _callback?.Invoke();
            }
        }

        public static string GetTargetDescription(CardTarget target, CardTargetSide side, CardTargetType type)
        {
            string text = "";

            if (target == CardTarget.FirstSelect || target == CardTarget.SelectTarget)
                text += "";

            if (target == CardTarget.FirstSelected)
                text += LocalizationManager.Get("FirstSelectedDes");

            if (target == CardTarget.Random)
                text += LocalizationManager.Get("CardTargetRandomDes");

            if (target == CardTarget.SelectAnother)
                text += LocalizationManager.Get("SelectAnotherDes");

            if (target == CardTarget.SelectedTargets)
                text += LocalizationManager.Get("SelectedTargetsDes");

            if (target == CardTarget.AllNotSelected)
                text += LocalizationManager.Get("AllNotSelectedDes");

            return text;
        }

        public List<iBattleUnit> GetTargets => _targets;
    }
}

