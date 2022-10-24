using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragAndDropSystem;

public class HeroCell : DragCell
{
    [SerializeField] private UIItem deleteButton;
    [SerializeField] private UIImage _icon;

    public override void Setting()
    {
        base.Setting();
        deleteButton._but.onClick.RemoveAllListeners();
        deleteButton._but.onClick.AddListener(RemoveHero);
    }
    public override void PlaceItem(string item)
    {
        if (_icon == null)
            throw new System.Exception("Setup Icon in hero cell");

        this.SetupHero(item);
    }

    public void SetupHero(string heroID)
    {
        if(heroID.IsNullOrEmpty())
        {
            this._itemID = string.Empty;
            _icon.Visible = false;
            deleteButton.Visible = false;
            return;
        }

        this._itemID = heroID;
        _icon.Image = IOM.GetHeroData(heroID).Icon;
        deleteButton.Visible = true;
        _icon.Visible = true;
    }

    public bool ActiveButton
    {
        set
        {
            this.deleteButton.Visible = value;
        }
    }
    

    public void RemoveHero()
    {
        GM.PlayerIcon.Group.RemoveHero(this._itemID);
        this._itemID = string.Empty;
        _icon.Visible = false;
        deleteButton.Visible = false;
        this._parentMenu.UpdateMenu();
    }

    private string _itemID;
    public override string GetItemID => _itemID;
}
