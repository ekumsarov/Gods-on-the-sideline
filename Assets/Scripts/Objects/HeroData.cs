using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Lodkod;
using SimpleJSON;

[CreateAssetMenu(fileName = "New Hero", menuName = "Hero")]
public class HeroData : ScriptableObject, IHeroData, IBattleUnitData
{
    [SerializeField] private List<CardData> _group1;
    public List<CardData> Group1 => _group1;

    [SerializeField] List<CardData> _group2;
    public List<CardData> Group2 => _group2;

    [SerializeField] private List<CardData> _group3;
    public List<CardData> Group3 => _group3;

    [SerializeField] private SkillType _class;
    public SkillType Class => _class;

    private int _currentHP;
    public int CurrentHP
    {
        get => _currentHP;
        set { _currentHP += value; }
    }

    [SerializeField] private int _hp;
    public int HP => _hp;

    [SerializeField] private string _name;
    public string Name => _name;

    [SerializeField] private string _icon;
    public string Icon => _icon;

    [SerializeField] private string _description;
    public string Description => _description;

    private List<string> _equippedLoot;
    public List<string> EquippedLoot => _equippedLoot;

    [SerializeField] private List<string> _effectsImmune;
    public List<string> EffectsImmune => _effectsImmune;


    [SerializeField] private List<SkillDataFull> _skillsData;
    private Dictionary<string, SkillObject> _skills;
    public Dictionary<string, SkillObject> Skills
    {
        get
        {
            if(_skills == null)
            {
                _skills = new Dictionary<string, SkillObject>();
                foreach(var skill in _skillsData)
                {
                    if (_skills.ContainsKey(skill.SkillType))
                        continue;

                    _skills.Add(skill.SkillType, SkillObject.Make(skill));
                }
            }

            return _skills;
        }
    }

    public static HeroData Make(JSONNode data)
    {
        HeroData temp = new HeroData();

        if (data["Name"] != null)
            temp._name = data["Name"].Value;
        else
            throw new System.Exception("Not set name on hero");

        if(data["Skills"] == null)
        {
            if (IOM.HasHeroData(temp._name))
                temp = IOM.GetHeroData(temp._name);
            else
                throw new System.Exception("Not found hero id: " + temp._name);

            return temp;
        }
        else
        {
            JSONArray skillArray = data["Skills"].AsArray;
            temp._skills = new Dictionary<string, SkillObject>();
            for (int i = 0; i < skillArray.Count; i++)
            {
                temp._skills.Add(skillArray[i]["id"].Value, SkillObject.Make(skillArray[i]));
            }
        }

        if (data["Description"] != null)
            temp._description = data["Description"].Value;
        else
            temp._description = "Not ready";

        if (data["Icon"] != null)
            temp._icon = data["Icon"].Value;
        else
            temp._icon = "BaseIcon";

        if (data["Class"] != null)
            temp._class = (SkillType)Enum.Parse(typeof(SkillType), data["Class"].Value);
        else
            temp._class = SkillType.Light;

        if (data["HPAm"] != null)
            temp._hp = data["HPAm"].AsInt;
        else
            temp._hp = 10;

        temp._group1 = IOM.GetHeroData(temp._name)._group1;
        temp._group1 = IOM.GetHeroData(temp._name)._group2;
        temp._group1 = IOM.GetHeroData(temp._name)._group3;

        return temp;
    }

    public void SetNameIfNull(string name)
    {
        this._name = name;
    }
}
