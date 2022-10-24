using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleEngine;
using System.Linq;

public class CardActionGroup 
{
    GroupSkillCheck _check;
    private List<CardAction> Actions;

    public CardActionGroup(CardActionData data, string cardID)
    {
        Actions = new List<CardAction>();
        for(int i = 0; i < data.ActionsData.Count; i++)
        {
            Actions.Add(CardActionFactory.Get(data.ActionsData[i]));
            Actions[i].Init(data.ActionsData[i], cardID);
        }

        List<SkillCheckObject> skills = new List<SkillCheckObject>();
        for(int i = 0; i < data.SkillsData.Count; i++)
        {
            skills.Add(SkillCheckObject.Create(data.SkillsData[i].SkillType, data.SkillsData[i].SkillAmount, 1));
        }
        _check = GroupSkillCheck.Create(skills);
    }

    public bool Check()
    {
        if (_check == null || _check.Skills.Count == 0)
            return true;

        if (_check.IsComplete)
            return _check.Success;

        _check.CompleteCheck(GM.PlayerIcon.Group.GetAliveUnits().Where(unit => unit.IsActive == true).ToList());

        //for test
        _check.LogBattleData();

        return _check.Success;
    }

    public void AddActionsToQueue()
    {
        for(int i = 0; i < Actions.Count; i++)
            BattleSystem.AddFunctionToQuery(Actions[i]);
    }

    public string GetActionsDescriptions()
    {
        return "Not Setup";
    }
}
