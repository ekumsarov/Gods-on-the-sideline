using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "MoveAnimation", menuName = "UIeXAnimation/MoveAnimation")]
public class MoveAnimation : AnimationData
{
    [SerializeField] private Vector2 _startPoint;
    [SerializeField] private Vector2 _endPoint;

    public override Tween GetTween(UIeX parent)
    {
        if (_startPoint == Vector2.zero)
        {
            return parent.Rect.DOMove(_endPoint, _duration).SetEase(_curve);
        }
        else
        {
            return parent.Rect.DOMove(_endPoint, _duration).From(_startPoint).SetEase(_curve);
        }
    }
}
