using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleEffects;
using Lodkod;
using System.Linq;

namespace BattleEngine
{
    public class GhoulQueenDamageRuleScript : DamageRule
    {
        public override int Damage(ActionSourceData sourceData)
        {
            if (_parent == null)
            {
                Debug.LogError("Not set parent to Damage rule");
                return sourceData.ActionAmount;
            }

            if (this._parent.EffectSystem.HasEffect(EffectType.Dodge))
            {
                return 0;
            }

            int damage = sourceData.ActionAmount;

            if (sourceData.SourceType == ActionSourceType.Card)
            {
                SkillType elementType = IOM.GetCard(sourceData.SourceID).CardSkillType;
                if (this._parent.Sustainbility != null && this._parent.Sustainbility.Count > 0)
                {
                    if (this._parent.Sustainbility.Any(sustain => sustain == elementType))
                    {
                        damage = Mathf.FloorToInt(sourceData.ActionAmount / 2);
                    }
                }

                if (this._parent.Susceptibility != null && this._parent.Susceptibility.Count > 0)
                {
                    if (this._parent.Susceptibility.Any(suscept => suscept == elementType))
                    {
                        damage = damage * 2;
                    }
                }
            }

            this._parent.CurrentHP -= damage;
            if (this._parent.CurrentHP <= 0)
                this._parent.SetDead();

            return this._parent.EffectSystem.DamageDebuuffEffect(damage);
        }
    }
}