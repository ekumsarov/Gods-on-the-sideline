using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragAndDropSystem;
using UnityEngine.EventSystems;

public class HeroItem : DragItem
{
    [SerializeField] private UIImage Icon;
    private string _heroID;
    protected override void CreateDragItem(PointerEventData pointer)
    {
        HeroDragItem.Setup(_heroID, "ChooseHero", this, pointer, GetComponentInParent<DragCell>());
    }

    public void SetupHero(string heroID)
    {
        
        if(heroID.IsNullOrEmpty())
        {
            this.Icon.Visible = false;
            return;
        }

        HeroData data = IOM.GetHeroData(heroID);
        this._heroID = heroID;
        this.Icon.Image = data.Icon;
        this.Icon.Visible = true;
        this.TooltipText = this._heroID;
    }

}
