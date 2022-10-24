using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using BattleEffects;
using System.Linq;
using BattleEngine;

namespace BattleEngine
{
    public class AIUnit : Unit, IBattleFunction
    {
        protected SkillType _class;
        public SkillType Class => _class;

        protected UnitAIType _logicType;
        public UnitAIType LogicType => _logicType;

        protected UnitLogic _logic;

        protected int _baseDamage;
        public int BaseDamage => _baseDamage;

        protected List<UnitAction> _actions;

        protected List<Condition> _selectUnitConditions;

        public static new AIUnit Make(UnitData data)
        {
            AIUnit temp = new AIUnit();

            temp._actions = new List<UnitAction>();
            for (int i = 0; i < data.Actions.Count; i++)
            {
                temp._actions.Add((UnitAction)data.Actions[i].Clone());
            }

            temp._features = new List<UnitFeature>();
            for (int i = 0; i < data.Features.Count; i++)
            {
                temp._features.Add((UnitFeature)data.Features[i].Clone());
            }

            temp._logic = data.Logic;
            temp._logicType = data.LogicType;
            temp._unitSide = data.Side;
            temp._name = BattleSystem.CheckNames(data.Name);
            temp._icon = data.Icon;
            temp._class = data.Class;
            temp._hp = data.HP;
            temp._currentHP = temp._hp;
            temp._unitType = UnitType.Enemy;
            temp._baseDamage = data.BaseDamage;
            temp._selectUnitConditions = data.SelectUnitConditions;
            temp._susceptibility = data.Susceptibility;
            temp._sustainability = data.Sustainability;
            
            temp._healRule = data.HealRule;
            temp.ActivateEffect();

            if (data.DamageRule != null)
                temp._damageRule = (DamageRule)data.DamageRule.Clone();
            else
                temp._damageRule = new DamageRule();

            if (data.HealRule != null)
                temp._healRule = (HealRule)data.HealRule.Clone();
            if (temp._healRule == null)
                temp._healRule = new HealRule();

            if (temp._logic == null)
                temp._logic = new UnitLogic();

            temp._damageRule.SetupParent(temp);
            temp._healRule.SetupParent(temp);

            for (int i = 0; i < temp._actions.Count; i++)
            {
                temp._actions[i].Initialized(temp);
            }

            for (int i = 0; i < temp._features.Count; i++)
            {
                temp._features[i].Initialized(temp);
            }


            return temp;
        }

        private System.Action _callback;
        public void StartFunction(System.Action callback)
        {
            _callback = callback;
            StartAction();
        }

        public void Complete()
        {
            _callback?.Invoke();
        }

        protected virtual void StartAction()
        {
            // ForTest 
            //UnitLogicResult result = UnitLogic.DoLogic(this.LogicType, this._actions, this.UnitSide);

            UnitLogicResult result = _logic.DoLogic(this._actions, this.UnitSide);

            if(result.Action.ActionType == UnitActionType.Summon)
            {
                result.Action.BindTargets(result.Targets);
                result.Action.StartFunction(EndAction);
                result = null;
                return;
            }

            if (result == null || result.Action == null || result.Targets == null || result.Targets.Count == 0)
            {
                Debug.LogError("Enemy did not found units or hero");
                EndAction();
                return;
            }

            result.Action.BindTargets(result.Targets);
            result.Action.StartFunction(EndAction);
            result = null;
        }

        public void EndAction()
        {
            //this.EffectTurnEnd();
            this._completeTurn = true;
            Complete();
        }

        public override void NextRound()
        {
            for (int i = 0; i < this._actions.Count; i++)
            {
                this._actions[i].ReduceCoolDown();
            }
            base.NextRound();
        }
    }
}