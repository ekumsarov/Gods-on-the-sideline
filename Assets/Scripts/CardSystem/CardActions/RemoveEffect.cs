using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using System.Linq;

namespace BattleEngine
{
    public class RemoveEffect : CardAction
    {
        public override string ActionType => CardActionType.RemoveEffect.ToString();

        private int _amount;
        private int _roundCount;
        private List<EffectType> _effectType;
        public override void Init(CardActionTypes data, string heroID)
        {
            this._amount = data.dEffectAmount;
            this._roundCount = data.dEffectroundCount;
            this._effectType = data.dRemoveEffectType;
            this.HeroID = heroID;
            this.TargetSetup(data);
        }

        protected override void Play()
        {
            if (_target.IsTargetSelected() == false)
            {
                _target.SetTarget();
                return;
            }

            CurrentActionCount = 0;

            ActionCount = _target.GetTargets.Count;

            for (int i = 0; i < _target.GetTargets.Count; i++)
            {
                if(_effectType.Any(effect => effect == EffectType.Empty))
                    _target.GetTargets[i].EffectSystem.RemoveAllEffects();
                else
                {
                    for(int j = 0; j < _effectType.Count; j++)
                    {
                        _target.GetTargets[i].EffectSystem.RemoveEffect(_effectType[j].ToString());
                    }
                }
            }
        }

        public override string GetDescription()
        {
            string effecttype = "";
            if (_effectType.Any(effect => effect == EffectType.Empty))
                effecttype = LocalizationManager.Get("EmptyEffectDescription");
            else
            {
                if(_effectType.Count > 1)
                {
                    for(int i = 0; i < _effectType.Count; i++)
                    {
                        if(i == _effectType.Count-1)
                        {
                            effecttype += LocalizationManager.Get(_effectType[i].ToString() + "Des");
                        }
                        else
                            effecttype += LocalizationManager.Get(_effectType[i].ToString() + "Des") + ", ";
                    }
                }
                else
                    effecttype = LocalizationManager.Get(_effectType[0].ToString() + "Des");
            }
                
            return LocalizationManager.Get("RemoveEffectDescription", effecttype);
        }

        public override string GetDescription(CardActionTypes data)
        {
            string effecttype = "";
            if (data.dRemoveEffectType.Any(effect => effect == EffectType.Empty))
                effecttype = LocalizationManager.Get("EmptyEffectDescription");
            else
            {
                if (data.dRemoveEffectType.Count > 1)
                {
                    for (int i = 0; i < data.dRemoveEffectType.Count; i++)
                    {
                        if (i == data.dRemoveEffectType.Count - 1)
                        {
                            effecttype += LocalizationManager.Get(data.dRemoveEffectType[i].ToString() + "Des");
                        }
                        else
                            effecttype += LocalizationManager.Get(data.dRemoveEffectType[i].ToString() + "Des") + ", ";
                    }
                }
                else
                    effecttype = LocalizationManager.Get(data.dRemoveEffectType[0].ToString() + "Des");
            }

            return LocalizationManager.Get("RemoveEffectDescription", effecttype);
        }
    }
}