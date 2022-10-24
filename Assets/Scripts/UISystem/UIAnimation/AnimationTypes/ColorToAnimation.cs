using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "ColorToAnimation", menuName = "UIeXAnimation/ColorToAnimation")]
public class ColorToAnimation : AnimationData
{
    [SerializeField] private Color _endPoint;

    public override Tween GetTween(UIeX parent)
    {
        return DOTween.To(() => parent.Color, x => parent.Color = x, _endPoint, _duration).SetEase(_curve);
    }
}