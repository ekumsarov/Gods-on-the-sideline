using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using System.Linq;

namespace CardTargetSelection
{
    public class TargetSelectSide
    {
        public static iTargetSelection Get(CardActionTypes data, iTargetSelection target)
        {
            if (data.dTargetSide == CardTargetSide.Player)
                return new TargetSidePlayer(target);

            if (data.dTargetSide == CardTargetSide.Enemy)
                return new TargetSideEnemy(target);

            return new TargetSideAny(target);
        }
    }

    public class TargetSideAny : iTargetSelection
    {
        public bool NeedSelection()
        {
            return _selection.NeedSelection();
        }

        private iTargetSelection _selection;

        public TargetSideAny(iTargetSelection selection) { _selection = selection; }

        public List<iBattleUnit> CanChooseTarget(List<iBattleUnit> targets)
        {
            return _selection.CanChooseTarget(targets);
        }

        public iBattleUnit SelectedTarget(iBattleUnit unit)
        {
            return _selection.SelectedTarget(unit);
        }

        public List<iBattleUnit> SetTarget(List<iBattleUnit> targets)
        {
            return _selection.SetTarget(targets);
        }
    }

    public class TargetSidePlayer : iTargetSelection
    {
        private iTargetSelection _selection;

        public TargetSidePlayer(iTargetSelection selection) { _selection = selection; }

        public List<iBattleUnit> CanChooseTarget(List<iBattleUnit> targets)
        {
            return _selection.CanChooseTarget(targets.Where(unit => unit.UnitSide == CardTargetSide.Player).ToList<iBattleUnit>());
        }

        public bool NeedSelection()
        {
            return _selection.NeedSelection();
        }

        public iBattleUnit SelectedTarget(iBattleUnit unit)
        {
            if (unit == null)
                return null;

            if (unit.UnitSide == CardTargetSide.Enemy)
                return null;

            return _selection.SelectedTarget(unit);
        }

        public List<iBattleUnit> SetTarget(List<iBattleUnit> targets)
        {
            return _selection.SetTarget(targets.Where(unit => unit.UnitSide == CardTargetSide.Player).ToList<iBattleUnit>());
        }
    }

    public class TargetSideEnemy : iTargetSelection
    {
        private iTargetSelection _selection;

        public TargetSideEnemy(iTargetSelection selection) { _selection = selection; }

        public List<iBattleUnit> CanChooseTarget(List<iBattleUnit> targets)
        {
            return _selection.CanChooseTarget(targets.Where(unit => unit.UnitSide == CardTargetSide.Enemy).ToList<iBattleUnit>());
        }

        public bool NeedSelection()
        {
            return _selection.NeedSelection();
        }

        public iBattleUnit SelectedTarget(iBattleUnit unit)
        {
            if (unit == null)
                return null;

            if (unit.UnitSide == CardTargetSide.Player)
                return null;

            return _selection.SelectedTarget(unit);
        }

        public List<iBattleUnit> SetTarget(List<iBattleUnit> targets)
        {
            return _selection.SetTarget(targets.Where(unit => unit.UnitSide == CardTargetSide.Enemy).ToList<iBattleUnit>());
        }
    }
}