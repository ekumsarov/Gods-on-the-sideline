using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleEngine
{
    public interface IDamageEffectable
    {
        public int EffectOnDamage(int amount);
    }

    public class PoisonDamageEffect : IDamageEffectable
    {
        public int EffectOnDamage(int amount)
        {
            if (amount <= 0)
                return 0;

            return 0;
        }
    }
}