using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using CardTargetSelection;

namespace BattleEngine
{
    public class Summon : CardAction
    {
        public override string ActionType => CardActionType.Summon.ToString();

        private string summonID;

        public override void Init(CardActionTypes data, string heroID)
        {
            summonID = data.dPetID;
            this.HeroID = heroID;
            this.TargetSetup(data);
        }
        public override bool CanStart()
        {
            return BattleSystem.CanAddUnit(true);
        }

        protected override void Play()
        {
            BattleSystem.AddUnit(Unit.Make(summonID), true);
            Complete();
        }

        public override string GetDescription(CardActionTypes data)
        {
            return LocalizationManager.Get("SummonDescription", LocalizationManager.Get(data.dPetID)) + " " + CardActionTarget.GetTargetDescription(data.dTarget, data.dTargetSide, data.dTargetIncludes);
        }
    }

}

