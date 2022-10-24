using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

namespace BattleEngine
{
    public class LightInDark : CardAction
    {
        public override string ActionType => "LightInDark";

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