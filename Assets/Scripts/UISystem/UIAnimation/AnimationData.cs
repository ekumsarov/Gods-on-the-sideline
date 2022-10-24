using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class AnimationData : ScriptableObject
{
    [SerializeField] protected float _delay = 0f;
    public float Delay => _delay;
    [SerializeField] protected float _duration = 1f;
    [SerializeField] protected AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);

    public virtual Tween GetTween(UIeX parent)
    {
        return null;
    }
}
