using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameSettings", menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private int _cardToGet = 5;
    public int CardToGet => _cardToGet;

    [SerializeField] private int _cardActions = 3;
    public int CardActions => _cardActions;

    [SerializeField] private int _cardInDeck = 30;
    public int CardInDeck => _cardInDeck;
}
