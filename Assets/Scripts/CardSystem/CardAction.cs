using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using CardTargetSelection;
using System;

namespace BattleEngine
{
    public abstract class CardAction : IBattleFunction
    {
        private Action _trigger;
        public abstract string ActionType { get; }

        protected string CardID;
        protected string HeroID;

        protected CardActionTarget _target;
        public abstract void Init(CardActionTypes data, string cardID);
        public virtual bool CanStart()
        {
            if (_target.CanChooseTarget(Play) == false)
                return false;

            return true;
        }

        public void StartFunction(Action trigger)
        {
            _trigger = trigger;

            if (CanStart())
                Play();
            else
                Complete();
        }
        protected abstract void Play();
        public void Complete()
        {
            _trigger?.Invoke();
        }

        protected int CurrentActionCount = 0;
        protected int ActionCount = 0;

        public void CompleteAction()
        {
            CurrentActionCount += 1;
            if(CurrentActionCount >= ActionCount)
            {
                Complete();
            }
        }

        protected virtual void TargetSetup(CardActionTypes data)
        {
            _target = CardActionTarget.Create(data);
        }

        public virtual string GetDescription()
        {
            return "Not setup";
        }

        public virtual string GetDescription(CardActionTypes data)
        {
            return "Not Setup";
        }
    }
}