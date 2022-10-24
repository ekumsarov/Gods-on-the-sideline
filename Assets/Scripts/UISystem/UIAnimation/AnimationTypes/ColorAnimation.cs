using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "ColorAnimation", menuName = "UIeXAnimation/ColorAnimation")]
public class ColorAnimation : AnimationData
{
    [SerializeField] private Color _startPoint;
    [SerializeField] private Color _endPoint;

    public override Tween GetTween(UIeX parent)
    {
        return DOTween.To(() => parent.Color, x => parent.Color = x, _endPoint, _duration).From(_startPoint).SetEase(_curve);
    }
}