using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Lodkod;
using System;
using System.Linq;
using GameEvents;
using SimpleJSON;
using DragAndDropSystem;

public class HeroesMenu : MenuEx
{

    #region base parameters

    [SerializeField] private SetupCardPanel _setupCardPanel;
    [SerializeField] private UIItem _cardsPanel;
    [SerializeField] private UIItem _selectHeroPanel;

    private List<VirtualCard> _cardItems;
    [SerializeField] private List<UIItem> _heroesCells;
    private List<HeroItem> _heroesLoopCells;
    private List<HeroCell> _heroes;
    /*[SerializeField] List<SkillItem> skills;*/

    private List<string> _deck;
    #endregion

    public override void Setting()
    {
        base.Setting();

        _deck = new List<string>();
        _heroesLoopCells = new List<HeroItem>();
        foreach(var item in this._allItems.Where(her => her.Value.ItemTag.Equals("ChooseHero")))
        {
            _heroesLoopCells.Add(item.Value as HeroItem);
        }

        _cardItems = new List<VirtualCard>();
        foreach(var cardItem in this._allItems.Where(item => item.Value.ItemTag.Equals("VirtualCard")))
        {
            _cardItems.Add(cardItem.Value as VirtualCard);
        }

        _heroes = new List<HeroCell>();
        foreach (var cardItem in this._allItems.Where(item => item.Value.ItemTag.Equals("HeroCell")))
        {
            _heroes.Add(cardItem.Value as HeroCell);
        }
    }

    public override void Open()
    {
        this._allItems["SelectPanel"].Visible = false;
        _deck.Clear();
        _deck = DeckSystem.DeckList;
        _setupCardPanel.ClearPanel();
        for(int i = 0; i < _deck.Count; i++)
        {
            _setupCardPanel.AddCard(_deck[i]);
        }

        for (int i = 0; i < _heroes.Count; i++)
        {
            if (i < GM.PlayerIcon.Group.GetUnits().Count)
                this._heroes[i].SetupHero(GM.PlayerIcon.Group.GetUnits()[i].Name);
            else
                this._heroes[i].SetupHero(string.Empty);

            this._heroes[i].Visible = false;
        }

        int index = 0; 
        foreach(var hero in IOM.AvailiableHeroes)
        {
            if (index > this._heroesLoopCells.Count)
                break;

            if (GM.PlayerIcon.Group.GetUnits().Any(unit => unit.Name.Equals(hero.Name)))
                continue;

            this._heroesLoopCells[index].SetupHero(hero.Name);
            this._heroesLoopCells[index].Visible = true;
            index++;
        }
        if(index < this._heroesLoopCells.Count)
        {
            for(int i = index; i < this._heroesLoopCells.Count; i++)
            {
                this._heroesLoopCells[i].Visible = false;
            }
        }

        List<CardData> _heroCards = new List<CardData>();
        for (int i = 0; i < 4; i++)
        {
            if (i < GM.PlayerIcon.Group.GetUnits().Count)
            {
                _heroCards.AddRange(IOM.GetHeroCards(GM.PlayerIcon.Group.GetUnits()[i].Name, 1));
                _heroCards.AddRange(IOM.GetHeroCards(GM.PlayerIcon.Group.GetUnits()[i].Name, 2));
            }
        }

        for(int i = 0; i < _heroCards.Count; i++)
        {
            if(i < this._cardItems.Count)
            {
                this._cardItems[i].ReadCard(_heroCards[i].ID);
                this._cardItems[i].Visible = true;
            }
        }

        if(_heroCards.Count < this._cardItems.Count)
        {
            for(int i = _heroCards.Count; i < this._cardItems.Count; i++)
            {
                this._cardItems[i].Visible = false;
            }
        }

        this._setupCardPanel.ClearPanel();
        for(int i = 0; i < DeckSystem.DeckList.Count; i++)
        {
            this._setupCardPanel.AddCard(DeckSystem.DeckList[i]);
        }

        this._setupCardPanel.Visible = true;

        base.Open();
    }

    public override void SelectedItem(UIItem data, bool enter)
    {

        if (data.ItemTag.Equals("EndRound"))
            return;
    }

    public override void PressedItem(UIItem data)
    {
        if (data.ItemTag.Equals("HeroButton"))
        {
            this._allItems["HeroGroups"].Visible = true;
            this._allItems["PlayerActions"].Visible = true;
            this._allItems["HeroInventory"].Visible = true;
            this._allItems["ActionChoices"].Visible = false;
            this._allItems["Inventory"].Visible = false;
        }

        if (data.ItemTag.Equals("LootButton"))
        {
            this._allItems["Hero1"].Visible = false;
            this._allItems["Hero2"].Visible = false;
            this._allItems["Hero3"].Visible = false;
            this._allItems["Hero4"].Visible = false;
            this._allItems["SetupCardsPlace"].Visible = true;
            this._allItems["Inventory"].Visible = true;
            this._allItems["HeroPool"].Visible = false;
        }

        if (data.ItemTag.Equals("ActionButton"))
        {
            this._allItems["Hero1"].Visible = true;
            this._allItems["Hero2"].Visible = true;
            this._allItems["Hero3"].Visible = true;
            this._allItems["Hero4"].Visible = true;
            this._allItems["SetupCardsPlace"].Visible = false;
            this._allItems["Inventory"].Visible = false;
            this._allItems["HeroPool"].Visible = true;
        }

        if (data.ItemTag.Equals("Close"))
        {
            if (DeckSystem.DeckList.Count < GSM.GameSettings.CardToGet)
                return;

            UIM.CloseMenu("HeroesMenu");
        }
    }

    #region show panel control

    public override void ItemsChanged(Descriptor descriptor)
    {
        if(descriptor.ItemTag.Equals("VirtualCard"))
        {
            DeckSystem.AddCard(descriptor.ItemID);
        }

        if(descriptor.ItemTag.Equals("ChooseHero"))
        {
            GM.PlayerIcon.Group.RemoveHero(descriptor.targetItemID);
            GM.PlayerIcon.Group.AddNewHero(descriptor.ItemID);
            this.UpdateMenu();
        }
    }

    public override void UpdateMenu()
    {
        int index = 0;
        foreach (var hero in IOM.AvailiableHeroes)
        {
            if (index > this._heroesLoopCells.Count)
                break;

            if (GM.PlayerIcon.Group.GetUnits().Any(unit => unit.Name.Equals(hero.Name)))
                continue;

            this._heroesLoopCells[index].SetupHero(hero.Name);
            this._heroesLoopCells[index].Visible = true;
            index++;
        }
        if (index < this._heroesLoopCells.Count)
        {
            for (int i = index; i < this._heroesLoopCells.Count; i++)
            {
                this._heroesLoopCells[i].Visible = false;
            }
        }

        List<CardData> _heroCards = new List<CardData>();
        for (int i = 0; i < 4; i++)
        {
            if (i < GM.PlayerIcon.Group.GetUnits().Count)
            {
                List<CardData> tempList;
                tempList = IOM.GetHeroCards(GM.PlayerIcon.Group.GetUnits()[i].Name, 1);
                
                if(tempList != null && tempList.Count > 0)
                    _heroCards.AddRange(tempList);

                tempList = IOM.GetHeroCards(GM.PlayerIcon.Group.GetUnits()[i].Name, 2);
                if (tempList != null && tempList.Count > 0)
                    _heroCards.AddRange(tempList);
            }
        }

        for (int i = 0; i < _heroCards.Count; i++)
        {
            if (i < this._cardItems.Count)
            {
                this._cardItems[i].ReadCard(_heroCards[i].ID);
                this._cardItems[i].Visible = true;
            }
        }

        if (_heroCards.Count < this._cardItems.Count)
        {
            for (int i = _heroCards.Count; i < this._cardItems.Count; i++)
            {
                this._cardItems[i].Visible = false;
            }
        }

        this._setupCardPanel.ClearPanel();
        for (int i = 0; i < DeckSystem.DeckList.Count; i++)
        {
            this._setupCardPanel.AddCard(DeckSystem.DeckList[i]);
        }
    }

    #endregion

    
}