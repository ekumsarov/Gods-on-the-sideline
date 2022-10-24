using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "RotateToAnimation", menuName = "UIeXAnimation/RotateToAnimation")]
public class RotateToAnimation : AnimationData
{
    [SerializeField] private Vector3 _endPoint;

    public override Tween GetTween(UIeX parent)
    {
        return parent.Rect.DORotate(_endPoint, _duration).SetEase(_curve);
    }
}