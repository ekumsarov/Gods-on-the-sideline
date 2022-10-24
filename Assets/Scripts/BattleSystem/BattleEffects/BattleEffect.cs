using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.IO;
using SimpleJSON;
using Lodkod;
using System.Reflection;
using BattleEngine;

namespace BattleEffects
{

    public class BattleEffect : ObjectID, ICloneable, IBattleFunction
    {
        #region properties
        string ObjectID;

        iBattleUnit _object;
        public iBattleUnit Object
        {
            get { return this._object; }
            set { this._object = value; }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public string ID
        {
            get { return ObjectID; }
            set { this.ObjectID = value; }
        }

        protected bool initialized = false;
        protected BattleEffectInfo data;

        protected int _amount = 0;
        public int AmountCount => _amount;

        protected EffectType _effectType;
        public EffectType EffectType => _effectType;

        protected EffectBuffType _buffType = EffectBuffType.Negative;
        public EffectBuffType BuffType => _buffType;

        protected bool _isVisible = true;
        public bool IsVisible => _isVisible;

        protected bool _viewVisibility = true;
        public bool ViewVisibility => this._viewVisibility;

        private bool cooldownOn = false;
        private int _cooldownCount = -1;
        public int Cooldown
        {
            get { return this._cooldownCount; }
            set
            {
                this._cooldownCount = value;
                if (this._cooldownCount <= 0 && cooldownOn)
                    this.DestroyEffect();
            }
        }

        protected int SetCooldown
        {
            set
            {
                if (value <= 0)
                    return;

                this._cooldownCount = value;
                this.cooldownOn = true;
            }
        }

        public bool EffectDestroyed = false;
        #endregion

        #region Effect Info

        protected string _ico = "info";
        public string Icon
        { get { return this._ico; } }

        protected string _desctiprion = "info";
        public string Description
        { get { return this._desctiprion; } }

        protected List<string> _actionsToCancel;
        public bool CancelAction(string action)
        {
            if (this._actionsToCancel == null)
                return false;

            return this._actionsToCancel.Any(act => act.Equals(action));
        }

        #endregion

        #region Sub Work

        public virtual void Init()
        {
            this._actionsToCancel = new List<string>();

            this.data = IOM.GetEffect(this.ID);

            if (this.data.actToCancel != null && this.data.actToCancel.Count > 0)
                this._actionsToCancel.AddRange(this.data.actToCancel);

            this.initialized = true;
        }

        #endregion

        #region override functions

        public virtual void StartEffect()
        {
        }

        public virtual bool CanApplyEffect()
        {
            return true;
        }

        public virtual void PrepareBattleEffect(CardActionTypes data)
        {
        }

        public virtual void PrepareBattleEffect(int amount)
        {
        }

        protected virtual void TurnStart()
        {
            TurnEnd();
        }

        private Action _callback;
        public void StartFunction(Action callback)
        {
            _callback = callback;
            TurnStart();
        }

        public void Complete()
        {
            _callback?.Invoke();
        }

        public virtual int EffectOnDamage(int damage)
        {
            return damage;
        }

        protected virtual void TurnEnd()
        {
            if (this.EffectDestroyed)
            {
                Complete();
                return;
            }

            // Remove this part
            
            if(this.cooldownOn == false)
            {
                this._amount -= 1;
                if (this._amount <= 0)
                    this.DestroyEffect();
            }

            Complete();
        }

        public virtual void DestroyEffect()
        {
            ES.NotifySubscribers(TriggerType.EffectDestroyed.ToString(), this.ID);
            this.EffectDestroyed = true;
        }

        public virtual void SumEffects(BattleEffect effect)
        {
            this._amount += effect._amount;
        }

        public virtual void ReduceAmount(int amount)
        {
            this._amount -= amount;
            if (this._amount < 0)
                this._amount = 0;
        }

        public virtual string GetDescription()
        {
            return LocalizationManager.Get("test_string");
        }

        public virtual string GetEffectActiveMessage()
        {
            return LocalizationManager.Get("test_string");
        }

        #endregion

        #region EffectData

        public static string GetEffectIcon(string effectID)
        {
            if(effectID.Equals(EffectType.Fire.ToString()))
            {
                return "fire";
            }
            else if(effectID.Equals(EffectType.Dodge.ToString()))
            {
                return "dodge";
            }
            else if (effectID.Equals(EffectType.Shock.ToString()))
            {
                return "shock";
            }
            else if (effectID.Equals(EffectType.Stunn.ToString()))
            {
                return "stunn";
            }
            else if (effectID.Equals(EffectType.Poison.ToString()))
            {
                return "posion";
            }
            else if (effectID.Equals(EffectType.Defense.ToString()))
            {
                return "defense";
            }
            else if (effectID.Equals(EffectType.Bleeding.ToString()))
            {
                return "bleeding";
            }

            return "simple";
        }

        #endregion

        #region InitAllBattleEffects
        public static EffectBuffType GetBuffType(string effectType)
        {
            if (effectType.Equals("Defense") || effectType.Equals("Dodge") || effectType.Equals("Power") || effectType.Equals("Provocation"))
                return EffectBuffType.Positive;

            return EffectBuffType.Negative;
        }

        public static EffectBuffType GetBuffType(EffectType effectType)
        {
            if (effectType.Equals("Defense") || effectType.Equals("Dodge") || effectType.Equals("Power") || effectType.Equals("Provocation"))
                return EffectBuffType.Positive;

            return EffectBuffType.Negative;
        }

        private static Dictionary<string, Type> assets;
        public static Dictionary<string, BattleEffect> packs;

        public static void initAllPacks()
        {
            assets = new Dictionary<string, Type>();
            packs = new Dictionary<string, BattleEffect>();

            getAllAssets();

            BattleEffect res = null;

            foreach (var key in assets.Keys)
            {
                try
                {
                    res = Activator.CreateInstance(assets[key]) as BattleEffect;
                    res.ID = key;
                    res.Init();
                    packs.Add(key, res);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Error reading script " + key);
                    Debug.LogError(e.Message);
                }
            }
        }

        public static BattleEffect loadBattleEffect(CardActionTypes data)
        {
            BattleEffect res = null;

            if (assets == null)
            {
                getAllAssets();
            }

            // for search by spec id
            if(data.dEffectType == EffectType.BySpecID)
            {
                if(packs.ContainsKey(data.dEffectSpecID))
                {
                    res = packs[data.dEffectType.ToString()].Clone() as BattleEffect;
                    res.PrepareBattleEffect(data);
                    return res;
                }

                if (assets.ContainsKey(data.dEffectSpecID))
                {
                    try
                    {
                        res = Activator.CreateInstance(assets[data.dEffectSpecID]) as BattleEffect;
                        res.ID = data.dEffectSpecID;
                        packs.Add(data.dEffectSpecID, res);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("Error reading script " + data.dEffectType.ToString());
                        Debug.LogError(e.Message);
                    }
                }

                if (packs.ContainsKey(data.dEffectSpecID))
                {
                    res = packs[data.dEffectSpecID].Clone() as BattleEffect;
                    res.PrepareBattleEffect(data);
                }


                if (res == null)
                {
                    Debug.LogError("Failed to load event! ID: " + data.dEffectSpecID);
                }

                return res;
            }

            if (packs.ContainsKey(data.dEffectType.ToString()))
            {
                res = packs[data.dEffectType.ToString()].Clone() as BattleEffect;
                res.PrepareBattleEffect(data);
                return res;

            }

            if (assets.ContainsKey(data.dEffectType.ToString()))
            {
                try
                {
                    res = Activator.CreateInstance(assets[data.dEffectType.ToString()]) as BattleEffect;
                    res.ID = data.dEffectType.ToString();
                    packs.Add(data.dEffectType.ToString(), res);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Error reading script " + data.dEffectType.ToString());
                    Debug.LogError(e.Message);
                }
            }

            if (packs.ContainsKey(data.dEffectType.ToString()))
            {
                res = packs[data.dEffectType.ToString()].Clone() as BattleEffect;
                res.PrepareBattleEffect(data);
            }


            if (res == null)
            {
                Debug.LogError("Failed to load event! ID: " + data.dEffectType.ToString());
            }

            return res;
        }

        public static BattleEffect loadBattleEffect(EffectType type, int amount)
        {
            BattleEffect res = null;

            if (assets == null)
            {
                getAllAssets();
            }

            if (packs.ContainsKey(type.ToString()))
            {
                res = packs[type.ToString()].Clone() as BattleEffect;
                res.PrepareBattleEffect(amount);
                return res;

            }

            if (assets.ContainsKey(type.ToString()))
            {
                try
                {
                    res = Activator.CreateInstance(assets[type.ToString()]) as BattleEffect;
                    res.ID = type.ToString();
                    packs.Add(type.ToString(), res);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Error reading script " + type.ToString());
                    Debug.LogError(e.Message);
                }
            }

            if (packs.ContainsKey(type.ToString()))
            {
                res = packs[type.ToString()].Clone() as BattleEffect;
                res.PrepareBattleEffect(amount);
            }


            if (res == null)
            {
                Debug.LogError("Failed to load event! ID: " + type.ToString());
            }

            return res;
        }

        static void getAllAssets()
        {
            Assembly[] asset = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .ToArray();
            foreach (var a in asset)
            {
                foreach (var t in a.GetExportedTypes())
                    if (t.IsSubclassOf(typeof(BattleEffects.BattleEffect)))
                    {
                        string typeName = t.ToString();
                        string shortName;
                        if (typeName.IndexOf("BattleEffects.") == 0)
                        {
                            shortName = typeName.Substring("BattleEffects.".Length);
                            Debug.Log(shortName);
                            assets[shortName] = t;
                        }
                        else if (typeName.IndexOf("BattleEffects" + GM.missionPath + ".") == 0)
                        {
                            shortName = typeName.Substring(string.Concat("BattleEffects.", GM.missionPath).Length);
                            Debug.Log(shortName);
                            assets[shortName] = t;
                        }
                    }
            }
        }
        #endregion
    }

}