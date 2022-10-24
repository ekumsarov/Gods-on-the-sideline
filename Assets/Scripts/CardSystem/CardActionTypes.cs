using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

[System.Serializable]
public class CardActionTypes
{
    //
    // d - prefix nessecary measure 
    // bad example of class and data structure

    [SerializeField] private CardTarget TargetType = CardTarget.FirstSelect;
    public CardTarget dTarget
    {
        get { return this.TargetType; }
    }

    [SerializeField] private CardTargetSide TargetSide = CardTargetSide.Enemy;
    public CardTargetSide dTargetSide
    {
        get { return this.TargetSide; }
    }

    [SerializeField] private CardTargetType TargetIncludeType = CardTargetType.Any;
    public CardTargetType dTargetIncludes
    {
        get { return this.TargetIncludeType; }
    }

    [SerializeField] [ConditionalField("TargetIncludeType", CardTargetType.Class)] private SkillType ClassType = SkillType.Light;
    public SkillType dClassType
    { 
        get { return this.ClassType; }
    }
    [SerializeField] [ConditionalField("TargetIncludeType", CardTargetType.EnemyType)] private EnemySubType EnemyType = EnemySubType.Any;
    public EnemySubType dTnemyType
    {
        get { return this.EnemyType; }
    }
    [SerializeField] [ConditionalField("TargetIncludeType", CardTargetType.ID)] private string TargetID;
    public string dTargetID
    {
        get { return this.TargetID; }
    }

    [SerializeField] private List<ConditionData> _conditions;
    public List<ConditionData> dConditions => _conditions;

    [SerializeField] private CardActionType ActionType = CardActionType.Attack;
    public CardActionType dActionType
    {
        get { return this.ActionType; }
    }
    

    /// <summary>
    /// Heal
    /// </summary>
    [SerializeField] [ConditionalField("ActionType", CardActionType.Attack)] private int DamageAmount = 0;
    public int dDamageAmount
    {
        get { return this.DamageAmount; }
    }

    /// <summary>
    /// Apply Effects
    /// </summary>
    [SerializeField] [ConditionalField("ActionType", CardActionType.ApplyEffect)] private EffectType EffectType = EffectType.Fire;
    public EffectType dEffectType
    {
        get { return this.EffectType; }
    }
    [SerializeField] [ConditionalField("ActionType", CardActionType.ApplyEffect)] private int EffectAmount = 0;
    public int dEffectAmount
    {
        get { return this.EffectAmount; }
    }
    [SerializeField] [ConditionalField("ActionType", CardActionType.ApplyEffect)] private int EffectRoundCount = 0;
    public int dEffectroundCount
    {
        get { return this.EffectRoundCount; }
    }

    [SerializeField][ConditionalField("ActionType", CardActionType.ApplyEffect)] private string EffectSpecID = string.Empty;
    public string dEffectSpecID
    {
        get { return this.EffectSpecID; }
    }

    /// <summary>
    /// Heal
    /// </summary>
    [SerializeField] [ConditionalField("ActionType", CardActionType.Heal)] private int HealAmount = 0;
    public int dHealAmount
    {
        get { return this.HealAmount; }
    }

    [SerializeField] [ConditionalField("ActionType", CardActionType.RemoveEffect)] private List<EffectType> RemoveEffectType;
    public List<EffectType> dRemoveEffectType
    {
        get { return this.RemoveEffectType; }
    }

    [SerializeField] [ConditionalField("ActionType", CardActionType.Summon)] private string PetID;
    public string dPetID
    {
        get { return this.PetID; }
    }

    [SerializeField] [ConditionalField("ActionType", CardActionType.Uniqe)] private string CardID;
    public string dCardID
    {
        get { return this.CardID; }
    }

    //[SerializeField] [ConditionalField("ActionType", Card.CardActionType.AddCard)] private string CardID;
    //[SerializeField] [ConditionalField("ActionType", Card.CardActionType.AddCard)] private int CardAmount;
    //[SerializeField] [ConditionalField("ActionType", Card.CardActionType.AddCard)] private Card.DeckType FromDeck = Card.DeckType.Deck;

    //[SerializeField] [ConditionalField("ActionType", Card.CardActionType.ActionPoint)] private int PointAmount;
    //[SerializeField] [ConditionalField("ActionType", Card.CardActionType.ActionPoint)] private bool AddPoint = true;
    //[SerializeField] [ConditionalField("ActionType", Card.CardActionType.ActionPoint)] private bool NextRound = true;
}
