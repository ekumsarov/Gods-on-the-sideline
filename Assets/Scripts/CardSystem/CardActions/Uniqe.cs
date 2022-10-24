using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using BattleEngine;

public class Uniqe : CardAction
{
    public override string ActionType => CardActionType.Uniqe.ToString();

    public override void Init(CardActionTypes data, string heroID)
    {
        throw new System.NotImplementedException();
    }

    protected override void Play()
    {
        if (_target.IsTargetSelected() == false)
        {
            BattleSystem.SubscribeOnUnitPoint(_target.SelectedTarget);
            return;
        }
    }
}