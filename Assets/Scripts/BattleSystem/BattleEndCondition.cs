using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Reflection;
using System.Linq;

namespace BattleEngine
{
    public class BattleEndCondition : ICloneable
    {
        public virtual bool Available
        {
            get { return true; }
        }

        #region InitAllBattleEndConditions
        private static Dictionary<string, Type> assets;
        public static Dictionary<string, BattleEndCondition> packs;

        public static void initAllPacks()
        {
            assets = new Dictionary<string, Type>();
            packs = new Dictionary<string, BattleEndCondition>();

            getAllAssets();

            BattleEndCondition res = null;

            foreach (var key in assets.Keys)
            {
                try
                {
                    res = Activator.CreateInstance(assets[key]) as BattleEndCondition;
                    packs.Add(key, res);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Error reading script " + key);
                    Debug.LogError(e.Message);
                }
            }
        }

        public static BattleEndCondition loadAssets(string node)
        {
            BattleEndCondition res = null;

            if (assets == null)
            {
                getAllAssets();
            }

            if (packs.ContainsKey(node))
            {
                res = packs[node].Clone() as BattleEndCondition;
                return res;

            }

            if (assets.ContainsKey(node))
            {
                try
                {
                    res = Activator.CreateInstance(assets[node]) as BattleEndCondition;
                    packs.Add(node, res);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Error reading script " + node);
                    Debug.LogError(e.Message);
                }
            }

            if (packs.ContainsKey(node))
            {
                res = packs[node].Clone() as BattleEndCondition;
            }


            if (res == null)
            {
                Debug.LogError("Failed to load event! ID: " + node);
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
                    if (t.IsSubclassOf(typeof(BattleEndCondition)))
                    {
                        string typeName = t.ToString();
                        string shortName;
                        if (typeName.IndexOf("BattleEndCondition.") == 0)
                        {
                            shortName = typeName.Substring("BattleEndCondition.".Length);
                            Debug.Log(shortName);
                            assets[shortName] = t;
                        }
                    }
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}