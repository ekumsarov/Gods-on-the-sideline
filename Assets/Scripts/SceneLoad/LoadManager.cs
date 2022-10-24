using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;

public class LoadManager : MonoBehaviour 
{

    public static LoadManager Instance = null;

    [SerializeField] private GameObject _canvas;
    [SerializeField] private Image _loadBar;

    private float _target = 0f;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }

	// Use this for initialization
	public async void LoadScene (string sceneName) {

        _loadBar.fillAmount = 0f;
        _canvas.SetActive(true);

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        do
        {
            await Task.Delay(100);
            _loadBar.fillAmount = scene.progress;
        }
        while (scene.progress < 0.9f);

        await Task.Delay(100);
        scene.allowSceneActivation = true;
    }

}
