using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

namespace BattleEngine
{
    public class UnitSummon : UnitAction
    {
        [SerializeField] private string _summonID = string.Empty;
        [SerializeField] private int _summonCount = 1;

        public override void Initialized(AIUnit parent)
        {
            _actionID = "UnitSummon";
            this._actionType = UnitActionType.Summon;
            base.Initialized(parent);
        }

        protected override void StartAction()
        {
            for (int i = 0; i < _summonCount; i++)
            {
                BattleSystem.AddUnit(Unit.Make(_summonID), false);
            }

            Done();
        }
    }

}