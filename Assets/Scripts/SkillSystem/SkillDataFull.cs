using UnityEditor;
using UnityEngine;
using System;
using Lodkod;

// use only with Skill for project "Gods
[System.Serializable]
public class SkillDataFull
{

    [SerializeField] private SkillType _skillType;
    public string SkillType
    {
        get { return _skillType.ToString(); }
    }

    [SerializeField] private int _skillMin;
    public int SkillMin
    {
        get { return _skillMin; }
    }

    [SerializeField] private int _skillMax;
    public int SkillMax
    {
        get { return _skillMax; }
    }
}