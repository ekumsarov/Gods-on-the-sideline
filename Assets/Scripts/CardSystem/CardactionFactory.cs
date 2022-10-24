using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using Lodkod;
using System;
using BattleEngine;

public static class CardActionFactory
{
    private static Dictionary<string, Type> actionsByName;
    private static bool IsInitialized => actionsByName != null;

    public static void Initialized()
    {
        if (IsInitialized)
            return;

        var assemblies = Assembly.GetAssembly(typeof(CardAction)).GetTypes().
            Where(mytype => mytype.IsClass && !mytype.IsAbstract && mytype.IsSubclassOf(typeof(CardAction)));

        actionsByName = new Dictionary<string, Type>();

        foreach(var action in assemblies)
        {
            var tempAction = Activator.CreateInstance(action) as CardAction;

            if (actionsByName.ContainsKey(tempAction.ActionType))
                continue;

            actionsByName.Add(tempAction.ActionType, action);
        }
    }

    public static CardAction Get(CardActionTypes data)
    {
        Initialized();

        if(data.dActionType == CardActionType.Uniqe && actionsByName.ContainsKey(data.dCardID))
        {
            Type temp = actionsByName[data.dCardID];
            CardAction instance = Activator.CreateInstance(temp) as CardAction;
            //instance.Init(data);
            return instance;
        }

        //if (data.dActionType == CardActionType.Uniqe)
        //{
        //    throw new Exception("Not implemented card action " + data.dCardID);
        //}

        if(actionsByName.ContainsKey(data.dActionType.ToString()))
        {
            Type temp = actionsByName[data.dActionType.ToString()];
            CardAction instance = Activator.CreateInstance(temp) as CardAction;
            //instance.Init(data);
            return instance;
        }

        return null;
    }

    public static string GetCardDescription(CardData data)
    {
        string text = "\n\n";
        foreach(var act in data.CardActions)
        {
            if(act.SkillsData.Count > 0)
            {
                text += LocalizationManager.Get("SkillRequestPattern") + act.SkillsData[0].SkillAmount + 
                    " " + SkillObject.SkillFullName(act.SkillsData[0].SkillType);
                for(int i = 1; i < act.SkillsData.Count; i++)
                {
                    text += ", ";
                    text += act.SkillsData[i].SkillAmount + " " + SkillObject.SkillFullName(act.SkillsData[i].SkillType);
                }

                text += ".\n";
            }

            foreach(var action in act.ActionsData)
            {
                var tempAction = Get(action);
                if(tempAction == null)
                {
                    Debug.LogError("No card action id: " + action.dActionType.ToString());
                    continue;
                }

                text += tempAction.GetDescription(action);
                tempAction = null;
            }

            text += "\n";
        }

        return text;
    }
}
