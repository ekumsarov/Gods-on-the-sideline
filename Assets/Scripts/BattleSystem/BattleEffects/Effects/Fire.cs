using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

namespace BattleEffects
{
    public class Fire : BattleEffect
    {
        public override void PrepareBattleEffect(CardActionTypes data)
        {
            this.ID = "Fire";
            this._effectType = EffectType.Fire;
            this._buffType = EffectBuffType.Negative;

            this._amount = data.dEffectAmount;
        }

        public override void PrepareBattleEffect(int data)
        {
            this.ID = "Fire";
            this._effectType = EffectType.Fire;

            this._amount = data;
        }

        public override bool CanApplyEffect()
        {
            return base.CanApplyEffect();
        }

        public override void SumEffects(BattleEffect effect)
        {
            base.SumEffects(effect);

            if (_amount > 5)
                _amount = 5;
        }

        public override void StartEffect()
        {
            if(this.Object.UnitSide == CardTargetSide.Player)
            {
                foreach(var skill in Object.Skills)
                {
                    Object.AddSkills(skill.Value.ID, -1, -2, Object.Name, "Fire" + skill.Value.ID);
                }
            }
        }

        public override void DestroyEffect()
        {
            if(Object.UnitSide == CardTargetSide.Player)
            {
                foreach (var skill in Object.Skills)
                {
                    Object.RemoveSkills("effect", "Fire" + skill.Value.ID);
                }
            }
            
            base.DestroyEffect();
        }
    }

}