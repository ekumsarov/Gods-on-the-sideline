using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject, ICardData
{
    public string ID => _id;
    public int CardCost => _cardCost;
    public List<CardActionData> CardActions => _actions;
    public AfterPlayEffect PlayEffect => _playEffect;
    public SkillType CardSkillType => _cardSkillType;

    [SerializeField] private string _id;
    [SerializeField] private int _cardCost = 1;
    [SerializeField] private AfterPlayEffect _playEffect = AfterPlayEffect.Discard;
    [SerializeField] private SkillType _cardSkillType = SkillType.Light;
    [SerializeField] private List<CardActionData> _actions;
}

