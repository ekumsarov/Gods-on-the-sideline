using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleActions;

namespace BattleEngine
{
    public class AIPlayer : TurnLogic
    {
        private int _currentUnit = 0;

        public AIPlayer(MonoBehaviour coroutineHolder)
        {
            CoroutineHolder = coroutineHolder;
            ActionQuery = new List<IBattleFunction>();
        }

        public override void Start(Action callback)
        {
            base.Start(callback);

            _currentUnit = -1;

            ActionQuery.Add(CallbackAction.Get(() => { BattleLog.PushLog("Start Enemy turn"); }));
            ActionQuery.Add(CallbackAction.Get(NextUnit));
            
            CoroutineHolder.StartCoroutine(Update());
        }

        private void NextUnit()
        {
            _currentUnit += 1;

            if(_currentUnit >= BattleSystem.Enemies.Count)
            {
                Complete();
                return;
            }

            if(BattleSystem.Enemies[_currentUnit].CanActivate)
            {
                ActionQuery.AddRange(BattleSystem.Enemies[_currentUnit].EffectSystem.GetEffectActions());
                ActionQuery.Add(CallbackAction.Get(BattleSystem.Enemies[_currentUnit].EffectSystem.EffectTurnEnd));
                ActionQuery.Add(CallbackAction.Get(() => 
                {
                    ActionQuery.Add(BattleSystem.Enemies[_currentUnit]);
                    ActionQuery.Add(CallbackAction.Get(NextUnit));
                    //BattleSystem.Enemies[_currentUnit].Start(NextUnit);
                } ));
            }
            else
            {
                ActionQuery.AddRange(BattleSystem.Enemies[_currentUnit].EffectSystem.GetEffectActions());
                ActionQuery.Add(CallbackAction.Get(BattleSystem.Enemies[_currentUnit].EffectSystem.EffectTurnEnd));
                ActionQuery.Add(CallbackAction.Get(NextUnit));
            }
                
        }

        public override void Complete()
        {
            for(int i = 0; i < BattleSystem.Enemies.Count; i++)
            {
                ActionQuery.Add(CallbackAction.Get(BattleSystem.Enemies[i].NextRound));
            }
            ActionQuery.Add(CallbackAction.Get(base.Complete));
        }

        IEnumerator Update()
        {
            bool isPlaying = false;
            while (true)
            {
                if (isPlaying == false && ActionQuery.Count > 0)
                {
                    isPlaying = true;

                    ActionQuery[0].StartFunction(() => {
                        isPlaying = false;
                        ActionQuery.RemoveAt(0);
                        InsertCount = 1;
                    });
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }
}