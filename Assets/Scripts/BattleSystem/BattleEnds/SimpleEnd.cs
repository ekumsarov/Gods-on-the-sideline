using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BattleEngine
{
    public class SimpleEnd : BattleEndCondition
{
        public override bool Available
        {
            get
            {
                if (BattleSystem.Heroes.Any(hero => hero.IsActive) && BattleSystem.Enemies.Any(enemy => enemy.IsActive))
                    return false;

                return true;
            }
        }
    }
}

