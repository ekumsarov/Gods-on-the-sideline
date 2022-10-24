using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lodkod;

namespace BattleEngine
{
    public class BattleAction : IBattleFunction
    {
        #region parameters

        public Action Callback;
        protected BattleResponseActions _responseType = BattleResponseActions.Empty;
        public BattleResponseActions Response => _responseType;

        #endregion

        public virtual void StartFunction(Action callback)
        {
            Callback = callback;
        }

        public virtual void Update()
        {
            Complete();
        }

        public virtual void Complete()
        {
            Callback?.Invoke();
        }
    }
}