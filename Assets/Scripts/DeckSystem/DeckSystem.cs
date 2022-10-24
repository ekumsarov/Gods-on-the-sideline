using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SimpleJSON;
using BattleEngine;

public class DeckSystem
{
    private static DeckSystem _instance;
    public static void NewGame()
    {
        if (DeckSystem._instance != null)
            DeckSystem._instance = null;

        DeckSystem._instance = new DeckSystem();

        DeckSystem._instance._preparedCards = new List<string>();
        DeckSystem._instance._graveDeck = new List<string>();
        DeckSystem._instance._currentDeck = new List<string>();
        DeckSystem._instance._actualBattleDeck = new List<string>();
        DeckSystem._instance._deckList = new Dictionary<string, int>();
        DeckSystem._instance._subscriber = Subscriber.Create(DeckSystem._instance);
        DeckSystem._instance._subscriber.AddEvent("PartyUpdate");
    }

    public static void NewGame(JSONNode node)
    {
        if (DeckSystem._instance != null)
            DeckSystem._instance = null;

        DeckSystem._instance = new DeckSystem();

        DeckSystem._instance._currentDeck = new List<string>();
        DeckSystem._instance._graveDeck = new List<string>();
        DeckSystem._instance._preparedCards = new List<string>();

        DeckSystem._instance._actualBattleDeck = new List<string>();
        DeckSystem._instance._deckList = new Dictionary<string, int>();
        DeckSystem._instance._subscriber = Subscriber.Create(DeckSystem._instance);
        DeckSystem._instance._subscriber.AddEvent("PartyUpdate");

        if(node["deck"] != null)
        {
            JSONArray array = node["deck"].AsArray;
            for(int i = 0; i < array.Count; i++)
            {
                if(array[i]["Card"] != null && array[i]["Amount"] != null)
                {
                    DeckSystem._instance._deckList.Add(array[i]["Card"].Value, array[i]["Amount"].AsInt);
                }
                else if(array[i]["Card"] != null && array[i]["Amount"] == null)
                {
                    DeckSystem._instance._deckList.Add(array[i]["Card"].Value, 1);
                }
                else
                {
                    DeckSystem._instance._deckList.Add(array[i].Value, 1);
                }
            }
        }
    }

    #region properties

    // notify
    private Subscriber _subscriber;

    // HACK
    bool firstInit = true;

    private Dictionary<string, int> _deckList;
    private List<string> _actualBattleDeck;
    private readonly int _deckLimitation = GSM.GameSettings.CardInDeck;
    private readonly int _cardLimitaion = 1;

    #endregion

    public static List<string> DeckList
    {
        get
        {
            if (DeckSystem._instance == null || DeckSystem._instance._deckList == null)
                throw new System.Exception("Something wrong with deck");

            return DeckSystem._instance._deckList.Keys.ToList();
        }
    }

    public static void RebuildDeck()
    {
        List<CardData> _heroCards = new List<CardData>();
        for (int i = 0; i < 4; i++)
        {
            if (i < GM.PlayerIcon.Group.GetUnits().Count)
            {
                _heroCards.AddRange(IOM.GetHeroCards(GM.PlayerIcon.Group.GetUnits()[i].Name, 1));
                _heroCards.AddRange(IOM.GetHeroCards(GM.PlayerIcon.Group.GetUnits()[i].Name, 2));
            }
        }

        List<string> deleteCards = new List<string>();
        foreach (var card in DeckSystem._instance._deckList)
        {
            if (!_heroCards.Any(check => check.ID.Equals(card.Key)))
                deleteCards.Add(card.Key);
        }

        for (int i = 0; i < deleteCards.Count; i++)
        {
            DeckSystem._instance._deckList.Remove(deleteCards[i]);
        }
    }

    /// <summary>
    /// Use for update after setup deck from UI
    /// </summary>
    /// <param name="deck"></param>
    public static void UpdateDeck(Dictionary<string, int> deck)
    {
        DeckSystem._instance._deckList = deck;
    }

    public static void UpdateDeck(List<string> deck)
    {
        DeckSystem._instance._deckList.Clear();
        foreach(var card in deck)
        {
            DeckSystem._instance._deckList.Add(card, 1);
        }
    }

    public static void AddCard(string cardID)
    {
        if (DeckSystem._instance._deckList.ContainsKey(cardID))
            DeckSystem._instance._deckList[cardID] += 1;
        else
            DeckSystem._instance._deckList.Add(cardID, 1);
    }

    public static bool HasCard(string cardID)
    {
        return DeckSystem._instance._deckList.ContainsKey(cardID);
    }

    public static bool CanAdd(string cardID)
    {
        int cardAmount = 0;
        foreach(var card in DeckSystem._instance._deckList)
        {
            cardAmount += card.Value;
        }

        if (cardAmount + 1 > DeckSystem._instance._deckLimitation)
            return false;

        if (DeckSystem._instance._deckList.ContainsKey(cardID))
            return DeckSystem._instance._deckList[cardID] < DeckSystem._instance._cardLimitaion;

        return true;
    }

    #region BattleSystem

    private List<string> _currentDeck;
    private List<string> _graveDeck;
    private List<string> _preparedCards;

    public static List<string> PreparedCards => DeckSystem._instance._preparedCards;

    public static void PrepareForBattle()
    {
        DeckSystem._instance._actualBattleDeck.Clear();
        DeckSystem._instance._currentDeck.Clear();
        DeckSystem._instance._graveDeck.Clear();
        DeckSystem._instance._preparedCards.Clear();

        foreach (var card in DeckSystem._instance._deckList)
        {
            DeckSystem._instance._actualBattleDeck.Add(card.Key);
            DeckSystem._instance._currentDeck.Add(card.Key);
            //DeckSystem._instance._actualDeck.Add(Card.GetCard(IOM.GetCard(card.Key)), 1);
        }
    }

    public static void ChooseCards()
    {
        RebuildBattleDeck();
        DeckSystem._instance._preparedCards.Clear();
        int chooseCards = GSM.GameSettings.CardToGet;

        if (DeckSystem._instance._currentDeck.Count == 0)
            DeckSystem.RestoreGrave();

        if(DeckSystem._instance._currentDeck.Count < chooseCards && DeckSystem._instance._currentDeck.Count > 0)
        {
            for(int i = 0; i < DeckSystem._instance._currentDeck.Count; i++)
            {
                DeckSystem._instance._preparedCards.Add(DeckSystem._instance._currentDeck[i]);
                chooseCards -= 1;
            }
            DeckSystem.RestoreGrave();
        }

        for(int i = 0; i < chooseCards; i++)
        {
            int selected = Random.Range(0, DeckSystem._instance._currentDeck.Count);
            DeckSystem._instance._preparedCards.Add(DeckSystem._instance._currentDeck[selected]);
            DeckSystem._instance._currentDeck.RemoveAt(selected);
        }
    }

    public static void RestoreGrave()
    {
        DeckSystem._instance._currentDeck.Clear();
        foreach (var card in DeckSystem._instance._actualBattleDeck)
        {
            DeckSystem._instance._currentDeck.Add(card);
        }

        for(int i = 0; i < DeckSystem._instance._preparedCards.Count; i++)
        {
            DeckSystem._instance._currentDeck.Remove(DeckSystem._instance._preparedCards[i]);
        }
    }

    public static void RebuildBattleDeck()
    {
        List<CardData> _heroCards = new List<CardData>();
        foreach(var hero in BattleSystem.Heroes)
        {
            if (hero.IsAlive)
            {
                _heroCards.AddRange(IOM.GetHeroCards(hero.Name, 1));
                _heroCards.AddRange(IOM.GetHeroCards(hero.Name, 2));
            }
        }

        List<string> deleteCards = new List<string>();
        foreach (var card in DeckSystem._instance._actualBattleDeck)
        {
            if (!_heroCards.Any(check => check.ID.Equals(card)))
                deleteCards.Add(card);
        }

        for (int i = 0; i < deleteCards.Count; i++)
        {
            DeckSystem._instance._actualBattleDeck.Remove(deleteCards[i]);
        }

        deleteCards.Clear();
        foreach (var card in DeckSystem._instance._currentDeck)
        {
            if (!DeckSystem._instance._actualBattleDeck.Any(check => check.Equals(card)))
                deleteCards.Add(card);
        }

        for (int i = 0; i < deleteCards.Count; i++)
        {
            DeckSystem._instance._currentDeck.Remove(deleteCards[i]);
        }
    }

    #endregion
}
