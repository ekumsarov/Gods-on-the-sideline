using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lodkod;

namespace BattleEngine
{
    [System.Serializable]
    public class UnitFeature : MonoBehaviour, ICloneable
    {
        protected AIUnit _parent;
        protected iBattleUnit _target;
        [SerializeField] protected UnitFeatureResponse _responseTime;
        public UnitFeatureResponse ResponseTime => _responseTime;

        public void Initialized(AIUnit parent)
        {
            _parent = parent;
            Setup();
        }

        protected virtual void Setup()
        {

        }

        public virtual void PlayFeature()
        {

        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}