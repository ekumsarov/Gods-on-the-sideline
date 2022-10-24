using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleEngine;

public class ListOfHeroes : MonoBehaviour
{
    public List<HeroData> Heroes;
    public List<UnitData> Enemies;
    public List<FamiliarData> Familiars;
}

public class LOF
{
    public static ListOfHeroes instance = null;
    public static void NewGame()
    {
        LOF.instance = GameObject.Find("Managers").GetComponentInChildren<ListOfHeroes>();
    }
}
