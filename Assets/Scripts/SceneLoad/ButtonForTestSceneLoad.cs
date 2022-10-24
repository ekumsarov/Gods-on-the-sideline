using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonForTestSceneLoad : MonoBehaviour
{
    public void LoadCampsite()
    {
        LoadManager.Instance.LoadScene("Campsite");
    }
}
