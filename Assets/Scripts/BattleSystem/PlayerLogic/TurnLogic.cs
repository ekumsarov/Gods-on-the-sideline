using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleEngine
{
    public class TurnLogic
    {
        private Action _onComplete;
        protected MonoBehaviour CoroutineHolder;
        protected int InsertCount = 1;

        protected List<IBattleFunction> ActionQuery;

        protected virtual void Initialized()
        {

        }

        public static TurnLogic Create(bool isPlayer, MonoBehaviour coroutineHolder)
        {
            if (isPlayer)
            {
                PlayerLogic temp = new PlayerLogic(coroutineHolder);
                temp.Initialized();
                return temp;
            }
                

            return new AIPlayer(coroutineHolder);
        }

        public virtual void Start(Action callback)
        {
            _onComplete = callback;
        }

        public virtual void Complete()
        {
            CoroutineHolder.StopAllCoroutines();
            _onComplete?.Invoke();
        }

        public virtual void PlayAction(IBattleFunction action)
        {
            ActionQuery.Add(action);
        }

        // Using then need sub query after main action complete
        public virtual void InsertAction(IBattleFunction action)
        {
            ActionQuery.Insert(InsertCount, action);
            InsertCount++;
        }
    }
}