using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

namespace BattleEngine
{
    public class TestCard : CardAction
    {
        public override string ActionType => "TestCard";

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