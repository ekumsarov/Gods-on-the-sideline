using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;

public interface IHeroData 
{

    SkillType Class { get; }

    List<string> EquippedLoot { get; }
}
