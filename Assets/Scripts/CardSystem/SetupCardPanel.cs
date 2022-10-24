using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DragAndDropSystem;

public class SetupCardPanel : DragCell
{
    private List<SmallVirtualCard> _cards;

    public override void Setting()
    {
        base.Setting();
        _cards = new List<SmallVirtualCard>();
        _cards.AddRange(this.GetTransform.GetComponentsInChildren<SmallVirtualCard>());
    }

    public override bool CanPlace(string itemTag)
    {
        if (DeckSystem.CanAdd(itemTag) == false)
            return false;

        return base.CanPlace(itemTag);
    }

    public override void PlaceItem(string item)
    {
        if (_cards == null || _cards.Count == 0 )
            throw new System.Exception("Setup card place");

        if (_cards.All(card => card.Visible == true))
            return;

        if (_cards.Any(card => card.DataInfo.Equals(item)))
            return;

        for(int i = 0; i < _cards.Count; i++)
        {
            if(_cards[i].Visible == false)
            {
                _cards[i].ReadCard(item);
                _cards[i].Visible = true;
                break;
            }
        }

        for(int i = 0; i < _cards.Count; i++)
        {
            if(_cards[i].Visible == false)
            {
                _cards[i].ReadCard(string.Empty);
            }
        }
    }

    public void AddCard(string card)
    {
        if (_cards == null || _cards.Count == 0)
            throw new System.Exception("Setup card place");

        if (_cards.All(card => card.Visible == true))
            return;

        for (int i = 0; i < _cards.Count; i++)
        {
            if (_cards[i].Visible == false)
            {
                _cards[i].ReadCard(card);
                _cards[i].Visible = true;
                break;
            }
        }
    }

    public void ClearPanel()
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            _cards[i].ReadCard(string.Empty);
            _cards[i].Visible = false;
        }
    }

    public void RemoveCard(string id)
    {
        for(int i = 0; i < _cards.Count; i++)
        {
            if(_cards[i].CardID.Equals(id))
            {
                _cards[i].ReadCard(string.Empty);
                _cards[i].Visible = false;
                break;
            }
        }
    }

    public override string GetItemID => string.Empty;
}
