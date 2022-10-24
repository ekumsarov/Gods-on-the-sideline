using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using BattleEffects;
using System.Linq;
using BattleEngine;
using System;

public class Unit : iBattleUnit
{
    public static AIUnit Make(HeroData data)
    {
        return Hero.Make(data);
    }

    public static AIUnit Make(UnitData data)
    {
        if(data.Name.Equals("GhoulQueen"))
        {
            return GhoulQueenUnit.Make(data);
        }

        return AIUnit.Make(data);
    }

    public static AIUnit Make(FamiliarData data)
    {
        return Familiar.Make(data);
    }

    public static Unit Make(string id)
    {
        if(IOM.HasHeroData(id))
        {
            return Unit.Make(IOM.GetHeroData(id));
        }
        else if(IOM.HasEnemyData(id))
        {
            return Unit.Make(IOM.GetUnitData(id));
        }
        else if(IOM.HasFamiliarData(id))
        {
            return Unit.Make(IOM.GetFamiliarData(id));
        }
        else
        {
            Debug.LogError("No hero/enemy id:" + id);
            return null;
        }
    }

    protected int _currentHP;
    public int CurrentHP
    {
        get => _currentHP;
        set
        {
            _currentHP = value;
        }

    }



    [SerializeField] protected int _hp;
    public int HP => _hp;

    [SerializeField] protected string _name;
    public string Name => _name;

    [SerializeField] protected string _icon;
    public string Icon => _icon;

    [SerializeField] protected string _description;
    public string Description => _description;

    [SerializeField] protected List<string> _effectsImmune;
    public List<string> EffectsImmune => _effectsImmune;

    protected UnitType _unitType = UnitType.Unknown;
    public UnitType UnitType => _unitType;

    protected List<UnitFeature> _features;

    protected CardTargetSide _unitSide;
    public CardTargetSide UnitSide => _unitSide;

    protected List<SkillType> _sustainability = new List<SkillType>();
    public List<SkillType> Sustainbility => _sustainability;

    protected List<SkillType> _susceptibility = new List<SkillType>();
    public List<SkillType> Susceptibility => _susceptibility;

    protected DamageRule _damageRule;
    protected HealRule _healRule;

    public void RebuildEnemyName(string name)
    {
        this._name = name;
    }

    protected Dictionary<string, SkillObject> _heroSkills;
    protected Dictionary<string, SkillObject> _skills;
    protected Dictionary<string, SkillObject> _effectSkill;
    protected Dictionary<string, SkillObject> _lootSkill;
    public Dictionary<string, SkillObject> Skills
    {
        get
        {
            RebuildAllSkills();

            return _skills;
        }
    }

    public SkillObject Skill(string skill)
    {
        if (this._skills.ContainsKey(skill))
            return this._skills[skill];

        return SkillObject.Make(skill, 0, 0);
    }

    public SkillObject Skill(SkillType skill)
    {
        if (this._skills.ContainsKey(skill.ToString()))
            return this._skills[skill.ToString()];

        return SkillObject.Make(skill.ToString(), 0, 0);
    }

    private bool _isActive;
    public bool IsActive
    {
        get
        {
            if (EffectSystem.HasEffect(EffectType.Stunn.ToString()) || CurrentHP <= 0)
                return false;

            return _isActive;
        }
    }

    public bool IsAlive
    {
        get
        {
            if (CurrentHP <= 0)
                return false;

            return true;
        }
    }

    #region Skills

    void RebuildAllSkills()
    {
        foreach (var skill in this._skills)
        {
            if (this._heroSkills.ContainsKey(skill.Key))
                this._skills[skill.Key].SetupSkill(this._heroSkills[skill.Key]);

            foreach (var effect in this._lootSkill)
            {
                if (this._skills.ContainsKey(effect.Value.Skill))
                    this._skills[effect.Value.Skill].SetSkill(effect.Value.Min, effect.Value.Max);
            }
        }

        foreach (var effect in this._effectSkill)
        {
            if (this._skills.ContainsKey(effect.Value.Skill))
                this._skills[effect.Value.Skill].RemoveSkills(effect.Value.Min, effect.Value.Max);
        }

        foreach (var effect in this._lootSkill)
        {
            if (this._skills.ContainsKey(effect.Value.Skill))
                this._skills[effect.Value.Skill].RemoveSkills(effect.Value.Min, effect.Value.Max);
        }
    }

    public void AddSkills(string skill, int min, int max, string to, string id)
    {
        to = to.ToLower();

        if (to.Equals("hero"))
        {
            if (!this._heroSkills.ContainsKey(skill))
            {
                this._heroSkills.Add(skill, SkillObject.Make(skill, min, max));
                RebuildAllSkills();
                return;
            }

            this._heroSkills[skill].SetSkill(min, max);
            RebuildAllSkills();

            return;
        }

        if (to.Equals("loot"))
        {
            if (this._lootSkill.ContainsKey(id))
            {
                Debug.LogError("already has loot: " + id);
                return;
            }

            this._lootSkill.Add(id, SkillObject.Make(skill, min, max));
            RebuildAllSkills();
            return;
        }

        if (to.Equals("effect"))
        {
            if (this._effectSkill.ContainsKey(id))
            {
                Debug.LogError("already has loot: " + id);
                return;
            }

            this._effectSkill.Add(id, SkillObject.Make(skill, min, max));
            RebuildAllSkills();
            return;
        }
    }

    public void RemoveSkills(string from, string id)
    {
        from = from.ToLower();

        if (from.Equals("loot"))
        {
            if (this._lootSkill.ContainsKey(id))
            {
                this._lootSkill.Remove(id);
                RebuildAllSkills();
                return;
            }
        }

        if (from.Equals("effect"))
        {
            if (this._effectSkill.ContainsKey(id))
            {
                this._effectSkill.Remove(id);
                RebuildAllSkills();
                return;
            }
        }
    }

    public void RemoveSkillsFrom(string from, int min, int max)
    {
        from = from.ToLower();

        if (from.Equals("loot"))
        {
            foreach(var skill in this._lootSkill)
            {
                skill.Value.RemoveSkills(min, max);
            }

            RebuildAllSkills();
            return;
        }

        if (from.Equals("effect"))
        {
            foreach (var skill in this._effectSkill)
            {
                skill.Value.RemoveSkills(min, max);
            }

            RebuildAllSkills();
            return;
        }
    }

    public void RemoveSkills(string skill, int min, int max)
    {
        if (!this._heroSkills.ContainsKey(skill))
        {
            Debug.LogError("No skill in hero list: " + skill);
            return;
        }

        this._heroSkills[skill].RemoveSkills(min, max);
    }

    public void UpAllSkills(int min, int max, string to)
    {
        to = to.ToLower();

        if (to.Equals("hero"))
        {
            foreach(var skill in this._heroSkills)
            {
                skill.Value.SetSkill(min, max);
            }

            RebuildAllSkills();

            return;
        }

        if (to.Equals("loot"))
        {
            foreach (var skill in this._lootSkill)
            {
                skill.Value.SetSkill(min, max);
            }

            RebuildAllSkills();
            return;
        }

        if (to.Equals("effect"))
        {
            foreach (var skill in this._effectSkill)
            {
                skill.Value.SetSkill(min, max);
            }

            RebuildAllSkills();
            return;
        }
    }

    #endregion

    private bool _isDead = false;
    public void SetDead() { _isDead = true; }

    public void Damage(ActionSourceData sourceData, Action callback)
    {
        int finalAmount = this._damageRule.Damage(sourceData);

        ActionSourceData notifyData = ActionSourceData.Create()
            .SetSourceID(sourceData.SourceID)
            .SetActionAmount(finalAmount)
            .SetActionID(sourceData.ActionID)
            .SetSourceType(sourceData.SourceType)
            .SetAdditionalActionID(sourceData.AdditionalActionID)
            .SetAdditionalSourceID(sourceData.AdditionalSourceID);

        BattleLog.PushLog(this.Name + " get " + finalAmount + " damage");

        this.currentAnimationCount = 0;
        this.animationCount = 0;
        this._actionCallback = callback;

        this._view.PlayHit();
        this.animationCount += 1;

        BattleResponse.Notify(notifyData, BattleResponseActions.GetDamage);

        BattleLog.PushLog(this.Name + " has " + this._currentHP + " health");

        if (this._isDead)
        {
            this._isActive = false;
            this._view.Visible = false;
            this._currentHP = 0;
            ES.NotifySubscribers("UnitDead", null);
        }
    }

    public void Heal(int amount, string from, Action callback)
    {
        this._healRule.Heal(amount);

        this.currentAnimationCount = 0;
        this.animationCount = 1;
        this._actionCallback = callback;

        this._view.PlayHeal();
    }

    private int animationCount = 0;
    private int currentAnimationCount = 0;
    private Action _actionCallback;

    public void CompleteAnimation()
    {
        currentAnimationCount += 1;
        if(currentAnimationCount >= animationCount)
        {
            _actionCallback?.Invoke();
        }
    }

    public virtual void NextRound()
    {
        this._completeTurn = false;
        this._view.UpdateItem();
    }

    public virtual int HPRatioInAbsolute()
    {
        return Mathf.FloorToInt((this.CurrentHP / this.HP) * 100);
    }

    #region Effects

    private UnitEffect _unitEffectSystem;
    public UnitEffect EffectSystem => _unitEffectSystem;

    public void ActivateEffect()
    {
        _unitEffectSystem = new UnitEffect();
        _unitEffectSystem.Init(this, _view);
    }

    #endregion

    #region View Controll

    private BattleUnitView _view;
    public BattleUnitView View
    {
        set { this._view = value; this._unitEffectSystem.BindView(value); }
    }

    protected bool _completeTurn = false;

    public bool CanActivate
    {
        get
        {
            if (EffectSystem.HasEffect("Stunn"))
                return false;

            return !_completeTurn;
        }
    }

    public void ShowInfo()
    {

    }

    public void CloseInfo()
    {

    }

    public void PrepareForBattle()
    {
        this._isActive = true;
    }

    #endregion
}