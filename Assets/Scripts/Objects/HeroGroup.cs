using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroGroup 
{
    List<Unit> _units;

    public HeroGroup()
    {
        _units = new List<Unit>();
    }

    public void AddHeroes(Dictionary<string, HeroData> data)
    {
        foreach(var unit in data)
        {
            this._units.Add(Unit.Make(unit.Value));
        }
    }

    public void AddHeroes(List<string> data)
    {
        foreach (var unit in data)
        {
            Unit gameUnit = Unit.Make(unit);
            if (gameUnit == null)
            {
                Debug.LogError("Could not found data of hero/enemy:" + unit);
                continue;
            }
                

            this._units.Add(gameUnit);
        }
    }

    public List<Unit> GetUnits()
    {
        return this._units;
    }

    public List<Unit> GetAliveUnits()
    {
        return this._units.Where(unit => unit.CurrentHP > 0).ToList();
    }

    #region Hero manage
    public virtual void AddNewHero(string hero)
    {
        Unit unit = Unit.Make(hero);

        if(unit == null)
        {
            Debug.LogError("No such hero/enemy info: " + hero);
            return;
        }

        unit.RebuildEnemyName(CheckNames(hero));

        this._units.Add(unit);

        ES.NotifySubscribers("PartyUpdate", "Player");
    }

    public void RemoveHero(string hero)
    {
        if (hero.IsNullOrEmpty())
            return;

        Unit unit = this._units.FirstOrDefault(uni => uni.Name.Equals(hero));
        if (unit != null)
            this._units.Remove(unit);

        unit = null;
        DeckSystem.RebuildDeck();
        ES.NotifySubscribers("PartyUpdate", "Player");
    }

    public void RemoveAllHeroes()
    {
        this._units.Clear();
    }

    public void RebuildHeroGroupWithList(List<string> heroes)
    {
        List<string> toRemove = new List<string>();
        for(int i = 0; i < this._units.Count; i++)
        {
            if(heroes.Any(unit => unit.Equals(this._units[i].Name)) == false)
            {
                toRemove.Add(this._units[i].Name);
            }
        }

        for(int i = 0; i < toRemove.Count; i++)
        {
            Unit hero = this._units.FirstOrDefault<Unit>(unit => unit.Name.Equals(toRemove[i]));
            if (hero != null)
                this._units.Remove(hero);
        }

        for(int i = 0; i < heroes.Count; i++)
        {
            if(this._units.Any(unit => unit.Equals(heroes[i])) == false)
            {
                this.AddNewHero(heroes[i]);
            }
        }

        LS.PartyUpdate();
    }

    #endregion

    #region Skill manage
    public void AddSkills(string hero, string skill, int min, int max, string to, string idE)
    {
        Unit unit = this._units.FirstOrDefault(uni => uni.Name.Equals(hero));
        if (unit == null)
        {
            Debug.LogError("No such hero in group: " + hero);
            return;
        }

        unit.AddSkills(skill, min, max, to, idE);
    }

    public void AddSkills(string hero, SkillObject skill, string to, string idE)
    {
        Unit unit = this._units.FirstOrDefault(uni => uni.Name.Equals(hero));
        if (unit == null)
        {
            Debug.LogError("No such hero in group: " + hero);
            return;
        }

        unit.AddSkills(skill.Skill, skill.Min, skill.Max, to, idE);
    }

    public void AddSkills(string hero, List<SkillObject> skills, string to, string idE)
    {
        Unit unit = this._units.FirstOrDefault(uni => uni.Name.Equals(hero));
        if (unit == null)
        {
            Debug.LogError("No such hero in group: " + hero);
            return;
        }

        foreach(var skill in skills)
        {
            unit.AddSkills(skill.Skill, skill.Min, skill.Max, to, idE);
        }
        
    }

    public void RemoveSkills(string hero, string skill, int min, int max, string to, string idE)
    {
        Unit unit = this._units.FirstOrDefault(uni => uni.Name.Equals(hero));
        if (unit == null)
        {
            Debug.LogError("No such hero in group: " + hero);
            return;
        }


        if (to.Equals("hero"))
            unit.RemoveSkills(skill, min, max);
        else
            unit.RemoveSkills(to, idE);
    }

    public void RemoveSkills(string hero, List<SkillObject> skills, string to, string idE)
    {
        Unit unit = this._units.FirstOrDefault(uni => uni.Name.Equals(hero));
        if (unit == null)
        {
            Debug.LogError("No such hero in group: " + hero);
            return;
        }

        if(to.Equals("Hero") || to.Equals("hero"))
        {
            foreach (var skill in skills)
            {
                unit.RemoveSkills(skill.Skill, skill.Min, skill.Max);
            }
        }
        else
        {
            foreach (var skill in skills)
            {
                unit.RemoveSkills(to, idE);
            }
        }

        
    }
    #endregion

    // TODO :
    /// Need to complete deal damage
    public void Damage(string hero, int amount)
    {
        if(hero.Equals("Group"))
        {
            for(int i = 0; i < this._units.Count; i++)
            {
                this._units[i].CurrentHP -= amount;
            }
        }
        else
        {
            for (int i = 0; i < this._units.Count; i++)
            {
                if(this._units[i].Name.Equals(hero))
                {
                    this._units[i].CurrentHP -= amount;
                    break;
                }
                    
            }
        }
    }

    public void SetCurHP(string hero, int val)
    {
        if (hero.Equals("Group"))
        {
            for (int i = 0; i < this._units.Count; i++)
            {
                this._units[i].CurrentHP = val;
            }
        }
        else
        {
            for (int i = 0; i < this._units.Count; i++)
            {
                if (this._units[i].Name.Equals(hero))
                {
                    this._units[i].CurrentHP = val;
                    break;
                }

            }
        }
    }

    public void HealHero(string hero, int val)
    {
        if (hero.Equals("Group"))
        {
            for (int i = 0; i < this._units.Count; i++)
            {
                this._units[i].CurrentHP += val;
            }
        }
        else
        {
            for (int i = 0; i < this._units.Count; i++)
            {
                if (this._units[i].Name.Equals(hero))
                {
                    this._units[i].CurrentHP += val;
                    break;
                }

            }
        }
    }

    public void RestoreCurHP(string hero)
    {
        if (hero.Equals("Group"))
        {
            for (int i = 0; i < this._units.Count; i++)
            {
                this._units[i].CurrentHP = this._units[i].HP;
            }
        }
        else
        {
            for (int i = 0; i < this._units.Count; i++)
            {
                if (this._units[i].Name.Equals(hero))
                {
                    this._units[i].CurrentHP = this._units[i].HP;
                    break;
                }

            }
        }
    }

    Dictionary<string, int> _checkinNames;
    private string CheckNames(string name)
    {
        if (_checkinNames == null)
            _checkinNames = new Dictionary<string, int>();

        string changedName = name;

        if (_checkinNames.ContainsKey(name))
        {
            _checkinNames[name] += 1;
            changedName = name + _checkinNames[name];
        }
        else
            _checkinNames.Add(name, 0);

        return changedName;
    }
}
