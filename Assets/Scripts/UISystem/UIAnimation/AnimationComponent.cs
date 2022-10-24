using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimationComponent
{
    public static AnimationComponent Get(UIeX parent)
    {

        return new AnimationComponent() { _sequences = new Dictionary<string, Sequence>(), _parent = parent };
    }

    private Dictionary<string,Sequence> _sequences;
    private UIeX _parent;

    public void AddSequence(Sequence seq, string id)
    {
        seq.Pause();
        seq.SetId(id);
        _sequences.Add(id, seq);
    }
    
    public void ReadSequnce(AnimationAsset asset)
    {
        if(_sequences.ContainsKey(asset.AnimaitionID))
        {
            Debug.LogError("Already have animation:" + asset.AnimaitionID);
            return;
        }

        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < asset.AnimationsData.Count; i++)
        {
            
            if (asset.AnimationsData[i].Delay > 0)
                seq.AppendInterval(asset.AnimationsData[i].Delay);
            seq.Append(asset.AnimationsData[i].GetTween(_parent));
        }

        seq.SetId(asset.AnimaitionID);
        seq.OnComplete(_parent.AnimationComplete);
        _sequences.Add(asset.AnimaitionID, seq);
    }

    public void Play(string animationID)
    {
        if(_sequences.ContainsKey(animationID) == false)
        {
            Debug.LogError("NoAnimationSet");
            return;
        }

        _sequences[animationID].Restart();
        _sequences[animationID].Play();
    }

    public void Play(AnimationAsset asset)
    {
        if(_sequences.ContainsKey(asset.AnimaitionID) == false)
        {
            this.ReadSequnce(asset);
        }

        _sequences[asset.AnimaitionID].Restart();
        _sequences[asset.AnimaitionID].Play();
    }

    public void Stop(string animationID)
    {
        if (_sequences.ContainsKey(animationID) == false)
        {
            Debug.LogError("NoAnimationSet");
            return;
        }

        _sequences[animationID].Complete();
    }

    public void StopAll()
    {
        foreach(var seq in _sequences)
        {
            seq.Value.Complete();
        }
    }
}
