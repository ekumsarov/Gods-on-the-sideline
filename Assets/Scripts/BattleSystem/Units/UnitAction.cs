using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Lodkod;

namespace BattleEngine
{
    [System.Serializable]
    public class UnitAction : MonoBehaviour, ICloneable, IBattleFunction
    {
        protected string _actionID = "UnitAction";
        public string ActionID => _actionID;

        [SerializeField] private CardTargetSide _targetSide;
        public CardTargetSide TargetSide => _targetSide;

        [SerializeField] private int _coolDown = -1;
        private int _currentCoolDown = 0;

        protected UnitActionType _actionType = UnitActionType.Attack;
        public UnitActionType ActionType => _actionType;

        [SerializeField] private bool HasCondition = false;

        [SerializeField] [ConditionalField("HasCondition", true)] protected ConditionUseType _conditionUseType;
        [SerializeField] [ConditionalField("HasCondition", true)] protected List<Condition> _conditions;

        protected AIUnit _parent;
        public virtual void Initialized(AIUnit parent)
        {
            _parent = parent;
        }

        public bool OnCooldown => _currentCoolDown > 0;

        public void ReduceCoolDown()
        {
            _currentCoolDown -= 1;
            if (_currentCoolDown < 0)
                _currentCoolDown = 0;
        }

        public void ReduceCoolDownToZero()
        {
            _currentCoolDown = 0;
        }

        public void SetCoolDownTo(int amount)
        {
            _currentCoolDown = amount;
        }

        Action _callback = null;
        public void StartFunction(Action trigger)
        {
            _callback = trigger;

            if(_conditionUseType == ConditionUseType.ForAvaliable && _conditions != null)
            {
                for(int i = 0; i < _conditions.Count; i++)
                {
                    if (_conditions[i].Available == false)
                    {
                        Done();
                        return;
                    }
                }
            }

            StartAction();
        }

        public void Complete()
        {
            _callback?.Invoke();
        }    

        protected virtual void StartAction()
        {
            Done();
        }

        private int _currentTargetAmount = 0;
        protected List<iBattleUnit> _targets;
        public virtual void BindTargets(List<iBattleUnit> targets)
        {
            _targets = targets;
            _currentTargetAmount = 0;
        }

        private void MakeAction(List<iBattleUnit> target)
        {
            Debug.LogError(_parent.Name + " played empty action");
            Done();
        }

        
        protected void Done()
        {
            _currentTargetAmount += 1;
            if (_targets != null && _targets.Count > 0 && _currentTargetAmount < _targets.Count)
                return;

            _currentTargetAmount = 0;
            _targets = null;

            if (_coolDown > -1)
            {
                _currentCoolDown = _coolDown + 1;
            }
            Complete();
        }

        public virtual int GetActionAmount()
        {
            return 0;
        }

        public virtual string GetActionID()
        {
            return string.Empty;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}