using System.Collections.Generic;
using UnityEngine;
using BattleEngine;
public class Card
{
    private CardData _data;
    private List<CardActionGroup> _actions;

    private bool _initialized = false;
    public bool IsInitialized => _initialized;
    public CardData Data => _data;

    public void InitializeCard(CardData data)
    {
        if(data == null)
        {
            Debug.LogError("Not found card data for card creation: ");
            return;
        }

        _selectedtargets = new List<iBattleUnit>();
        _data = data;
        _actions = new List<CardActionGroup>();

        for (int i = 0; i < _data.CardActions.Count; i++)
        {
            CardActionGroup group = new CardActionGroup(_data.CardActions[i], data.ID);
            _actions.Add(group);
        }

        _initialized = true;
    }

    public static Card GetCard(CardData data)
    {
        Card temp = new Card();
        temp.InitializeCard(data);
        return temp;
    }

    public static Card GetCard(string id)
    {
        Card temp = new Card();
        temp.InitializeCard(IOM.GetCard(id));
        return temp;

    }

    public void PlayCard()
    {
        if(_initialized == false)
        {
            Debug.LogError("No card");
            return;
        }

        BattleLog.PushLog("Played card: " + LocalizationManager.Get(this.CardID), false);

        for(int i = 0; i < _actions.Count; i++)
        {
            if(_actions[i].Check())
            {
                _actions[i].AddActionsToQueue();
            }
        }
    }
    public string CardID
    {
        get { return _data.ID; }
    }
    public int CardCost
    {
        get { return _data.CardCost; }
    }

    public string GetActionDescriptions()
    {
        string text = "";
        for (int i = 0; i < _actions.Count; i++)
        {
            text += i + 1 + ") " + _actions[i].GetActionsDescriptions();
        }

        return text;
    }

    private List<iBattleUnit> _selectedtargets;
}
