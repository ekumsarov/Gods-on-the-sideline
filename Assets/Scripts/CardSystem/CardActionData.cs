using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardActionData 
{
    public enum ActionTarget { FirstTarget, SelectedTarget, SelectedFirstTarget, SelectedTargets, AllNonSelected, NonSelectedTarget, Group }

    [SerializeField] private List<SkillData> Skills;
    [SerializeField] private List<CardActionTypes> Actions;
    
    public List<SkillData> SkillsData
    {
        get { return this.Skills; }
    }

    public List<CardActionTypes> ActionsData
    {
        get { return this.Actions; }
    }
}
