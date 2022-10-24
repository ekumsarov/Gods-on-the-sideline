using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class ManagerForFirstGameStart : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Image _loadBar;

    private bool checkForStart = false;
    private bool _completeFileWrite = false;

    void Awake()
    {
        if (File.Exists(Application.dataPath + "/Resources/CrossSceneData/PlayerData.json") == false)
        {
            Directory.CreateDirectory(Application.dataPath + "/Resources/CrossSceneData/");
            checkForStart = true;
            _completeFileWrite = false;
        }

        LoadScene();
    }

    public async void LoadScene()
    {

        _loadBar.fillAmount = 0f;
        _canvas.SetActive(true);

        if(checkForStart)
        {
            TextAsset asetCrossText = Resources.Load("ForStartData/PlayerData") as TextAsset;
            FileSystemWatcher temp = new FileSystemWatcher(Application.dataPath + "/Resources/CrossSceneData/");
            temp.Created += CompleteSave;
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

            File.WriteAllText(Application.dataPath + "/Resources/CrossSceneData/PlayerData.json", asetCrossText.text);

            do
            {
                await Task.Delay(100);
                _loadBar.fillAmount = 0.25f;
            } while (_completeFileWrite == false);

            // 
            // Load Campsite Data
            //
            temp = null;
            _completeFileWrite = false;
            asetCrossText = Resources.Load("missions/Campsite/OriginalMissionData/MissionData") as TextAsset;
            temp = new FileSystemWatcher(Application.dataPath + "/Resources/missions/Campsite/");
            temp.Created += CompleteSave;
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

            File.WriteAllText(Application.dataPath + "/Resources/missions/Campsite/MissionData.json", asetCrossText.text);

            do
            {
                await Task.Delay(100);
                _loadBar.fillAmount = 0.5f;
            } while (_completeFileWrite == false);
        }

        await Task.Delay(100);

        var scene = SceneManager.LoadSceneAsync("MainMenu");
        scene.allowSceneActivation = false;

        await Task.Delay(100);

        do
        {
            await Task.Delay(100);
            if (checkForStart)
                _loadBar.fillAmount = 0.5f + (scene.progress / 2);
            else
                _loadBar.fillAmount = scene.progress;
        }
        while (scene.progress < 0.9f);

        await Task.Delay(100);
        scene.allowSceneActivation = true;
    }

    public void CompleteSave(object sender, FileSystemEventArgs e)
    {
        _completeFileWrite = true;
    }
}
