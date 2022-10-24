using UnityEngine;
using SimpleJSON;
using Lodkod;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

namespace GameEvents
{
    public class SceneLoadEvent : GameEvent
    {
        JSONArray _objects;
        string _cameraPosition;
        public override void PrepareEvent(JSONNode node)
        {
            this.ID = "SceneLoadEvent";

            if (node["ActiveObjects"] != null)
                _objects = node["ActiveObjects"].AsArray;

            if (node["CameraPosition"] != null)
                _cameraPosition = node["CameraPosition"].Value;

            if (node["Conditions"] != null)
                MakeConditions(node["Conditions"].AsArray);
        }

        public override void Start()
        {
            for(int i = 0; i < _objects.Count; i++)
            {
                SkyObject temp = GetObject(_objects[i]["ID"].Value);
                SceneObject tempScene = temp as SceneObject;
                if(tempScene != null)
                {
                    tempScene.MainEvent = _objects[i]["MainEvent"].Value;
                    tempScene.gameObject.SetActive(_objects[i]["Active"].AsBool);

                    JSONArray eventArray = _objects[i]["Events"].AsArray;
                    for (int j = 0; j < eventArray.Count; j++)
                    {
                        tempScene.Activity.AddEvent(eventArray[j].Value);
                    }
                }
                else
                {
                    temp.MainEvent = _objects[i]["MainEvent"].Value;

                    JSONArray eventArray = _objects[i]["Events"].AsArray;
                    for (int j = 0; j < eventArray.Count; j++)
                    {
                        temp.Activity.AddEvent(eventArray[j].Value);
                    }
                }
            }

            SceneObject tempCamera = GetObject(_cameraPosition) as SceneObject;
            if (tempCamera == null)
            {
                Debug.LogError("Not found object in camera move");
                End();
                return;
            }

            GM.Camera.MoveToPoint(tempCamera.CameraPoint);
            End();
        }


        #region static
        public static SceneLoadEvent Create(string SceneID)
        {
            SceneLoadEvent temp = new SceneLoadEvent();
            temp.ID = "SceneLoadEvent";
//            temp._sceneID = SceneID;

            return temp;
        }
        #endregion
    }
}