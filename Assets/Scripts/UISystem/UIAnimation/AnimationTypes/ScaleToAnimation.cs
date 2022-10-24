using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "ScaleAnimation", menuName = "UIeXAnimation/ScaleAnimation")]
public class ScaleToAnimation : AnimationData
{
    [SerializeField] private Vector3 _endPoint;

    public override Tween GetTween(UIeX parent)
    {
        return parent.Rect.DOScale(_endPoint, _duration).SetEase(_curve);
    }
}