using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleEngine
{
    public class HealRule : MonoBehaviour, ICloneable
    {
        protected Unit _parent;

        public void SetupParent(Unit parent)
        {
            this._parent = parent;
        }

        public virtual void Heal(int amount)
        {
            if(_parent == null)
            {
                Debug.LogError("Not set parent for heal rule");
                return;
            }

            if (this._parent.CurrentHP + amount > this._parent.HP)
            {
                this._parent.CurrentHP = this._parent.HP;
            }
            else
                this._parent.CurrentHP += amount;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}