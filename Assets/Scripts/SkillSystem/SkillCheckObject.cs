using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using SimpleJSON;

[Serializable]
public class SkillCheckObject : ObjectID
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

    // Complex - complex of skill check
    private int _complex = 0;
    public int Complex
    {
        get { return this._complex; }
    }

    // Result - amount of result
    private int _complexResult = 0;
    public int ComplexResult
    {
        get { return this._complexResult; }
    }

    public static SkillCheckObject Create(string id, int complex, int amount = 0, int maxResult = 0)
    {
        return new SkillCheckObject()
        {
            _skillID = id,
            _complex = complex
        };
    }

    public static SkillCheckObject Create(JSONNode node)
    {
        SkillCheckObject temp = new SkillCheckObject();

        if (node["id"] != null)
            temp._skillID = node["id"].Value;

        if (node["complex"] != null)
            temp._complex = node["complex"].AsInt;

        return temp;
    }

    public void CompleteCheck(iBattleUnit unit)
    {
        if(!unit.Skills.ContainsKey(this.Skill))
        {
            Debug.LogError("Unit: " + unit.Name + " has no skill: " + this.Skill);
            return;
        }

        this._complexResult = unit.Skills[this.Skill].GetValue;
    }

    public void CompleteCheck(List<Unit> unit)
    {
        if (unit.Any(uni => !uni.Skills.ContainsKey(this.Skill)))
        {
            Debug.LogError("Unit has no skill: " + this.Skill);
            return;
        }

        foreach(var uni in unit)
            this._complexResult += uni.Skills[this.Skill].GetValue;
    }

    public bool CheckResult
    {
        get
        {
            return this._complex <= this._complexResult;
        }
    }
}

public class GroupSkillCheck
{
    private List<SkillCheckObject> _skills;
    public List<SkillCheckObject> Skills
    { get { return this._skills; } }

    private bool _isComplete = false;
    public bool IsComplete
    {
        get { return _isComplete; }
    }

    private bool _success = true;
    public bool Success
    {
        get { return this._success; }
    }

    public static GroupSkillCheck Create()
    {
        return new GroupSkillCheck();
    }

    public static GroupSkillCheck Create(List<SkillCheckObject> list)
    {
        GroupSkillCheck temp = new GroupSkillCheck();
        temp._skills = new List<SkillCheckObject>();
        temp._skills.AddRange(list);

        return temp;
    }

    public GroupSkillCheck AddSkillObject(SkillCheckObject obj)
    {
        if (this._skills == null)
            this._skills = new List<SkillCheckObject>();

        this._skills.Add(obj);
        return this;
    }

    public void AddSkill(SkillCheckObject obj)
    {
        if (this._skills == null)
            this._skills = new List<SkillCheckObject>();

        this._skills.Add(obj);
    }

    public void CompleteCheck(iBattleUnit unit)
    {
        foreach(var skill in this._skills)
        {
            if (!unit.Skills.ContainsKey(skill.Skill))
            {
                Debug.LogError("Unit: " + unit.Name + " has no skill: " + skill.Skill);
                return;
            }
        }
        
        foreach(var skill in this._skills)
        {
            skill.CompleteCheck(unit);
            if(skill.CheckResult == false)
            {
                _success = false;
            }
        }

        _isComplete = true;
    }

    public void CompleteCheck(List<Unit> units)
    {
        foreach (var skill in this._skills)
        {
            foreach(var unit in units)
            if (!unit.Skills.ContainsKey(skill.Skill))
            {
                Debug.LogError("Unit: " + unit.Name + " has no skill: " + skill.Skill);
                return;
            }
        }

        foreach (var skill in this._skills)
        {
            skill.CompleteCheck(units);
            if (skill.CheckResult == false)
            {
                _success = false;
            }
        }

        _isComplete = true;
    }

    public void LogBattleData()
    {
        string forBattleLog = "Check: ";
        foreach (var skill in this._skills)
        {
            forBattleLog += string.Format("\n{0} : {1}, complex is {2}", LocalizationManager.Get(skill.Skill), skill.ComplexResult, skill.Complex);
        }

        forBattleLog += string.Format("\nSuccess: " + this.Success);

        BattleLog.PushLog(forBattleLog);
    }
}