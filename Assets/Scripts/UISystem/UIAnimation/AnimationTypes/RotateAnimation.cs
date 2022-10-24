using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "RotateAnimation", menuName = "UIeXAnimation/RotateAnimation")]
public class RotateAnimation : AnimationData
{
    [SerializeField] private Vector3 _startPoint;
    [SerializeField] private Vector3 _endPoint;

    public override Tween GetTween(UIeX parent)
    {
        if (_startPoint == Vector3.zero)
        {
            return parent.Rect.DORotate(_endPoint, _duration).SetEase(_curve);
        }
        else
        {
            return parent.Rect.DORotate(_endPoint, _duration).From(_startPoint).SetEase(_curve);
        }
    }
}