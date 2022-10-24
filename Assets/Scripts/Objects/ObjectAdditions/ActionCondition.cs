using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using SimpleJSON;
using Lodkod;
using BattleEngine;

public class Condition
{
    public virtual bool Available
    {
        get { return true; }
    }

    public virtual string Text
    {
        get { return ""; }
    }

    public virtual List<iBattleUnit> GetUnitsFromCondition(List<iBattleUnit> avaliables)
    {
        return avaliables;
    }

    protected ConditionType _conType = ConditionType.NoType;
    public ConditionType ConditionType
    {
        get { return this._conType; }
    }

    public static Condition GetCondition(ConditionData data)
    {
        if(data.Type == ConditionType.HeroRequire)
        {
            return HeroRequierCondition.Make(data);
        }

        if(data.Type == ConditionType.EffectCondition)
        {
            return EffectCondition.Make(data);
        }

        return new Condition();
    }

    public static List<Condition> GetConditions(List<ConditionData> data)
    {
        List<Condition> temp = new List<Condition>();

        for(int i = 0; i < data.Count; i++)
        {
            temp.Add(Condition.GetCondition(data[i]));
        }

        return temp;
    }
}

public class FunctionCondition : Condition
{
    public delegate bool CheckCondition();
    CheckCondition _function;

    public override bool Available
    {
        get
        {
            if (_function == null)
                return false;
            else
                return _function();
        }
    }

    public static FunctionCondition Create(CheckCondition function)
    {
        return new FunctionCondition() { _function = function };
    }
}

public class StatCondition : Condition
{
    public enum StatConType { Equal, NotEqual, Less, More }

    List<iStat> stats;
    StatConType type;

    public List<iStat> GetStats
    {
        get { return this.stats; }
    }

    public override bool Available
    {
        get
        {
            bool avaliable = true;
            foreach(var stat in stats)
            {
                if (!SM.Stats.ContainsKey(stat.type))
                {
                    Debug.LogError("No Stat type: " + stat.type);
                    return false;
                }

                if (type == StatConType.Equal)
                    avaliable = SM.Stats[stat.type].Count == stat.amount;
                else if(type == StatConType.NotEqual)
                    avaliable = SM.Stats[stat.type].Count != stat.amount;
                else if (type == StatConType.Less)
                    avaliable = SM.Stats[stat.type].Count <= stat.amount;
                else if (type == StatConType.More)
                    avaliable = SM.Stats[stat.type].Count >= stat.amount;

                if(avaliable == false)
                {
                    //UIM.ShowTooltip(UIM.ScreenCenter, TooltipFit.Auto, TooltipTimeMode.Tootip, TooltipFillMode.Instantly, TooltipObject.UI, string.Format("Не хватает ресурсов для этого действия. Нужно {0}{1}", stat.amount, stat.type), lSize:35, time:2f);
                    return avaliable;
                }
            }

            return avaliable;
        }
    }

    public override string Text
    {
        get
        {
            bool avaliable = true;
            string text = string.Empty;
            foreach (var stat in stats)
            {
                if (!SM.Stats.ContainsKey(stat.type))
                {
                    Debug.LogError("No Stat type: " + stat.type);
                    return string.Format("Не такого ресурса " + stat.type);
                }

                if (type == StatConType.Equal)
                    avaliable = SM.Stats[stat.type].Count == stat.amount;
                else if (type == StatConType.NotEqual)
                    avaliable = SM.Stats[stat.type].Count != stat.amount;
                else if (type == StatConType.Less)
                    avaliable = SM.Stats[stat.type].Count <= stat.amount;
                else if (type == StatConType.More)
                    avaliable = SM.Stats[stat.type].Count >= stat.amount;

                if (avaliable == false)
                {
                    return string.Format("Не хватает ресурсов для этого действия. Нужно {0}{1}", stat.amount, stat.type);
                }
            }

            return text;
        }
    }

    public static StatCondition Make(iStat sta, StatConType type = StatConType.Equal)
    {
        return new StatCondition
        {
            stats = new List<iStat>
            {
                sta
            },
            type = type,
            _conType = ConditionType.Stat
        };
    }

    public static StatCondition Make(string type, int amount, StatConType stype = StatConType.Equal)
    {
        return new StatCondition
        {
            stats = new List<iStat>
            {
                iStat.Create(type, amount)
            },
            type = stype,
            _conType = ConditionType.Stat
        }; 
    }

    public static StatCondition Make(JSONNode nod, StatConType type = StatConType.Equal)
    {
        if (nod == null)
            return null;

        if (nod["ConditionID"] == null || nod["Value"] == null || nod["QualityType"] == null)
            return null;

        return StatCondition.Make(nod["ConditionID"].Value, nod["Value"].AsInt, (StatConType)Enum.Parse(typeof(StatConType), nod["QualityType"].Value));
    }
}

public class DaypartCondition : Condition
{
    public enum StatConType { Equal, NotEqual, Less, More }

    DayPart dayPart;
    StatConType type;

    public override bool Available
    {
        get
        {
            if (type == StatConType.Equal)
                return TM.DayPart == dayPart;
            else if (type == StatConType.NotEqual)
                return TM.DayPart != dayPart;
            else if (type == StatConType.Less)
                return TM.DayPart <= dayPart;
            else if (type == StatConType.More)
                return TM.DayPart >= dayPart;

            return true;
        }
    }

    public override string Text
    {
        get { return ""; }
    }

    public static DaypartCondition Make(DayPart sta, StatConType type = StatConType.Equal)
    {
        return new DaypartCondition
        {
            dayPart = sta,
            type = type
        };
    }

    public static DaypartCondition Make(JSONNode nod, StatConType type = StatConType.Equal)
    {
        if (nod == null)
            return null;

        if (nod["DayPart"] == null || nod["QualityType"] == null)
        {
            Debug.LogError("Cannot create DayPartCondition");
            return null;
        }
            

        return DaypartCondition.Make((DayPart)Enum.Parse(typeof(DayPart), nod["DayPart"].Value), (StatConType)Enum.Parse(typeof(StatConType), nod["QualityType"].Value));
    }
}

public class FlagCondition : Condition
{

    Dictionary<string, bool> flags;

    public override bool Available
    {
        get
        {
            foreach (var stat in flags)
            {
                if (SM.CheckFlag(stat.Key) != stat.Value)
                    return false;
            }

            return true;
        }
    }

    public override string Text
    {
        get { return ""; }
    }

    public static FlagCondition Make(string fl, bool bFlag = true)
    {
        SM.AddFlag(fl);

        return new FlagCondition
        {
            flags = new Dictionary<string, bool>
            {
                { fl, bFlag }
            }
        };
    }

    public static FlagCondition Make(Dictionary<string, bool> _fl)
    {
        foreach(var ftl in _fl)
            SM.AddFlag(ftl.Key);

        return new FlagCondition
        {
            flags = _fl
        };
    }

    public static FlagCondition Make(JSONNode nod)
    {
        if (nod == null)
            return null;

        if (nod["ConditionID"] != null)
        {
            SM.AddFlag(nod["ConditionID"].Value);
            return FlagCondition.Make(nod["ConditionID"].Value, nod["isOn"].AsBool);
        }

        JSONArray arr = nod["flags"].AsArray;
        if (arr == null)
            return null;

        FlagCondition temp = new FlagCondition();
        temp.flags = new Dictionary<string, bool>();

        for (int i = 0; i < arr.Count; i++)
        {
            SM.AddFlag(arr[i].Value);
            temp.flags.Add(arr[i]["key"].Value, arr[i]["bool"].AsBool);
        }
            

        return temp;
    }
}

public class QuestCondition : Condition
{

    Dictionary<string, bool> flags;

    public override bool Available
    {
        get
        {
            foreach (var stat in flags)
            {
                if (QS.IsComplete(stat.Key) != stat.Value)
                    return false;
            }

            return true;
        }
    }

    public override string Text
    {
        get { return ""; }
    }

    public static QuestCondition Make(string fl, bool bFlag = true)
    {
        return new QuestCondition
        {
            flags = new Dictionary<string, bool>
            {
                { fl, bFlag }
            }
        };
    }

    public static QuestCondition Make(Dictionary<string, bool> _fl)
    {
        return new QuestCondition
        {
            flags = _fl
        };
    }

    public static QuestCondition Make(JSONNode nod)
    {
        if (nod == null)
            return null;

        if (nod["quest"] != null)
        {
            return QuestCondition.Make(nod["quest"].Value);
        }

        JSONArray arr = nod["quests"].AsArray;
        if (arr == null)
            return null;

        QuestCondition temp = new QuestCondition();
        temp.flags = new Dictionary<string, bool>();

        for (int i = 0; i < arr.Count; i++)
        {
            temp.flags.Add(arr[i]["key"].Value, arr[i]["bool"].AsBool);
        }


        return temp;
    }
}

public class AvaliableQuestCondition : Condition
{

    Dictionary<string, bool> flags;

    public override bool Available
    {
        get
        {
            foreach (var stat in flags)
            {
                if (QS.HasQuest(stat.Key) != stat.Value)
                    return false;
            }

            return true;
        }
    }

    public override string Text
    {
        get { return ""; }
    }

    public static AvaliableQuestCondition Make(string fl, bool bFlag = true)
    {
        return new AvaliableQuestCondition
        {
            flags = new Dictionary<string, bool>
            {
                { fl, bFlag }
            }
        };
    }

    public static AvaliableQuestCondition Make(Dictionary<string, bool> _fl)
    {
        return new AvaliableQuestCondition
        {
            flags = _fl
        };
    }

    public static AvaliableQuestCondition Make(JSONNode nod)
    {
        if (nod == null)
            return null;

        if (nod["quest"] != null)
        {
            return AvaliableQuestCondition.Make(nod["quest"].Value);
        }

        JSONArray arr = nod["quests"].AsArray;
        if (arr == null)
            return null;

        AvaliableQuestCondition temp = new AvaliableQuestCondition();
        temp.flags = new Dictionary<string, bool>();

        for (int i = 0; i < arr.Count; i++)
        {
            temp.flags.Add(arr[i]["key"].Value, arr[i]["bool"].AsBool);
        }


        return temp;
    }
}

public class HeroRequierCondition : Condition
{

    List<string> _heroes;
    bool _toHas;

    public override List<iBattleUnit> GetUnitsFromCondition(List<iBattleUnit> avaliables)
    {
        List<iBattleUnit> temp = new List<iBattleUnit>();

        foreach(var unit in avaliables)
        {
            if (_heroes.Any(hero => hero.Equals(unit.Name)) == _toHas)
                temp.Add(unit);
                
        }

        return temp;
    }

    public override bool Available
    {
        get
        {
            List<iBattleUnit> avaliables = BattleSystem.GetAllAlive();
            foreach (var unit in _heroes)
            {
                if (avaliables.Any(uni => uni.Name.Equals(unit)) != _toHas)
                    return false;
            }

            return true;
        }
    }

    public override string Text
    {
        get { return ""; }
    }

    public static HeroRequierCondition Make(ConditionData data)
    {
        return new HeroRequierCondition
        {
            _heroes = data.ConditionIDs,
            _toHas = data.ConditionToHas
        };
    }
}

public class EffectCondition : Condition
{
    List<EffectType> effects;
    bool condition;

    public override List<iBattleUnit> GetUnitsFromCondition(List<iBattleUnit> avaliables)
    {
        List<iBattleUnit> temp = new List<iBattleUnit>();

        foreach (var unit in avaliables)
        {
            for(int i = 0; i < this.effects.Count; i++)
            {
                if (unit.EffectSystem.HasEffect(this.effects[i].ToString()) == condition)
                    temp.Add(unit);
            }
        }

        return temp;
    }

    public override bool Available
    {
        get
        {
            bool avaliable = true;
            List<iBattleUnit> avaliables = BattleSystem.GetAllAlive();

            for (int i = 0; i < effects.Count; i++)
            {
                if (avaliables.Any(unit => unit.EffectSystem.HasEffect(effects[i].ToString()) != condition))
                    return false;
            }

            return avaliable;
        }
    }

    public override string Text
    {
        get
        {
            string text = string.Empty;
            return text;
        }
    }

    public static EffectCondition Make(ConditionData data)
    {
        EffectCondition temp = new EffectCondition();

        temp.condition = data.ConditionToHas;
        temp.effects = data.EffectIDs;

        return temp;
    }
}

public class LootCondition : Condition
{

    Dictionary<string, bool> flags;

    public override bool Available
    {
        get
        {
            foreach (var stat in flags)
            {
                if (LS.HasItem(stat.Key) != stat.Value)
                    return false;
            }

            return true;
        }
    }

    public override string Text
    {
        get { return ""; }
    }

    public static LootCondition Make(string fl, bool bFlag = true)
    {
        return new LootCondition
        {
            flags = new Dictionary<string, bool>
            {
                { fl, bFlag }
            }
        };
    }

    public static LootCondition Make(Dictionary<string, bool> _fl)
    {
        return new LootCondition
        {
            flags = _fl
        };
    }

    public static LootCondition Make(JSONNode nod)
    {
        if (nod == null)
            return null;

        if (nod["ConditionID"] != null)
        {
            return LootCondition.Make(nod["ConditionID"].Value, nod["isOn"].AsBool);
        }

        JSONArray arr = nod["flags"].AsArray;
        if (arr == null)
            return null;

        LootCondition temp = new LootCondition();
        temp.flags = new Dictionary<string, bool>();

        for (int i = 0; i < arr.Count; i++)
        {
            temp.flags.Add(arr[i]["key"].Value, arr[i]["bool"].AsBool);
        }


        return temp;
    }
}