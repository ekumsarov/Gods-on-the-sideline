using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lodkod;
using BattleEffects;
using System.Linq;

namespace BattleEngine
{
    public class DamageRule : MonoBehaviour, ICloneable
    {
        protected Unit _parent;

        public void SetupParent(Unit parent)
        {
            this._parent = parent;
        }

        public virtual int Damage(ActionSourceData sourceData)
        {
            if(_parent == null)
            {
                Debug.LogError("Not set parent to Damage rule");
                return sourceData.ActionAmount;
            }

            if(this._parent.EffectSystem.HasEffect(EffectType.Dodge))
            {
                this._parent.EffectSystem.ReduceEffectAmount(EffectType.Dodge.ToString(), 3);
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

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}