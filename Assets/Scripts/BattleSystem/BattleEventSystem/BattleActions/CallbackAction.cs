using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BattleEngine;

namespace BattleActions
{
    public class CallbackAction : BattleAction
    {
        private Action _functionCall;

        public static void Create(Action callback)
        {
            CallbackAction temp = new CallbackAction();

            temp._functionCall = callback;

            BattleSystem.AddFunctionToQuery(temp);
        }

        public static CallbackAction Get(Action callback)
        {
            CallbackAction temp = new CallbackAction();

            temp._functionCall = callback;

            return temp;
        }

        public override void StartFunction(Action callback)
        {
            base.StartFunction(callback);
            _functionCall?.Invoke();
            Complete();
        }
    }
}
