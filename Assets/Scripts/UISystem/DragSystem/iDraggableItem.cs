using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DragAndDropSystem
{
    public interface iDraggableItem
    {
        public string DataID { get; }
        public string DragItemTag { get; }
        public Vector3 SetPosition { set; }

        public bool DestroyItem { set; }
    }
}