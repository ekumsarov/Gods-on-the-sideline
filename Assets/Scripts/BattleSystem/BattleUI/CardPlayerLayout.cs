using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class CardPlayerLayout : UIItem
{
    private List<VirtualCard> _allCards;
    [SerializeField] private Vector3 _startCardPosition = new Vector3(-770f, 160f, 0f);
    [SerializeField] private Vector3 _centerPosition = new Vector3(0f, -122.5f, 0f);
    private float _cardSize;
    private float _cardHeight;

    public override void Setting()
    {
        base.Setting();

        //_centerPosition = this._itemStore.transform.TransformPoint(_centerPosition);
        //_startCardPosition = this._itemStore.transform.TransformPoint(_startCardPosition);
        this._allCards = new List<VirtualCard>();
        foreach(var item in GetComponentsInChildren<VirtualCard>())
        {
            this._allCards.Add(item);
        }

        this._allCards[0].Rect.localScale = new Vector3(1f, 1f, 1f);
        this._cardSize = this._allCards[0].Rect.rect.width * 0.8f;
        this._cardHeight = this._allCards[0].Rect.rect.height;

        for (int i = 0; i < this._allCards.Count; i++)
        {
            this._allCards[i].Rect.localScale = new Vector3(0f, 0f, 1f);
            this._allCards[i].Rect.localPosition = new Vector3(-200f, 350f, 0f);
            this._allCards[i].Rect.localEulerAngles = new Vector3(0f, 0f, 0f);
            this._allCards[i].OnDestroy += PlayCardsAnimation;
            this._allCards[i].OnEnd += PlayCardsAnimation;
            this._allCards[i].Visible = false;
        }

        this._placingCards = new List<string>();
    }

    public void PlaceStartCard()
    {
        _completeAnimation = true;
        _placingCards.Clear();

        for (int i = 0; i < DeckSystem.PreparedCards.Count; i++)
        {
            _placingCards.Add(DeckSystem.PreparedCards[i]);
        }

        StartCoroutine(AddCard());
    }

    private List<string> _placingCards;
    private bool _completeAnimation;

    public void SelectCard(VirtualCard card, bool enter)
    {
        if (enter)
        {
            if (this._allCards.Any(aCard => aCard == card))
            {
                Sequence seq = DOTween.Sequence();

                seq.Append(card.Rect.DOLocalMove(new Vector3(card.Rect.anchoredPosition.x, card.Rect.anchoredPosition.y + _cardHeight / 2, 0f), 0.4f).Pause());
                seq.Join(card.Rect.DOScale(new Vector3(1f, 1f, 1f), 0.4f).Pause());
                seq.Join(card.Rect.DORotate(new Vector3(0, 0, 0), 0.4f).Pause());

                seq.Play();
            }
        }
        else
            PlayCardsAnimation();
        
    }

    IEnumerator AddCard()
    {
        while(_placingCards.Count > 0)
        {
            if (_completeAnimation == false)
                yield return null;
            else
            {
                _completeAnimation = false;
                string cardID = _placingCards[0];
                _placingCards.RemoveAt(0);
                VirtualCard card = this._allCards.LastOrDefault<VirtualCard>(card => card.Visible == false);
                if (card == null)
                {
                    Debug.LogError("No avaliable cards");
                    _completeAnimation = true;
                    yield return null;
                }
                card.ReadCard(cardID);
                card.GetTransform.localPosition = this._startCardPosition;
                card.GetTransform.localScale = new Vector3(0f, 0f, 1f);
                card.GetTransform.localEulerAngles = new Vector3(0f, 0f, 0f);
                card.Visible = true;
                PlayCardsAnimation();
            }
        }
    }

    private void PlayCardsAnimation()
    {
        List<VirtualCard> animate = this._allCards.Where<VirtualCard>(card => card.Visible == true).ToList();

        if (animate.Count == 1)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(animate[0].GetTransform.DOLocalMove(new Vector3(_centerPosition.x, _centerPosition.y, 1f), 0.5f).Pause());
            seq.Join(animate[0].GetTransform.DORotate(new Vector3(0f, 0f, 0f), 0.5f).Pause());
            seq.Join(animate[0].GetTransform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f).Pause());
            seq.OnComplete(this.AnimationComplete);
            seq.Play();
        }
        else
        {
            if (animate.Count % 2 == 1)
            {
                int halfOfAmount = (animate.Count - 1) / 2;
                int startRotate = 5 * halfOfAmount;
                int startXPosition = (int)(_centerPosition.x - (_cardSize / 2) * halfOfAmount);
                float yPosition = _centerPosition.y;
                

                Sequence allSeq = DOTween.Sequence();

                for (int i = 0; i < animate.Count; i++)
                {
                    if (i < halfOfAmount)
                    {
                        yPosition = _centerPosition.y - 20 * (halfOfAmount - i);
                    }
                    else if (i > halfOfAmount)
                    {
                        yPosition = _centerPosition.y - 20 * (i - halfOfAmount + 1);
                    }
                    else
                        yPosition = _centerPosition.y;

                    float duration = 0.4f;
                    if (i == 0)
                        duration = 0.5f;

                    Vector3 position = new Vector3(startXPosition + (_cardSize / 2) * i, yPosition, 0f);

                    Sequence seq = DOTween.Sequence();
                    seq.Append(animate[i].Rect.DOLocalMove(position, duration));
                    seq.Join(animate[i].Rect.DOLocalRotate(new Vector3(0f, 0f, startRotate - 5 * i), duration));
                    seq.Join(animate[i].Rect.DOScale(new Vector3(0.8f, 0.8f, 0.8f), duration));

                    if (i == 0)
                        allSeq.Append(seq);
                    else
                        allSeq.Join(seq);
                }

                allSeq.OnComplete(this.AnimationComplete);
                allSeq.Play();
            }
            else
            {
                int halfOfAmount = animate.Count/ 2;
                float yPosition = _centerPosition.y;
                float rotationPosition = 0;
                int startXPosition = (int)(_centerPosition.x - (_cardSize/2) * (animate.Count / 2));

                Sequence allSeq = DOTween.Sequence();

                for (int i = 0; i < animate.Count; i++)
                {
                    if (i < halfOfAmount)
                    {
                        yPosition = _centerPosition.y - 20 * (halfOfAmount - i);
                        rotationPosition = 2.5f * (halfOfAmount - i);
                    }
                    else if (i > halfOfAmount)
                    {
                        yPosition = _centerPosition.y - 20 * (i - halfOfAmount + 1);
                        rotationPosition = -2.5f * (i - halfOfAmount + 1);
                    }
                    else
                    {
                        yPosition = _centerPosition.y - 20;
                        rotationPosition = -2.5f;
                    }
                        

                    float duration = 0.4f;
                    if (i == 0)
                        duration = 0.5f;

                    Sequence seq = DOTween.Sequence();
                    seq.Append(animate[i].Rect.DOLocalMove(new Vector3(startXPosition + (_cardSize / 2) * i, yPosition, 1f), duration));
                    seq.Join(animate[i].Rect.DOLocalRotate(new Vector3(0f, 0f, rotationPosition), duration));
                    seq.Join(animate[i].Rect.DOScale(new Vector3(0.8f, 0.8f, 0.8f), duration));

                    if (i == 0)
                        allSeq.Append(seq);
                    else
                        allSeq.Join(seq);
                }

                allSeq.OnComplete(this.AnimationComplete);
                allSeq.Play();
            }
        }
    }

    public override void AnimationComplete()
    {
        _completeAnimation = true;
    }

    public void DiscardAll()
    {
        for (int i = 0; i < this._allCards.Count; i++)
        {
            this._allCards[i].Visible = false;
        }
    }
}
