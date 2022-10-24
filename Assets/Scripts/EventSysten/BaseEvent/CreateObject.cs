using UnityEngine;
using SimpleJSON;
using Lodkod;
using System.Collections;
using System.Collections.Generic;
using System;

namespace GameEvents
{
    public class CreateObject : GameEvent
    {
        string ObjectName;

        public override void PrepareEvent(JSONNode node)
        {
            this.ID = "CreateObject";

            if (node["ID"] != null)
                ObjectName = node["ID"].Value;

            if (node["Conditions"] != null)
                MakeConditions(node["Conditions"].AsArray);
        }

        public override void Start()
        {
            if (!GM.Uniqs.ContainsKey(ObjectName))
            {
                Debug.LogError("No Such Object In Mission Folder: " + ObjectName);
                return;
            }

            SceneObject temp = GM.Uniqs[ObjectName] as SceneObject;

            if (!temp.Initialized)
                temp.HardSet();

            if(temp == null)
            {
                Debug.LogError("This object not a SceneObject: " + ObjectName);
                return;
            }
            

            temp.gameObject.SetActive(true);
            End();
        }


        #region static
        public static CreateObject Create(string _id, string ils = "Random")
        {
            CreateObject temp = new CreateObject();
            temp.ID = "CreateObject";

            temp.ObjectName = _id;

            return temp;
        }
        #endregion
    }
}