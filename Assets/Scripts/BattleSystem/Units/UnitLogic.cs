using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using System.Linq;

namespace BattleEngine
{
    public class UnitLogic : MonoBehaviour
    {
        public virtual UnitLogicResult DoLogic(List<UnitAction> actions, CardTargetSide side)
        {
            List<Unit> temp = null;

            List<UnitAction> avaliavleActions = new List<UnitAction>();
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i].OnCooldown == false)
                {
                    avaliavleActions.Add(actions[i]);
                }
            }

            if (avaliavleActions.Count == 0)
            {
                Debug.LogError("No avaliable action");
                return UnitLogicResult.Create(null, null);
            }

            UnitAction action = avaliavleActions[Random.Range(0, avaliavleActions.Count)];

            if (action.ActionType == UnitActionType.Attack || action.ActionType == UnitActionType.MassiveAttack)
            {
                temp = SelectForAttack(action.ActionType, side);
            }
            else if (action.ActionType == UnitActionType.ApplyOnPlayer)
            {
                temp = SelectForEffects(action.ActionType);
            }

            return UnitLogicResult.Create(action, temp.ToList<iBattleUnit>());
        }

        protected virtual List<Unit> SelectForAttack(UnitActionType actionType, CardTargetSide side)
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

        protected virtual List<Unit> SelectForEffects(UnitActionType actionType)
        {
            List<Unit> temp = new List<Unit>();
            List<Unit> selected = UnitLogicUtility.GetUnitGroup(CardTargetSide.Enemy);
            selected = selected.Where(unit => unit.UnitType != UnitType.Familiar).ToList();

            if (selected == null || selected.Count == 0)
            {
                Debug.LogError("Not found target for dps");
                return null;
            }

            temp.Add(selected[Random.Range(0, selected.Count)]);

            return temp;
        }

        protected virtual List<Unit> SelectForHeal(UnitActionType actionType)
        {
            List<Unit> temp = new List<Unit>();

            return temp;
        }
    }

    public static class UnitLogicUtility
    {
        public static List<iBattleUnit> GetGroup(CardTargetSide side)
        {
            if (side == CardTargetSide.Enemy)
                return BattleSystem.GetAllAlive().Where(unit => unit.UnitSide == CardTargetSide.Player).ToList();
            else if (side == CardTargetSide.Player)
                return BattleSystem.GetAllAlive().Where(unit => unit.UnitSide == CardTargetSide.Enemy).ToList();
            else
                return BattleSystem.GetAllAlive();
        }

        public static List<Unit> GetUnitGroup(CardTargetSide side)
        {
            if (side == CardTargetSide.Player)
            {
                List<Unit> temp = new List<Unit>();
                temp.AddRange(BattleSystem.Enemies);
                temp.AddRange(BattleSystem.EnemiesFamiliars);
                return temp;
            }
            else if (side == CardTargetSide.Enemy)
            {
                List<Unit> temp = new List<Unit>();
                temp.AddRange(BattleSystem.Heroes);
                temp.AddRange(BattleSystem.HeroesFamiliars);
                return temp;
            }

            List<Unit> tempf = new List<Unit>();
            tempf.AddRange(BattleSystem.Enemies);
            tempf.AddRange(BattleSystem.EnemiesFamiliars);
            tempf.AddRange(BattleSystem.Heroes);
            tempf.AddRange(BattleSystem.HeroesFamiliars);
            return tempf;
        }

        public static List<iBattleUnit> GetWithLowestHP(CardTargetSide unitSide)
        {
            List<iBattleUnit> heroAvaliable = null;

            if (unitSide == CardTargetSide.Enemy)
            {
                heroAvaliable = GetGroup(unitSide);
                if (heroAvaliable != null && heroAvaliable.Count > 1)
                {
                    int lowest = 1000;
                    for (int i = 0; i < heroAvaliable.Count; i++)
                    {
                        int actionAmount = heroAvaliable[i].CurrentHP;
                        if (actionAmount < lowest)
                            lowest = actionAmount;
                    }

                    heroAvaliable = heroAvaliable.Where(action => action.CurrentHP == lowest).ToList();

                    if (heroAvaliable.Count > 1)
                    {
                        int count = heroAvaliable.Count - 1;
                        for (int i = 0; i < count; i++)
                        {
                            heroAvaliable.RemoveAt(Random.Range(0, heroAvaliable.Count));
                        }
                    }
                }
            }
            else
            {
                heroAvaliable = GetGroup(unitSide);
                if (heroAvaliable != null && heroAvaliable.Count > 1)
                {
                    int lowest = 1000;
                    for (int i = 0; i < heroAvaliable.Count; i++)
                    {
                        int actionAmount = heroAvaliable[i].CurrentHP;
                        if (actionAmount < lowest)
                            lowest = actionAmount;
                    }

                    heroAvaliable = heroAvaliable.Where(action => action.CurrentHP == lowest).ToList();

                    if (heroAvaliable.Count > 1)
                    {
                        int count = heroAvaliable.Count - 1;
                        for (int i = 0; i < count; i++)
                        {
                            heroAvaliable.RemoveAt(Random.Range(0, heroAvaliable.Count));
                        }
                    }
                }
            }


            if (heroAvaliable == null)
                return null;

            if (heroAvaliable.Count > 1)
                return new List<iBattleUnit>() { heroAvaliable[Random.Range(0, heroAvaliable.Count)] };
            else if (heroAvaliable.Count == 1)
                return heroAvaliable;
            else
                return null;
        }

        public static List<Unit> GetUnitWithLowestHP(CardTargetSide unitSide)
        {
            List<Unit> heroAvaliable = null;

            if (unitSide == CardTargetSide.Enemy)
            {
                heroAvaliable = GetUnitGroup(unitSide);
                if (heroAvaliable != null && heroAvaliable.Count > 1)
                {
                    int lowest = 1000;
                    for (int i = 0; i < heroAvaliable.Count; i++)
                    {
                        int actionAmount = heroAvaliable[i].CurrentHP;
                        if (actionAmount < lowest)
                            lowest = actionAmount;
                    }

                    heroAvaliable = heroAvaliable.Where(action => action.CurrentHP == lowest).ToList();

                    if (heroAvaliable.Count > 1)
                    {
                        int count = heroAvaliable.Count - 1;
                        for (int i = 0; i < count; i++)
                        {
                            heroAvaliable.RemoveAt(Random.Range(0, heroAvaliable.Count));
                        }
                    }
                }
            }
            else
            {
                heroAvaliable = GetUnitGroup(unitSide);
                if (heroAvaliable != null && heroAvaliable.Count > 1)
                {
                    int lowest = 1000;
                    for (int i = 0; i < heroAvaliable.Count; i++)
                    {
                        int actionAmount = heroAvaliable[i].CurrentHP;
                        if (actionAmount < lowest)
                            lowest = actionAmount;
                    }

                    heroAvaliable = heroAvaliable.Where(action => action.CurrentHP == lowest).ToList();

                    if (heroAvaliable.Count > 1)
                    {
                        int count = heroAvaliable.Count - 1;
                        for (int i = 0; i < count; i++)
                        {
                            heroAvaliable.RemoveAt(Random.Range(0, heroAvaliable.Count));
                        }
                    }
                }
            }


            if (heroAvaliable == null)
                return null;

            if (heroAvaliable.Count > 1)
                return new List<Unit>() { heroAvaliable[Random.Range(0, heroAvaliable.Count)] };
            else if (heroAvaliable.Count == 1)
                return heroAvaliable;
            else
                return null;
        }
    }
}