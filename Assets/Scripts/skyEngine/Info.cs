using Lodkod;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BattleEngine;
using System.IO;

public class IOM // Info Manager
{
    private static IOM instance = null;
    public Dictionary<string, BattleEffectInfo> battleEffectInfo;
    public Dictionary<string, HeroData> heroList;
    public Dictionary<string, HeroData> availableHeroList;
    public Dictionary<string, HeroData> playerPartyList;
    public Dictionary<string, UnitData> enemyList;
    public Dictionary<string, FamiliarData>familiarList;
    public Dictionary<string, LootItem> lootItemList;
    private Dictionary<string, CardData> _cards;

    private List<string> _openedMapIcons;

    public static void NewGame(JSONNode slotInfo)
    {
        if (IOM.instance != null)
        {
            IOM.instance = null;
        }

        IOM.instance = new IOM();
        IOM.instance.battleEffectInfo = new Dictionary<string, BattleEffectInfo>();
        IOM.instance.heroList = new Dictionary<string, HeroData>();
        IOM.instance.availableHeroList = new Dictionary<string, HeroData>();
        IOM.instance.playerPartyList = new Dictionary<string, HeroData>();
        IOM.instance.lootItemList = new Dictionary<string, LootItem>();
        IOM.instance._cards = new Dictionary<string, CardData>();
        IOM.instance.enemyList = new Dictionary<string, UnitData>();
        IOM.instance.familiarList = new Dictionary<string, FamiliarData>();
        IOM.instance._openedMapIcons = new List<string>();

        TextAsset pathStringBattleAction = Resources.Load("missions/config/BattleActionInfo") as TextAsset;
        TextAsset pathStringBattleEffect = Resources.Load("missions/config/BattleEffectInfo") as TextAsset;
        TextAsset pathStringBattleEffectAdd = Resources.Load("missions/" + GM.missionPath + "/config/BattleEffectAdd") as TextAsset;
        TextAsset pathStringBattleActionAdd = Resources.Load("missions/" + GM.missionPath + "/config/BattleActionAdd") as TextAsset;
        TextAsset pathStringLootItem = Resources.Load("missions/config/LootItemsInfo") as TextAsset;


        //
        // Load opened map icons
        //
        //
        JSONArray mapIconArr = slotInfo["MapIcons"].AsArray;
        if (mapIconArr != null)
        {
            for(int i = 0; i < mapIconArr.Count; i++)
            {
                IOM.instance._openedMapIcons.Add(mapIconArr[i].Value);
            }
        }
        else
            throw new System.Exception();


        /*
         * 
         * Load Cards
         * 
         */

        
        CardData[] _cardsData = Resources.LoadAll<CardData>("Cards");
        for (int i = 0; i < _cardsData.Length; i ++)
        {
            if(IOM.instance._cards.ContainsKey(_cardsData[i].ID))
            {
                Debug.LogError("Has card ID: " + _cardsData[i].ID);
                continue;
            }
            IOM.instance._cards.Add(_cardsData[i].ID, _cardsData[i]);
        }

        /*
         * Battle Effect Info
         */
        if (pathStringBattleEffect != null && !pathStringBattleEffect.text.Equals(""))
        {
            JSONNode BADoc = JSON.Parse(pathStringBattleEffect.text);
            JSONNode battleArray = BADoc["Effects"];
            BattleEffectInfo btemp;

            foreach (string key in battleArray.Keys)
            {
                btemp = BattleEffectInfo.Make(key, battleArray[key]);
                IOM.instance.battleEffectInfo.Add(key, btemp);
            }
        }
        else
            Debug.LogError("Null string by: pathStringBattleAction");

        if (pathStringBattleEffectAdd != null && !pathStringBattleEffectAdd.text.Equals(""))
        {
            JSONNode BADoc = JSON.Parse(pathStringBattleEffectAdd.text);
            JSONNode battleArray = BADoc["Effects"];
            BattleEffectInfo btemp;

            foreach (string key in battleArray.Keys)
            {
                btemp = BattleEffectInfo.Make(key, battleArray[key]);
                IOM.instance.battleEffectInfo.Add(key, btemp);
            }
        }
        else
            Debug.LogError("Null string by: pathStringBattleAction");


        /*
         * Hero list
         */

        for (int i = 0; i < LOF.instance.Heroes.Count; i++)
        {
            if (LOF.instance.Heroes[i].Name.IsNullOrEmpty())
                LOF.instance.Heroes[i].SetNameIfNull(LOF.instance.Heroes[i].name);

            IOM.instance.heroList.Add(LOF.instance.Heroes[i].Name, LOF.instance.Heroes[i]);
        }

        for(int i = 0; i < LOF.instance.Enemies.Count; i++)
        {
            if (LOF.instance.Enemies[i].Name.IsNullOrEmpty())
                LOF.instance.Enemies[i].SetNameIfNull(LOF.instance.Enemies[i].name); 

            IOM.instance.enemyList.Add(LOF.instance.Enemies[i].Name, LOF.instance.Enemies[i]);
        }

        for (int i = 0; i < LOF.instance.Familiars.Count; i++)
        {
            if (LOF.instance.Familiars[i].Name.IsNullOrEmpty())
                LOF.instance.Familiars[i].SetNameIfNull(LOF.instance.Familiars[i].name);

            IOM.instance.familiarList.Add(LOF.instance.Familiars[i].Name, LOF.instance.Familiars[i]);
        }

        HeroData htemp;
        JSONArray arr = slotInfo["HeroList"].AsArray;
        for(int i = 0; i < arr.Count; i++)
        {
            if (arr[i]["Class"] == null)
                htemp = IOM.instance.heroList[arr[i]["Name"].Value];
            else
                htemp = HeroData.Make(arr[i]);

            IOM.instance.playerPartyList.Add(arr[i]["Name"].Value, htemp);
        }

        /*JSONArray arrHeroes = slotInfo["AvailiableHeroes"].AsArray;
        for (int i = 0; i < arrHeroes.Count; i++)
        {
            if (arr[i]["Class"] == null)
                htemp = IOM.instance.heroList[arrHeroes[i]["Name"].Value];
            else
                htemp = HeroData.Make(arrHeroes[i]);

            IOM.instance.availableHeroList.Add(arrHeroes[i]["Name"].Value, htemp);
        }*/

        // костыль!!!! УДАЛИТЬ ПОСЛЕ
        IOM.instance.availableHeroList.Clear();
        foreach(var hero in IOM.instance.heroList)
        {
            IOM.instance.availableHeroList.Add(hero.Key, hero.Value);
        }

        /*
         * Loot Item Info
         */
        if (pathStringLootItem != null && !pathStringLootItem.text.Equals(""))
        {
            JSONNode LIDoc = JSON.Parse(pathStringLootItem.text);
            JSONArray lootArray = LIDoc["LootItemInfo"].AsArray;
            LootItem ltemp;

            for (int i = 0; i < lootArray.Count; i++)
            {
                ltemp = LootItem.Make(lootArray[i]["ID"].Value, lootArray[i]);
                IOM.instance.lootItemList.Add(lootArray[i]["ID"].Value, ltemp);
            }
        }
        else
            Debug.LogError("Null string by: pathStringBattleAction");

    }

    public static BattleEffectInfo GetEffect(string effectID)
    {
        try
        {
            return IOM.instance.battleEffectInfo[effectID];
        }
        catch 
        {
            throw new System.Exception("No effect id: " + effectID);
        }
    }

    public static List<HeroData> HeroList => IOM.instance.heroList.Values.ToList();

    public static List<UnitData> EnemyList => IOM.instance.enemyList.Values.ToList();

    public static List<HeroData> AvailiableHeroes => IOM.instance.availableHeroList.Values.ToList();

    public static List<string> OpenedMapIcons => IOM.instance._openedMapIcons;
    public static List<string> GetAvailiableHeroList()
    {
        List<string> temp = new List<string>();
        foreach(var hero in IOM.instance.availableHeroList)
        {
            if (GM.PlayerIcon.Group.GetUnits().Any(unit => unit.Name.Equals(hero.Key)) == false)
                temp.Add(hero.Key);
        }

        return temp;
    }

    public static HeroData GetAvaliableHeroData(string name)
    {
        if (IOM.instance.availableHeroList.ContainsKey(name) == false)
            throw new System.Exception("Not right name: " + name);

        return IOM.instance.availableHeroList[name];
    }

    public static Dictionary<string, HeroData> PlayerPartyList
    {
        get { return IOM.instance.playerPartyList; }
    }

    public static HeroData GetHeroData(string name)
    {
        if(IOM.instance.heroList.ContainsKey(name) == false)
        {
            throw new System.Exception("No Hero data id: " + name);
        }

        return IOM.instance.heroList[name];
    }

    public static UnitData GetUnitData(string name)
    {
        if (IOM.instance.enemyList.ContainsKey(name) == false)
        {
            throw new System.Exception("No Enemy data id: " + name);
        }

        return IOM.instance.enemyList[name];
    }

    public static FamiliarData GetFamiliarData(string name)
    {
        if (IOM.instance.familiarList.ContainsKey(name) == false)
        {
            throw new System.Exception("No Familiar data id: " + name);
        }

        return IOM.instance.familiarList[name];
    }

    public static bool HasHeroData(string name)
    {
        return IOM.instance.heroList.ContainsKey(name);
    }

    public static bool HasEnemyData(string name)
    {
        return IOM.instance.enemyList.ContainsKey(name);
    }

    public static bool HasFamiliarData(string name)
    {
        return IOM.instance.familiarList.ContainsKey(name);
    }

    public static LootItem GetLootItem(string name)
    {
        if(!IOM.instance.lootItemList.ContainsKey(name))
        {
            Debug.LogError("No such loot item ID: " + name);
            return null;
        }

        return IOM.instance.lootItemList[name];
    }

    public static Dictionary<string, CardData> GetAllCards()
    {
        return IOM.instance._cards;
    }

    public static CardData GetCard(string id)
    {
        if (id.IsNullOrEmpty())
            throw new System.Exception("Why card id is null");

        if (IOM.instance._cards.ContainsKey(id) == false)
            return null;

        return IOM.instance._cards[id];
    }

    public static List<CardData> GetHeroCards(string heroID, int group = 0)
    {
        if(IOM.instance.heroList.ContainsKey(heroID) == false)
        {
            throw new System.Exception("No hero ID " + heroID);
        }

        if(group == 0)
        {
            List<CardData> heroesCards = new List<CardData>();

            heroesCards.AddRange(IOM.instance.heroList[heroID].Group1);
            heroesCards.AddRange(IOM.instance.heroList[heroID].Group2);
            heroesCards.AddRange(IOM.instance.heroList[heroID].Group3);

            return heroesCards;
        }

        if (group == 1)
            return IOM.instance.heroList[heroID].Group1;

        if(group == 2)
            return IOM.instance.heroList[heroID].Group2;

        if (group == 3)
            return IOM.instance.heroList[heroID].Group3;

        return null;
    }

    public static string GetHeroIDByCard(string cardID)
    {
        string temp = string.Empty;

        foreach(var hero in IOM.instance.heroList)
        {
            if (hero.Value.Group1.Any(card => card.ID.Equals(cardID)))
                return hero.Key;

            if (hero.Value.Group2.Any(card => card.ID.Equals(cardID)))
                return hero.Key;

            if (hero.Value.Group3.Any(card => card.ID.Equals(cardID)))
                return hero.Key;
        }

        return temp;
    }
}
