using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

public interface ICardData 
{
    string ID { get; }
    int CardCost { get; }

    Lodkod.AfterPlayEffect PlayEffect { get; }

    List<CardActionData> CardActions { get; }

    SkillType CardSkillType { get; }
}
