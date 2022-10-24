using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Lodkod;
using System;

namespace BattleEngine
{
    using BattleEffects;

    public class UnitEffect
    {
        private List<BattleEffect> _effects;
        private List<BattleEffect> _removeEffectList;
        private Unit _parent;
        private BattleUnitView _view;

        public void Init(Unit unit, BattleUnitView view)
        {
            if(this._effects != null)
            {
                this._effects.Clear();
                this._effects = null;
            }

            this._effects = new List<BattleEffect>();
            this._removeEffectList = new List<BattleEffect>();

            if (_parent != null)
                _parent = null;

            _parent = unit;
            _view = view;
        }

        public void BindView(BattleUnitView view)
        {
            _view = view;
        }

        public void AddEffect(ActionSourceData data)
        {
            if(data.SourceType != ActionSourceType.Effect)
                return;

            EffectType effectType = (EffectType)Enum.Parse(typeof(EffectType), data.ActionID);

            BattleEffect applyEffect = BattleEffect.loadBattleEffect(effectType, data.ActionAmount);

            BattleResponse.Notify(data, BattleResponseActions.AppliedEffect);

            BattleEffect temp = this._effects.FirstOrDefault(seffect => seffect.EffectType == effectType);
            if (temp != null)
            {
                temp.SumEffects(applyEffect);
                applyEffect = null;
                _view.UpdateItem();
                return;
            }

            applyEffect.Object = _parent;
            this._effects.Add(applyEffect);
            applyEffect.StartEffect();
            this._view.UpdateItem();

            BattleLog.PushLog(_parent.Name + " applied effect " + applyEffect.ID + " with " + applyEffect.AmountCount + " amount");
        }

        /*public void AddEffect(BattleEffect effect)
        {
            BattleEffect temp = this._effects.FirstOrDefault(seffect => seffect.EffectType == effect.EffectType);
            if (temp != null)
            {
                temp.SumEffects(effect);
                effect = null;
                _view.UpdateItem();
                return;
            }

            effect.Object = _parent;
            this._effects.Add(effect);
            effect.StartEffect();
            this._view.UpdateItem();

            BattleLog.PushLog(_parent.Name + " applied effect " + effect.ID + " with " + effect.AmountCount + " amount");
        }*/

        public void RemoveEffect(string effectID)
        {
            this._removeEffectList.Add(this._effects.FirstOrDefault(eff => eff.ID.Equals(effectID)));
            this.EffectDestroy();
        }

        public void RemoveAllEffects()
        {
            for (int i = 0; i < this._effects.Count; i++)
            {
                this._removeEffectList.Add(this._effects[i]);
            }
            this.EffectDestroy();
        }

        public void ReduceEffectAmount(string effectID, int amount)
        {
            foreach (var effect in this._effects)
            {
                if (effect.Equals(effectID))
                {
                    effect.ReduceAmount(amount);
                    this._view.UpdateItem();
                    return;
                }
            }
        }

        public bool HasAnyEffect()
        {
            return this._effects != null && this._effects.Count > 0;
        }

        public bool HasEffect(string effectID)
        {
            if (effectID.IsNullOrEmpty())
            {
                Debug.LogError("Effect id to check is null");
                return false;
            }

            return this._effects.Any(eff => eff.ID.Equals(effectID));
        }

        public bool HasEffect(EffectType effectID)
        {
            return this._effects.Any(eff => eff.EffectType == effectID);
        }

        public void EffectTurnEnd()
        {
            for (int i = 0; i < this._effects.Count; i++)
            {
                this._effects[i].Cooldown -= 1;

                if (this._effects[i].EffectDestroyed)
                {
                    this._removeEffectList.Add(this._effects[i]);
                }
            }

            if (this._removeEffectList.Count > 0)
                this.EffectDestroy();

        }

        public void EffectDestroy()
        {
            for (int i = 0; i < this._removeEffectList.Count; i++)
            {
                this._effects.Remove(this._removeEffectList[i]);
            }
            this._removeEffectList.Clear();
            this._view.UpdateItem();
        }

        public bool EffectCancelAction(string action)
        {
            for (int i = 0; i < this._effects.Count; i++)
            {
                if (this._effects[i].CancelAction(action))
                    return true;
            }

            return false;
        }

        public BattleEffect GetEffect(string id)
        {
            for (int i = 0; i < this._effects.Count; i++)
            {
                if (this._effects[i].ID.Equals(id))
                    return this._effects[i];
            }

            return null;
        }

        public void SetupEffectViews(List<BattleEffectItem> views)
        {
            for (int i = 0; i < views.Count; i++)
            {
                if (i < this._effects.Count && this._effects[i].IsVisible)
                {
                    views[i].SetEffect(this._effects[i]);
                    views[i].Visible = true;
                }
                else
                {
                    views[i].Visible = false;
                }
            }
        }

        public List<IBattleFunction> GetEffectActions()
        {
            return this._effects.Cast<IBattleFunction>().ToList();
        }

        public int DamageBuffEffect(int amount)
        {
            BattleEffect temp = this._effects.FirstOrDefault(effect => effect.EffectType == Lodkod.EffectType.DamageBuff);
            if(temp != null)
            {
                amount += temp.AmountCount;
            }

            temp = this._effects.FirstOrDefault(effect => effect.EffectType == Lodkod.EffectType.DamageDebuff);
            if (temp != null)
            {
                amount -= temp.AmountCount;
            }

            /*temp = this._effects.FirstOrDefault(effect => effect.EffectType == Lodkod.EffectType.DamageBuff);
            if(temp != null)
            {
                amount = temp.EffectOnDamage(amount);
            }*/
            
            return amount;
        }

        // Rebuild this Function
        // Seperate from debuff from source and appliyng damage to target
        // this is bug
        public int DamageDebuuffEffect(int amount)
        {
            BattleEffect temp = this._effects.FirstOrDefault(effect => effect.EffectType == Lodkod.EffectType.Shock);

            if (temp != null)
            {
                amount = temp.EffectOnDamage(amount);
            }

            temp = this._effects.FirstOrDefault(effect => effect.EffectType == Lodkod.EffectType.Blindless);

            if (temp != null)
            {
                if (temp.AmountCount * 10 >= UnityEngine.Random.Range(0, 101))
                    return 0;
            }

            temp = this._effects.FirstOrDefault(effect => effect.EffectType == Lodkod.EffectType.Defense);
            if(temp != null)
            {
                if(amount >= temp.AmountCount)
                {
                    amount -= temp.AmountCount;
                    temp.ReduceAmount(temp.AmountCount);
                }
                else
                {
                    int diff = temp.AmountCount - amount;
                    amount = 0;
                    temp.ReduceAmount(temp.AmountCount - diff);
                }
            }

            return amount;
        }
    }

}
