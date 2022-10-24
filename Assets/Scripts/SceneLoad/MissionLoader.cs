using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;
using GameEvents;
using System;

public class MissionLoader : MonoBehaviour {

    [SerializeField] private string missionPath;

    void Awake()
    {
        //DragAndDropItem.PrepareDragItems();

        if (missionPath.IsNullOrEmpty())
            throw new Exception();

        TextAsset asetText = Resources.Load("missions/Slots/test") as TextAsset;
        string pathString = asetText.text;

        //TextAsset asetCrossText = Resources.Load("CrossSceneData/PlayerData") as TextAsset;
        //string pathCrossString = asetCrossText.text;
        string pathCrossString = File.ReadAllText(Application.dataPath + "/Resources/CrossSceneData/PlayerData.json");
        JSONNode crossPlayerData = JSON.Parse(pathCrossString);

        string pathSceneString = File.ReadAllText(Application.dataPath + "/Resources/missions/" + missionPath + "/MissionData.json");
        JSONNode sceneLoadData = JSON.Parse(pathSceneString);

        BattleLog.NewGame();

        LOF.NewGame();
        DragAndDropSystem.DragSystem.Initialize();

        JSONNode doc = JSON.Parse(pathString);
        GM.missionPath = missionPath;
        GM.PlatformType = (Lodkod.PlatformType)Enum.Parse(typeof(Lodkod.PlatformType), doc["Platform"].Value);
        IOM.NewGame(crossPlayerData);
        IconObject.InitializePrafabs();

        GSM.NewGame();
        ES.NewGame();
        DeckSystem.NewGame(crossPlayerData);
        TM.NewGame(doc);
        LS.NewGame(doc);
        
        GEM.NewGame();
        SaveManager.NewGame();
        UIParameters.NewGame();
        GameEvent.initAllPacks();
        GM.NewGame();
        SpriteProvider.InitAll();

        BattleEngine.BattleSystem.NewGame();
        UIM.NewGame();
        CM.NewGame();
        SM.NewGame();
        //RM.NewGame();
        //BM.NewGame();

        //BS.NewGame();
        //BattleUnitAI.BattleUnitEnemyAI.initAllPacks();
        //BattleActions.BattleAction.initAllPacks();
        BattleEffects.BattleEffect.initAllPacks();
        QS.NewGame();

        GM.InitGame(sceneLoadData);
    }
}

/*
 * 
 * GM - Global Manager (work with global functions, has units array, player hero)
 * 
 * GIM - GUI Manager. Need for work with GUI
 * 
 * IOM - Build Info Manager. Init build template. Cost of Builds and other info
 * 
 * AIM - AI Manager. Has list of AI Units.
 * 
 * SM - Storage Manager. Work with storages, for player and AI
 * 
 * TM - Timer Activity Manager. Need to create differents global events with timer
 * 
 * RM - Resource Manager. Have list of all resource object on map
 * 
 * BM - Build Manager. Have list of centers(AI and player). And have list of settlements.
 * 
 * IM - Island Manager. Init all islands and have a list of island to access them
 * 
 * EM - Event Manager. Need to add events in queue.
 * 
 * MM - Map Manager. Init all info and work with mesh map. Need to read to know about all objects on it and can show where to place it
 */
