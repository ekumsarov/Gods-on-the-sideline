using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "AnimationAsset", menuName = "UIeXAnimation/AnimationAsset")]
public class AnimationAsset : ScriptableObject
{
    [SerializeField] private string _animationID;
    public string AnimaitionID => _animationID;

    [SerializeField] private List<AnimationData> _animations;
    public List<AnimationData> AnimationsData => _animations;
}
