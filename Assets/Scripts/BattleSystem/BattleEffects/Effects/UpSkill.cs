using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

namespace BattleEffects
{
    public class UpSkill : BattleEffect
    {
        public override void PrepareBattleEffect(CardActionTypes data)
        {
            this.ID = "UpSkill";
            this._effectType = EffectType.UpSkill;
            this._buffType = EffectBuffType.Positive;

            this._amount = data.dEffectAmount;
        }

        public override void PrepareBattleEffect(int amount)
        {
            this.ID = "UpSkill";
            this._effectType = EffectType.UpSkill;

            this._amount = amount;
        }

        public override void StartEffect()
        {
            base.StartEffect();

            this.Object.UpAllSkills(1, 2, "effect");
        }

        public override bool CanApplyEffect()
        {
            return base.CanApplyEffect();
        }

        public override void DestroyEffect()
        {
            this.Object.RemoveSkillsFrom("effect", 1, 2);

            base.DestroyEffect();
        }
    }

}