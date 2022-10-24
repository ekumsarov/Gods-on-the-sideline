using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

[System.Serializable]
public class ConditionData
{
    [SerializeField] private ConditionType _type = ConditionType.NoType;
    public ConditionType Type => _type;

    [SerializeField] private CardTargetSide _side = CardTargetSide.Any;
    public CardTargetSide Side => _side;

    [SerializeField] private List<string> _conditionIDs;
    public List<string> ConditionIDs => _conditionIDs;

    [SerializeField] private List<EffectType> _effectIDs;
    public List<EffectType> EffectIDs => _effectIDs;

    [SerializeField] private bool _conditionToHas = true;
    public bool ConditionToHas => _conditionToHas;
}

