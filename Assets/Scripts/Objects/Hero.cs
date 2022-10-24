using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using BattleEffects;
using System.Linq;
using BattleEngine;

public class Hero : AIUnit, IHeroData
{
    [SerializeField] private SkillType _class;
    public SkillType Class => _class;

    private List<string> _equippedLoot;
    public List<string> EquippedLoot => _equippedLoot;

    public static Hero Make(HeroData data)
    {
        Hero temp = new Hero();

        temp._name = data.Name;
        temp._icon = data.Icon;
        temp._class = data.Class;
        temp._hp = data.HP;
        temp._currentHP = temp._hp;
        temp._unitSide = CardTargetSide.Player;
        temp._unitType = UnitType.Hero;
        temp._damageRule = new DamageRule();
        temp._healRule = new HealRule();
        temp.ActivateEffect();

        temp._damageRule.SetupParent(temp);
        temp._healRule.SetupParent(temp);

        temp._skills = new Dictionary<string, SkillObject>();
        temp._heroSkills = new Dictionary<string, SkillObject>();
        temp._lootSkill = new Dictionary<string, SkillObject>();
        temp._effectSkill = new Dictionary<string, SkillObject>();

        foreach (var skill in data.Skills)
        {
            temp._skills.Add(skill.Key, skill.Value);
            temp._heroSkills.Add(skill.Key, skill.Value);
            temp._lootSkill.Add(skill.Key, SkillObject.Make(skill.Key, 0, 0));
            temp._effectSkill.Add(skill.Key, SkillObject.Make(skill.Key, 0, 0));
        }

        return temp;
    }
}
