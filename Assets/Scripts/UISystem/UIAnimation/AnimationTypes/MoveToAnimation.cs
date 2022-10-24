using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "MoveToAnimation", menuName = "UIeXAnimation/MoveToAnimation")]
public class MoveToAnimation : AnimationData
{
    [SerializeField] private Vector2 _endPoint;

    public override Tween GetTween(UIeX parent)
    {
        return parent.Rect.DOMove(_endPoint, _duration).SetEase(_curve);
    }
}