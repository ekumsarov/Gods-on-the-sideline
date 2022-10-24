using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleActions;

namespace BattleEngine
{
    public class PlayerLogic : TurnLogic
    {
        int actionCount = 0;

        protected override void Initialized()
        {
            base.Initialized();

            BattleSystem.Menu.BindCardAction(PlayCard);
        }

        public PlayerLogic(MonoBehaviour coroutineHolder)
        {
            CoroutineHolder = coroutineHolder;
            ActionQuery = new List<IBattleFunction>();
        }

        public override void Start(Action callback)
        {
            base.Start(callback);

            SetActionCount(GSM.GameSettings.CardActions);

            for (int i = 0; i < BattleSystem.Heroes.Count; i++)
            {
                ActionQuery.AddRange(BattleSystem.Heroes[i].EffectSystem.GetEffectActions());
            }

            /*for (int i = 0; i < BattleSystem.Heroes.Count; i++)
            {
                ActionQuery.Add(CallbackAction.Get(BattleSystem.Heroes[i].EffectSystem.EffectTurnEnd));
            }*/

            ActionQuery.Add(CallbackAction.Get(() => { BattleLog.PushLog("Start player turn"); }));
            ActionQuery.Add(CallbackAction.Get(DeckSystem.ChooseCards));
            ActionQuery.Add(CallbackAction.Get(BattleSystem.Menu.NextTurn));

            BattleSystem.Menu.ActionTextChange(actionCount);

            CoroutineHolder.StartCoroutine(Update());
        }

        public void PlayCard(Card card)
        {
            BattleSystem.Menu.PlayerInteract(false);

            if (actionCount >= card.CardCost)
            {
                SetActionCount(-card.CardCost);
                BattleSystem.Menu.ActionTextChange(actionCount);
                card.PlayCard();
                ActionQuery.Add(CallbackAction.Get(BattleSystem.Menu.ClearOnPlayerTurn));
                ActionQuery.Add(CallbackAction.Get(EnableCardInteract));
                return;
            }

            ActionQuery.Add(CallbackAction.Get(BattleSystem.Menu.ClearOnPlayerTurn));
            BattleSystem.Menu.PlayerInteract(true);
        }

        public void EnableCardInteract()
        {
            BattleSystem.Menu.PlayerInteract(true);
        }

        public void SetActionCount(int count)
        {
            actionCount += count;
            if (actionCount <= 0)
            {
                actionCount = 0;
                BattleSystem.Menu.LockCardLayout();
            }
            else
                BattleSystem.Menu.UnlockCardLayout();
                

            BattleSystem.Menu.ActionTextChange(actionCount);
        }

        public override void Complete()
        {
            for(int i = 0; i < BattleSystem.HeroesFamiliars.Count; i++)
            {
                ActionQuery.Add(BattleSystem.HeroesFamiliars[i]);
            }

            for (int i = 0; i < BattleSystem.Heroes.Count; i++)
            {
                ActionQuery.Add(CallbackAction.Get(BattleSystem.Heroes[i].EffectSystem.EffectTurnEnd));
            }

            ActionQuery.Add(CallbackAction.Get(BattleSystem.Menu.EndPlayerTurn));
            ActionQuery.Add(CallbackAction.Get(base.Complete));
        }

        IEnumerator Update()
        {
            bool isPlaying = false;
            while (true)
            {
                if (!isPlaying && ActionQuery.Count > 0)
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

        #region getting data

        public int GetActionCount => actionCount;

        #endregion

    }
}