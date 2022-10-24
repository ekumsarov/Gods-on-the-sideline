using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using BattleEngine;
using System.Linq;

namespace CardTargetSelection
{
    public class TargetSelect
    {
        public static iTargetSelection Get(CardActionTypes data)
        {
            if (data.dTarget == CardTarget.FirstSelect)
                return new FirstSelect();

            if (data.dTarget == CardTarget.FirstSelected)
                return new FirstSelectedTarget();

            if (data.dTarget == CardTarget.AllSelected || data.dTarget == CardTarget.SelectedTargets)
                return new SelectedTargets();

            if (data.dTarget == CardTarget.SelectAnother)
                return new SelectAnotherTarget();

            if (data.dTarget == CardTarget.Random)
                return new RandomTargets();

            if (data.dTarget == CardTarget.Group)
                return new GroupTargets();

            if (data.dTarget == CardTarget.AllNotSelected)
                return new AllNotSelectedTargets();

            if (data.dTarget == CardTarget.AutoSelect || data.dTarget == CardTarget.FromCondition)
                return new ConditionAutoSelect();

            return new SelectTargetSelection();
        }
    }

    public class SelectTargetSelection : iTargetSelection
    {
        public bool NeedSelection()
        {
            return true;
        }

        public List<iBattleUnit> CanChooseTarget(List<iBattleUnit> targets)
        {
            return targets;
        }

        public iBattleUnit SelectedTarget(iBattleUnit unit)
        {
            return unit;
        }

        public List<iBattleUnit> SetTarget(List<iBattleUnit> targets)
        {
            return new List<iBattleUnit>();
        }
    }

    public class FirstSelectedTarget : iTargetSelection
    {
        public bool NeedSelection()
        {
            return false;
        }

        public List<iBattleUnit> CanChooseTarget(List<iBattleUnit> targets)
        {
            if (BattleSystem.SelectedTargets.Count > 0)
                return targets;

            return new List<iBattleUnit>();
        }

        public iBattleUnit SelectedTarget(iBattleUnit unit)
        {
            return null;
        }

        public List<iBattleUnit> SetTarget(List<iBattleUnit> targets)
        {
            return new List<iBattleUnit>() { BattleSystem.SelectedTargets[0] };
        }
    }

    public class SelectAnotherTarget : iTargetSelection
    {
        private iBattleUnit _selected = null;
        private bool _needSelection = true;

        public bool NeedSelection()
        {
            return _needSelection;
        }

        public List<iBattleUnit> CanChooseTarget(List<iBattleUnit> targets)
        {
            List<iBattleUnit> temp = new List<iBattleUnit>();
            foreach(var unit in targets)
            {
                if(BattleSystem.SelectedTargets.Any(sunit => sunit == unit) == false)
                {
                    temp.Add(unit);
                }
            }

            if (temp.Count == 1)
            {
                _needSelection = false;
                _selected = temp[0];
            }
                
            return temp;
        }

        public iBattleUnit SelectedTarget(iBattleUnit unit)
        {
            if (BattleSystem.SelectedTargets.Any(sunit => sunit == unit))
                return null;

            _selected = unit;
            return _selected;
        }

        public List<iBattleUnit> SetTarget(List<iBattleUnit> targets)
        {
            List<iBattleUnit> temp = new List<iBattleUnit>();

            if (_selected == null)
            {
                return temp;
            }

            
            if (targets.Any(unit => unit == _selected))
                temp.Add(_selected);

            return temp;
        }
    }

    public class FirstSelect : iTargetSelection
    {
        public bool NeedSelection()
        {
            return true;
        }

        public List<iBattleUnit> CanChooseTarget(List<iBattleUnit> targets)
        {
            return targets;
        }

        public iBattleUnit SelectedTarget(iBattleUnit unit)
        {
            return unit;
        }

        public List<iBattleUnit> SetTarget(List<iBattleUnit> targets)
        {
            return new List<iBattleUnit>();
        }
    }

    public class SelectedTargets : iTargetSelection
    {
        public bool NeedSelection()
        {
            return false;
        }

        public List<iBattleUnit> CanChooseTarget(List<iBattleUnit> targets)
        {
            return BattleSystem.SelectedTargets;
        }

        public iBattleUnit SelectedTarget(iBattleUnit unit)
        {
            return null;
        }

        public List<iBattleUnit> SetTarget(List<iBattleUnit> targets)
        {
            return BattleSystem.SelectedTargets;
        }
    }

    public class AllNotSelectedTargets : iTargetSelection
    {
        public bool NeedSelection()
        {
            return false;
        }

        public List<iBattleUnit> CanChooseTarget(List<iBattleUnit> targets)
        {
            List<iBattleUnit> temp = new List<iBattleUnit>();
            if(targets.Count > 0)
            {
                foreach(var unit in targets)
                {
                    if(BattleSystem.SelectedTargets.Any(sunit => sunit == unit) == false)
                    {
                        temp.Add(unit);
                    }
                }
            }

            return temp;
        }

        public iBattleUnit SelectedTarget(iBattleUnit unit)
        {
            return null;
        }

        public List<iBattleUnit> SetTarget(List<iBattleUnit> targets)
        {
            return targets;
        }
    }

    public class GroupTargets : iTargetSelection
    {
        public bool NeedSelection()
        {
            return false;
        }

        public List<iBattleUnit> CanChooseTarget(List<iBattleUnit> targets)
        {
            return targets;
        }

        public iBattleUnit SelectedTarget(iBattleUnit unit)
        {
            return null;
        }

        public List<iBattleUnit> SetTarget(List<iBattleUnit> targets)
        {
            return targets;
        }
    }

    public class RandomTargets : iTargetSelection
    {
        public bool NeedSelection()
        {
            return false;
        }

        public List<iBattleUnit> CanChooseTarget(List<iBattleUnit> targets)
        {
            return targets;
        }

        public iBattleUnit SelectedTarget(iBattleUnit unit)
        {
            return null;
        }

        public List<iBattleUnit> SetTarget(List<iBattleUnit> targets)
        {
            return new List<iBattleUnit>() { targets[Random.Range(0, targets.Count)] };
        }
    }

    public class ConditionAutoSelect : iTargetSelection
    {
        public bool NeedSelection()
        {
            return false;
        }

        public List<iBattleUnit> CanChooseTarget(List<iBattleUnit> targets)
        {
            return targets;
        }

        public iBattleUnit SelectedTarget(iBattleUnit unit)
        {
            return unit;
        }

        public List<iBattleUnit> SetTarget(List<iBattleUnit> targets)
        {
            return new List<iBattleUnit>();
        }
    }
}

