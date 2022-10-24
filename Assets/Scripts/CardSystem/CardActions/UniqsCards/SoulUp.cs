using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

namespace BattleEngine
{
    public class SoulUp : CardAction
    {
        public override string ActionType => "SoulUp";

        public override void Init(CardActionTypes data, string heroID)
        {
            this.HeroID = heroID;
            this.TargetSetup(data);
        }
        public override bool CanStart()
        {
            return true;
        }

        protected override void Play()
        {
        }
    }
}
