using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BattleEngine;


public class CardUIItem : UIItem
{
    private Card _card;

    [SerializeField] private UIImage Icon;
    [SerializeField] private SimpleText Name;
    [SerializeField] private UIIconText CardsActionText;

    private PlayCardAction _delegate;

    public void InitializeCard(string id)
    {
        Card temp = Card.GetCard(id);
        this.InitializeCard(temp);
    }

    public void BindCallAction(PlayCardAction del)
    {
        _delegate += del;
    }

    public void InitializeCard(Card data)
    {
        _card = data;

        if (_card.IsInitialized == false)
            return;

        Name.Text = LocalizationManager.Get(data.Data.ID);

        CardsActionText.IconText.Text(CardActionFactory.GetCardDescription(data.Data));
        CardsActionText.IconText.ShowComplete();

        this.Visible = true;

        _delegate?.Invoke(_card);
        //BattleActions.PlayCard.Create(_card);
    }
}