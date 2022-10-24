using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lodkod;
using System;
using System.Linq;
using GameEvents;
using SimpleJSON;
using BattleEngine;
using DG.Tweening;

public class BattleMenu : MenuEx
{

    #region base parameters

    [SerializeField] private UIItem _frameSelector;
    [SerializeField] private CardPlayerLayout _playerLayout;
    [SerializeField] private SimpleDragCell _placeCards;
    [SerializeField] private CardUIItem _cardItem;
    [SerializeField] private SimpleText _forTest;

    private List<BattleUnitView> _heroViews;
    private List<BattleUnitView> _enemyViews;
    private List<BattleUnitView> _additionalHeroViews;
    private List<BattleUnitView> _additionalEnemyViews;

    [SerializeField] private List<SkillItem> _skillItems;

    #endregion

    public override void Setting()
    {
        base.Setting();

        this._heroViews = new List<BattleUnitView>();
        this._enemyViews = new List<BattleUnitView>();
        this._additionalHeroViews = new List<BattleUnitView>();
        this._additionalEnemyViews = new List<BattleUnitView>();

        foreach (var unitView in this._allItems)
        {
            if (unitView.Value.ItemTag.Equals("HeroView"))
                this._heroViews.Add(unitView.Value as BattleUnitView);
            else if (unitView.Value.ItemTag.Equals("EnemyView"))
                this._enemyViews.Add(unitView.Value as BattleUnitView);
            else if (unitView.Value.ItemTag.Equals("AdditionalHeroView"))
                this._additionalHeroViews.Add(unitView.Value as BattleUnitView);
            else if (unitView.Value.ItemTag.Equals("AdditionalEnemyView"))
                this._additionalEnemyViews.Add(unitView.Value as BattleUnitView);

        }


        _placeCards.ItemPlaced = _cardItem.InitializeCard;
        _cardItem.Visible = false;
        
        BattleSystem.SetupMenu(this);
        BattleLog.ActivateVew(this._allIconTexts["BattleLogLabel"]);
    }

    public void Prepare()
    {
        _forTest.Visible = true;
        _forTest.Text = "PrepareBattle";
        this._frameSelector.Visible = false;

        for (int i = 0; i < this._heroViews.Count; i++)
        {
            if (i < BattleSystem.Heroes.Count)
            {
                this._heroViews[i].BindUnit(BattleSystem.Heroes[i]);
                this._heroViews[i].Visible = true;
            }
            else
                this._heroViews[i].Visible = false;
        }

        for (int i = 0; i < this._enemyViews.Count; i++)
        {
            if (i < BattleSystem.Enemies.Count)
            {
                this._enemyViews[i].BindUnit(BattleSystem.Enemies[i]);
                this._enemyViews[i].Visible = true;
            }
            else
                this._enemyViews[i].Visible = false;
        }

        List<SkillObject> heroSkills = new List<SkillObject>();

        List<SkillObject> forTemp = BattleSystem.Heroes[0].Skills.Values.ToList();
        for (int i = 0; i < forTemp.Count; i++)
        {
            string skillID = forTemp[i].Skill;
            SkillObject skill = SkillObject.Make(skillID, 0, 0);
            foreach (var hero in BattleSystem.Heroes)
            {
                skill.SetSkill(hero.Skills[skillID]);
            }
            heroSkills.Add(skill);
        }

        

        for (int i = 0; i < this._skillItems.Count; i++)
        {
            if (i < heroSkills.Count)
            {
                this._skillItems[i].Setup(heroSkills[i]);
                this._skillItems[i].Visible = true;
            }
            else
                this._skillItems[i].Visible = false;
        }

        BattleSystem.StartBattle();
    }

    public void AcivateUnit(Unit unit, bool isPlayerSide)
    {
        if(isPlayerSide)
        {
            for (int i = 0; i < this._heroViews.Count; i++)
            {
                if (this._heroViews[i].Visible == false)
                {
                    this._heroViews[i].BindUnit(unit);
                    this._heroViews[i].Visible = true;
                    return;
                }
            }
            
        }
        else
        {
            for (int i = 0; i < this._enemyViews.Count; i++)
            {
                if (this._enemyViews[i].Visible == false)
                {
                    this._enemyViews[i].BindUnit(unit);
                    this._enemyViews[i].Visible = true;
                    return;
                }
            }
        }
    }

    public bool CannActivateUnit(bool isPlayerSide)
    {
        if(isPlayerSide)
        {
            for(int i = 0; i < this._heroViews.Count; i++)
            {
                if (this._heroViews[i].Visible == false)
                    return true;
            }
        }
        else
        {
            for(int i = 0; i < this._enemyViews.Count; i++)
            {
                if (this._enemyViews[i].Visible == false)
                    return true;
            }
        }

        return false;
    }

    public void NextTurn()
    {
        _forTest.Text = "PlayerTurn\n";
        _playerLayout.PlaceStartCard();
    }

    public void ActionTextChange(int count)
    {
        _forTest.Text = "PlayerTurn\n" + "Action Count: " + count;
    }

    public void ClearOnPlayerTurn()
    {
        _cardItem.Visible = false;
    }

    bool _canPlayerInteract = true;
    public void PlayerInteract(bool value)
    {
        _canPlayerInteract = value;
    }

    public void EndPlayerTurn()
    {
        _forTest.Text = "PlayerTurnEnd";
        _playerLayout.DiscardAll();
        _cardItem.Visible = false;
        //StartCoroutine(EndTurnCount());
    }

    IEnumerator EndTurnCount()
    {
        float timer = 1f;

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
    }

    public void BindCardAction(PlayCardAction del)
    {
        _cardItem.BindCallAction(del);
    }

    public void NewRound()
    {
        _forTest.Text = "NextRound";
        this.NextTurn();
    }

    public override void SelectedItem(UIItem data, bool enter)
    {
        if (data.ItemTag.Equals("VirtualCardLayout"))
        {
            this._playerLayout.SelectCard(data as VirtualCard, enter);
        }
        

        if (!BattleSystem.PlayerTurn)
            return;

        if (data.ItemTag.Equals("EndRound"))
            return;

    }

    public override void PressedItem(UIItem data)
    {
        if (!BattleSystem.PlayerTurn)
            return;

        if (data.ItemTag.Equals("Log"))
            BattleLog.SaveLog();

        if (data.ItemTag.Equals("EndTurn") && _canPlayerInteract)
            BattleSystem.CompletePlayerTurn();
    }

    public void LockCardLayout()
    {
        this._placeCards.LockCell();
    }

    public void UnlockCardLayout()
    {
        this._placeCards.UnlockCell();
    }
}