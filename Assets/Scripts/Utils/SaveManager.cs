using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using SimpleJSON;
using Lodkod;
using System.Linq;
using System.Threading.Tasks;

public class SaveManager
{
    private static SaveManager instance = null;
    public static void NewGame()
    {
        if (SaveManager.instance != null)
            SaveManager.instance = null;

        SaveManager.instance = new SaveManager();
        SaveManager.instance._saveDate = new JSONObject();
    }

    JSONNode _saveDate;

    public static void AddSaveObject(object str, string key)
    {
        bool HasKey = false;

        foreach(var _key in SaveManager.instance._saveDate.Keys)
        {
            if(_key == key)
            {
                HasKey = true;
                break;
            }
        }

        if(!HasKey)
        {
            JSONArray ar = new JSONArray();
            SaveManager.instance._saveDate.Add(key, ar);
        }

        var node = new JSONObject();

        SaveManager.instance._saveDate[key].Add(JSON.Parse(JsonUtility.ToJson(str, true)));


    }

    public static void SaveFile()
    {
        File.WriteAllText(Application.dataPath + "/Resources/missions/Slots/startInfo.json", SaveManager.instance._saveDate.ToString());
    }

    private bool _completeSaveingFile = false;
    public static bool CompleteSaving => SaveManager.instance._completeSaveingFile;

    public static void SaveCrossSceneData()
    {
        JSONNode node = new JSONObject();
        JSONArray heroListArray = new JSONArray();
        SaveManager.instance._completeSaveingFile = false;

        foreach (var hero in GM.PlayerIcon.Group.GetUnits().Cast<Hero>())
        {
            JSONNode heroNode = new JSONObject();
            heroNode.Add("Name", hero.Name);
            heroListArray.Add(heroNode);
        }

        node.Add("HeroList", heroListArray);

        JSONArray deckListArray = new JSONArray();

        foreach (var card in DeckSystem.DeckList)
        {
            JSONNode heroNode = new JSONObject();
            heroNode.Add("Card", card);
            heroNode.Add("Amount", 1);
            deckListArray.Add(heroNode);
        }

        node.Add("deck", deckListArray);

        JSONArray mapIconArray = new JSONArray();

        foreach(var iconID in IOM.OpenedMapIcons)
        {
            mapIconArray.Add(iconID);
        }
        node.Add("MapIcons", mapIconArray);

        FileSystemWatcher temp = new FileSystemWatcher(Application.dataPath + "/Resources/CrossSceneData/");
        temp.Changed += CompleteSave;

        temp.NotifyFilter = NotifyFilters.Attributes
                             | NotifyFilters.CreationTime
                             | NotifyFilters.DirectoryName
                             | NotifyFilters.FileName
                             | NotifyFilters.LastAccess
                             | NotifyFilters.LastWrite
                             | NotifyFilters.Security
                             | NotifyFilters.Size;

        temp.Filter = "*.json";
        temp.IncludeSubdirectories = true;
        temp.EnableRaisingEvents = true;

        File.WriteAllText(Application.dataPath + "/Resources/CrossSceneData/PlayerData.json", node.ToString());
    }

    public static void CompleteSave(object sender, FileSystemEventArgs e)
    {
        SaveManager.instance._completeSaveingFile = true;
    }
}
