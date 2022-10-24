using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DragAndDropSystem;


public class VirtualCard : DragItem
{
    [SerializeField] private UIImage Icon;
    [SerializeField] private SimpleText Name;
    [SerializeField] private SimpleText Cost;
    [SerializeField] private UIIconText CardsActionText;

    private string _cardID;
    private static VirtualCard _prefabCard;
    private static float cellHeight = 0.0f;

    public override void Setting()
    {
        base.Setting();

        if(VirtualCard.cellHeight == 0f)
        {
            if (this.Rect.GetComponentInParent<GridLayoutGroup>() != null)
                VirtualCard.cellHeight = this.Rect.GetComponentInParent<GridLayoutGroup>().cellSize.y;
        }

        CardsActionText.Visible = true;
        Name.Visible = true;
//        this.CardsActionText.Rect.sizeDelta = new Vector2(CardsActionText.Rect.rect.width, VirtualCard.cellHeight - Icon.Rect.rect.height - Name.Rect.rect.height);
    }

    public void ReadCard(string data)
    {
        if(data.IsNullOrEmpty())
        {
            Debug.LogError("Card id is null");
            data = "StandartCard";
        }

        CardData cardInfo = IOM.GetCard(data);
        if (cardInfo == null)
            return;

        if(Cost != null)
        {
            Cost.Text = cardInfo.CardCost.ToString();
            Cost.Visible = true;
        }    
            

        Name.Text = LocalizationManager.Get(cardInfo.ID);
        _cardID = cardInfo.ID;

        CardsActionText.IconText.Text(CardActionFactory.GetCardDescription(cardInfo));
        CardsActionText.IconText.ShowComplete();
        SetAlpha(1f);
    }

    protected override void CreateDragItem(PointerEventData pointer)
    {
        DragVirtualCard.Setup(_cardID, "VirtualCard", IOM.GetCard(_cardID), this, pointer, GetComponentInParent<DragCell>());
    }

    public static VirtualCard GetCard(string cardID)
    {
        if (VirtualCard._prefabCard == null)
            VirtualCard._prefabCard = GameObject.Instantiate<VirtualCard>(Resources.Load<VirtualCard>("Prefabs/UIeX/Complete/VirtualCard"));

        VirtualCard temp = GameObject.Instantiate<VirtualCard>(VirtualCard._prefabCard);
        temp.ReadCard(cardID);
        temp.Visible = true;

        return temp;
    }
}