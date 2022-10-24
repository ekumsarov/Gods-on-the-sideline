using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using System.Linq;

namespace CardTargetSelection
{
    public class TargetSelectType
    {
        public static iTargetSelection Get(CardActionTypes data, iTargetSelection target)
        {
            if (data.dTargetIncludes == CardTargetType.Class)
                return new ClassTargetType(data, target);

            if (data.dTargetIncludes == CardTargetType.ID)
                return new IDTargetType(data, target);

            return new AnyTargetType(target);
        }
    }

    public class AnyTargetType : iTargetSelection
    {
        private iTargetSelection _selection;

        public AnyTargetType(iTargetSelection target)
        {
            _selection = target;
        }

        public bool NeedSelection()
        {
            return _selection.NeedSelection();
        }

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

    public class ClassTargetType : iTargetSelection
    {
        private string _class;
        private iTargetSelection _selection;
        public ClassTargetType(CardActionTypes data, iTargetSelection target)
        {
            _selection = target;
            _class = data.dTargetID;
        }

        public bool NeedSelection()
        {
            return _selection.NeedSelection();
        }

        public List<iBattleUnit> CanChooseTarget(List<iBattleUnit> targets)
        {
            List<iBattleUnit> temp = new List<iBattleUnit>();
            foreach(var unit in targets)
            {
                var tempUnit = unit as Hero;
                if (tempUnit != null && tempUnit.Class.ToString().Equals(_class))
                    temp.Add(tempUnit);
            }

            return _selection.CanChooseTarget(temp);
        }

        public iBattleUnit SelectedTarget(iBattleUnit unit)
        {
            var tempUnit = unit as Hero;
            if (tempUnit != null && tempUnit.Class.ToString().Equals(_class))
                return _selection.SelectedTarget(tempUnit);

            return null;
        }

        public List<iBattleUnit> SetTarget(List<iBattleUnit> targets)
        {
            List<iBattleUnit> temp = new List<iBattleUnit>();
            foreach (var unit in targets)
            {
                var tempUnit = unit as Hero;
                if (tempUnit != null && tempUnit.Class.ToString().Equals(_class))
                    temp.Add(tempUnit);
            }

            return _selection.SetTarget(temp);
        }


    }

    public class IDTargetType : iTargetSelection
    {
        private string _id;
        private iTargetSelection _selection;
        public IDTargetType(CardActionTypes data, iTargetSelection target)
        {
            _selection = target;
            _id = data.dTargetID;
        }

        public bool NeedSelection()
        {
            return _selection.NeedSelection();
        }

        public List<iBattleUnit> CanChooseTarget(List<iBattleUnit> targets)
        {
            return _selection.CanChooseTarget(targets.Where(unit => unit.Name.Equals(_id)).ToList<iBattleUnit>());
        }

        public iBattleUnit SelectedTarget(iBattleUnit unit)
        {
            if (unit != null && unit.Name.Equals(_id))
                return _selection.SelectedTarget(unit);

            return null;
        }

        public List<iBattleUnit> SetTarget(List<iBattleUnit> targets)
        {
            return _selection.SetTarget(targets.Where(unit => unit.Name.Equals(_id)).ToList());
        }
    }
}