using UnityEngine;
using SimpleJSON;
using Lodkod;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

namespace GameEvents
{
    public class LoadScene : GameEvent
    {
        string _sceneID;
        public override void PrepareEvent(JSONNode node)
        {
            this.ID = "LoadScene";

            if (node["SceneID"] != null)
                _sceneID = node["SceneID"].Value;

            if (node["Conditions"] != null)
                MakeConditions(node["Conditions"].AsArray);
        }

        public override void Start()
        {
            if(_sceneID.Equals(GM.missionPath))
            {
                UIM.CloseMenu("MapMenu");   
                End();
                return;
            }

            CrossSceneLoadManager.Instance.LoadScene(_sceneID);

            End();
        }


        #region static
        public static LoadScene Create(string sceneID)
        {
            LoadScene temp = new LoadScene();
            temp.ID = "LoadScene";
            temp._sceneID = sceneID;

            return temp;
        }
        #endregion
    }
}