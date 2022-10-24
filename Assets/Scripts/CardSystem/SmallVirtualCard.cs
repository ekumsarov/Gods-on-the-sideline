using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DragAndDropSystem;


public class SmallVirtualCard : UIItem
{
    [SerializeField] private SimpleText Name;

    private string _cardID;
    private static VirtualCard _showingTemplateCard;

    public override void Setting()
    {
        base.Setting();
        Name.Visible = true;
    }
    public string CardID
    {
        get { return this._cardID; }
    }
    public void ReadCard(string data)
    {
        if (data.IsNullOrEmpty())
        {
            _cardID = string.Empty;
            this.Visible = false;
            this.DataInfo = string.Empty;
            return;
        }



        CardData cardInfo = IOM.GetCard(data);
        if (cardInfo == null)
            return;

        Name.Text = LocalizationManager.Get(cardInfo.ID);
        _cardID = cardInfo.ID;
        this.DataInfo = cardInfo.ID;
    }

    public void RemoveCard()
    {
        this.ReadCard(string.Empty);
    }

    public override void Selected(bool enter)
    {
        if(enter)
        {
            if (this._frameType == FrameType.Selectable)
                this.Frame = true;

            StartCoroutine(FadeInCard());
        }
        else
        {
            if (this._frameType == FrameType.Selectable)
                this.Frame = false;

            StopAllCoroutines();

            if (_showingTemplateCard != null)
                _showingTemplateCard.Visible = false;
        }
    }

    IEnumerator FadeInCard()
    {
        yield return new WaitForSeconds(1f);

        if(_showingTemplateCard == null)
        {
            _showingTemplateCard = VirtualCard.GetCard(this._cardID);
            _showingTemplateCard.GetTransform.SetParent(this._parentMenu.GetTransform);
            _showingTemplateCard.GetTransform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }

        _showingTemplateCard.ReadCard(this._cardID);
        _showingTemplateCard.position = new Vector3(this.position.x + this.Rect.rect.width/2 + _showingTemplateCard.Rect.rect.width/2 + 10f,
            this.position.y, 0f);
        _showingTemplateCard.Visible = true;
    }
}