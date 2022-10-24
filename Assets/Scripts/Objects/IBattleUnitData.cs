using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleUnitData
{
    public int CurrentHP { get; set; }
    public int HP { get; }

    public string Name { get; }
    public string Icon { get; }
    public string Description { get; }

    public Dictionary<string, SkillObject> Skills { get; }

    public void SetNameIfNull(string name);
}