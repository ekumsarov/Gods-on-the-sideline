using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Lodkod;

public class SkillObject : ObjectID
{
    #region ID
    string ObjectID = "nil";

    public string ID
    {
        get { return ObjectID; }
        set { this.ObjectID = value; }
    }
    #endregion

    // SkillID - the name of skill
    string _skillID = "nill";
    public string Skill
    {
        get { return this._skillID; }
        set { this._skillID = value; }
    }

    // MinValue - the min value of skill
    private int savedMinMinusChanged = 0;
    int _minValue = 0;
    public int Min
    {
        get { return this._minValue; }
    }

    // MaxValue - the max value of skill
    private int savedMaxMinusChanged = 0;
    int _maxValue = 0;
    public int Max
    {
        get { return this._maxValue; }
    }

    public int GetValue
    {
        get
        {
            if (this._maxValue <= 0)
                return 0;
            else if (this._maxValue <= this._minValue)
                return this._minValue;
            else
                return UnityEngine.Random.Range(this._minValue, this._maxValue + 1);
        }
    }

    public void SetupSkill(int min, int max)
    {
        this._minValue = min;
        this._maxValue = max;
    }

    public void SetupSkill(SkillObject skill)
    {
        this._minValue = skill.Min;
        this._maxValue = skill.Max;
    }

    public void SetSkill(int min, int max)
    {
        this._minValue += min;
        // return minus of min value
        this._minValue += this.savedMinMinusChanged;
        this.savedMinMinusChanged = 0;

        this._maxValue += max;
        // return max of min value
        this._maxValue += this.savedMaxMinusChanged;
        this.savedMaxMinusChanged = 0;
    }

    public void SetSkill(SkillObject skill)
    {
        this._minValue += skill.Min;
        // return minus of min value
        this._minValue += this.savedMinMinusChanged;
        this.savedMinMinusChanged = 0;

        this._maxValue += skill.Max;
        // return max of min value
        this._maxValue += this.savedMaxMinusChanged;
        this.savedMaxMinusChanged = 0;
    }

    public void RemoveSkills(SkillObject skill)
    {
        this._minValue -= skill.Min;
        if (this._minValue < 0)
        {
            this.savedMinMinusChanged = this._minValue;
            this._minValue = 0;
        }

        this._maxValue -= skill.Max;
        if (this._maxValue < 0)
        {
            this.savedMaxMinusChanged = this._maxValue;
            this._maxValue = 0;
        }
    }

    public void RemoveSkills(int min, int max)
    {
        this._minValue -= min;
        if (this._minValue < 0)
        {
            this.savedMinMinusChanged = this._minValue;
            this._minValue = 0;
        }

        this._maxValue -= max;
        if (this._maxValue < 0)
        {
            this.savedMaxMinusChanged = this._maxValue;
            this._maxValue = 0;
        }
    }

    public string GetSkillString()
    {
        if (this._maxValue == this._minValue)
            return this._minValue.ToString();
        else if (this._maxValue == 0)
            return "0";
        else
            return string.Format("{0} - {1}", this._minValue, this._maxValue);
    }

    public static SkillObject Make(string id, int min, int max)
    {
        return new SkillObject()
        {
            ID = id,
            _skillID = id,
            _minValue = min,
            _maxValue = max
        };
    }

    public static SkillObject Make(SkillDataFull data)
    {
        return new SkillObject()
        {
            ID = data.SkillType,
            _skillID = data.SkillType,
            _minValue = data.SkillMin,
            _maxValue = data.SkillMax
        };
    }

    public static SkillObject Make(SkillObject skill)
    {
        return new SkillObject()
        {
            ID = skill.Skill,
            _skillID = skill.Skill,
            _minValue = skill.Min,
            _maxValue = skill.Max
        };
    }

    public static SkillObject Make(JSONNode data)
    {
        if (data["id"] == null || data["min"] == null || data["max"] == null)
        {
            Debug.LogError("Cannot read skill data");
            return null;
        }
            

        return new SkillObject()
        {
            ID = data["id"].Value,
            _skillID = data["id"].Value,
            _minValue = data["min"].AsInt,
            _maxValue = data["max"].AsInt
        };
    }

    public static string SkillIcon(string id)
    {
        if (id.Equals("light") || id.Equals("Light"))
            return "LightSkillIcon";
        else if (id.Equals("dark") || id.Equals("Dark"))
            return "DarkSkillIcon";
        else if (id.Equals("natural") || id.Equals("Natural"))
            return "NaturalSkillIcon";
        else if (id.Equals("spirit") || id.Equals("Spirit"))
            return "SpiritSkillIcon";
        else if (id.Equals("craft") || id.Equals("Craft"))
            return "CraftSkillIcon";
        else if (id.Equals("elementals") || id.Equals("Elementals"))
            return "ElementalsSkillIcon";
        else
            return "BaseIcon";
    }

    public static string SkillIcon(SkillType type)
    {
        return SkillIcon(type.ToString());
    }

    public static string SkillShortName(string id)
    {
        if (id.Equals("light") || id.Equals("Light"))
            return LocalizationManager.Get("SkillLabelLight");
        else if (id.Equals("dark") || id.Equals("Dark"))
            return LocalizationManager.Get("SkillLabelDark");
        else if (id.Equals("natural") || id.Equals("Natural"))
            return LocalizationManager.Get("SkillLabelNature");
        else if (id.Equals("spirit") || id.Equals("Spirit"))
            return LocalizationManager.Get("SkillLabelSpirit");
        else if (id.Equals("craft") || id.Equals("Craft"))
            return LocalizationManager.Get("SkillLabelCraft");
        else if (id.Equals("elementals") || id.Equals("Elementals"))
            return LocalizationManager.Get("SkillLabelElemental");
        else
            return LocalizationManager.Get("NoText");
    }

    public  string SkillFullName()
    {
        if (this.ID.Equals("strenght"))
            return LocalizationManager.Get("strenghtFullSRT");
        else if (this.ID.Equals("dexterity"))
            return LocalizationManager.Get("dexterityFullSRT");
        else if (this.ID.Equals("stamina"))
            return LocalizationManager.Get("staminaFullSRT");
        else if (this.ID.Equals("intelligence"))
            return LocalizationManager.Get("intelligenceFullSRT");
        else if (this.ID.Equals("charisma"))
            return LocalizationManager.Get("charismaFullSRT");
        else
            return LocalizationManager.Get("strenghtFullSRT");
    }

    public static string SkillFullName(string id)
    {
        if (id.Equals("light") || id.Equals("Light"))
            return LocalizationManager.Get("SkillLabelLight");
        else if (id.Equals("dark") || id.Equals("Dark"))
            return LocalizationManager.Get("SkillLabelDark");
        else if (id.Equals("natural") || id.Equals("Natural"))
            return LocalizationManager.Get("SkillLabelNature");
        else if (id.Equals("spirit") || id.Equals("Spirit"))
            return LocalizationManager.Get("SkillLabelSpirit");
        else if (id.Equals("craft") || id.Equals("Craft"))
            return LocalizationManager.Get("SkillLabelCraft");
        else if (id.Equals("elementals") || id.Equals("Elementals"))
            return LocalizationManager.Get("SkillLabelElemental");
        else
            return LocalizationManager.Get("strenghtFullSRT");
    }

    public static string SkillFullName(SkillType type)
    {
        return SkillFullName(type.ToString());
    }
}
