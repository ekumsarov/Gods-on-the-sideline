using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleEngine
{
    public class UnitLogicResult
    {
        public UnitAction Action = null;
        public List<iBattleUnit> Targets = null;

        public static UnitLogicResult Create(UnitAction action, List<iBattleUnit> targets)
        {
            return new UnitLogicResult()
            {
                Action = action,
                Targets = targets
            };
        }
    }
}