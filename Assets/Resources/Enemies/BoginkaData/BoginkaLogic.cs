using Lodkod;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BattleEngine
{
    public class BoginkaLogic : UnitLogic
    {
        protected override List<Unit> SelectForAttack(UnitActionType actionType, CardTargetSide side)
        {
            List<Unit> temp = new List<Unit>();

            if (actionType == UnitActionType.Attack)
            {
                List<Unit> selectable = UnitLogicUtility.GetUnitGroup(side);
                if (selectable.Count == 1)
                    return selectable;

                if (selectable.Any(unit => unit.EffectSystem.HasEffect(EffectType.Provocation)))
                {
                    temp.Add(selectable.First(unit => unit.EffectSystem.HasEffect(EffectType.Provocation)));
                    return new List<Unit>() { temp[Random.Range(0, temp.Count)] };
                }

                if (selectable.Any(unit => unit.Name.Equals("Devana")))
                {
                    temp.Add(selectable.First(unit => unit.Equals("Devana")));
                    return temp;
                }

                if (selectable.Any(unit => unit.UnitType == UnitType.Familiar))
                {
                    List<Unit> tempest = selectable.Where(uniy => uniy.UnitType == UnitType.Familiar).ToList<Unit>();
                    temp.Add(tempest[Random.Range(0, tempest.Count)]);
                    return temp;
                }

                List<Unit> selected = UnitLogicUtility.GetUnitWithLowestHP(side);

                if (selected == null || selected.Count == 0)
                {
                    Debug.LogError("Not found target for dps");
                    return null;
                }


                if (selected.Count == 1)
                    temp.Add(selected[0]);

                if (selected.Count > 1)
                    temp.Add(selected[Random.Range(0, selected.Count)]);
            }
            else if (actionType == UnitActionType.MassiveAttack)
            {
                List<Unit> selected = UnitLogicUtility.GetUnitGroup(side);

                if (selected == null || selected.Count == 0)
                {
                    Debug.LogError("Not found target for dps");
                    return null;
                }

                temp.AddRange(selected);
            }

            return temp;
        }
    }
}