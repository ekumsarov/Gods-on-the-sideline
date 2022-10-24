using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Lodkod;
using SimpleJSON;

namespace BattleEngine
{
    [CreateAssetMenu(fileName = "New Familiar", menuName = "Familiar")]
    public class FamiliarData : ScriptableObject, IBattleUnitData
    {
        [SerializeField] private SkillType _class;
        public SkillType Class => _class;

        [SerializeField] private UnitAIType _logicType;
        public UnitAIType LogicType => _logicType;

        [SerializeField] private CardTargetSide _unitSide;
        public CardTargetSide UnitSide => _unitSide;

        private int _currentHP;
        public int CurrentHP
        {
            get => _currentHP;
            set { _currentHP += value; }
        }

        [SerializeField] private int _hp;
        public int HP => _hp;

        [SerializeField] private int _baseDamage;
        public int BaseDamage => _baseDamage;

        [SerializeField] private int _turnsCount;
        public int TurnsCount => _turnsCount;

        [SerializeField] private string _name;
        public string Name => _name;

        [SerializeField] private string _icon;
        public string Icon => _icon;

        [SerializeField] private string _description;
        public string Description => _description;

        [SerializeField] private List<Condition> _selectUnitConditions;
        public List<Condition> SelectUnitConditions => _selectUnitConditions;

        [SerializeField] private List<string> _effectsImmune;
        public List<string> EffectsImmune => _effectsImmune;

        [SerializeField] private List<UnitAction> _actions;
        public List<UnitAction> Actions => _actions;

        [SerializeField] private List<UnitAction> _startActions;
        public List<UnitAction> StartActions => _startActions;

        [SerializeField] private List<UnitAction> _endActions;
        public List<UnitAction> EndActions => _endActions;

        private List<SkillDataFull> _skillsData;
        private Dictionary<string, SkillObject> _skills;
        public Dictionary<string, SkillObject> Skills
        {
            get
            {
                return null;
            }
        }

        public static FamiliarData Make(JSONNode data)
        {
            FamiliarData temp = new FamiliarData();

            if (data["Name"] != null)
                temp._name = data["Name"].Value;
            else
                throw new System.Exception("Not set name on hero");

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

            return temp;
        }

        public void SetNameIfNull(string name)
        {
            this._name = name;
        }
    }
}