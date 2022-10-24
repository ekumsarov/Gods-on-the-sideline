using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleEngine
{
    public class GhoulHealRule : HealRule
    {
        public override void Heal(int amount)
        {
            if (_parent == null)
            {
                Debug.LogError("Not set parent for heal rule");
                return;
            }

            this._parent.CurrentHP += amount;
        }
    }
}