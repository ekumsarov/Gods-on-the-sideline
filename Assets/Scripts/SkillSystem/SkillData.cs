using UnityEditor;
using UnityEngine;
using System;
using Lodkod;

// use only with Skill for project "Gods
[System.Serializable]
public class SkillData
{

    [SerializeField] private SkillType _skillType;
    public string SkillType
    {
        get { return _skillType.ToString(); }
    }

    [SerializeField] private int _skillAmount;
    public int SkillAmount
    {
        get { return _skillAmount; }
    }
}