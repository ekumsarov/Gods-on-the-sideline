using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleEngine;
using Lodkod;

public interface iBattleUnit
{
    public int CurrentHP { get; set; }
    public int HP { get; }

    public BattleUnitView View { set; }

    public string Name { get; }
    public string Icon { get; }
    public string Description { get; }

    public List<SkillType> Sustainbility { get; }

    public List<SkillType> Susceptibility { get; }

    public void ShowInfo();
    public void CloseInfo();

    public Dictionary<string, SkillObject> Skills { get; }

    public List<string> EffectsImmune { get; }

    public void AddSkills(string skill, int min, int max, string to, string id);

    public void RemoveSkills(string from, string id);

    public void RemoveSkills(string skill, int min, int max);

    public void RemoveSkillsFrom(string from, int min, int max);

    public void UpAllSkills(int min, int max, string to);

    public void Damage(ActionSourceData sourceData, System.Action callback);

    public void Heal(int amount, string from, System.Action callback);

    public void CompleteAnimation();

    public UnitEffect EffectSystem { get; }

    public void ActivateEffect();

    public bool IsActive { get; }

    public bool CanActivate { get; }

    public CardTargetSide UnitSide { get; }

    public void PrepareForBattle();
}
