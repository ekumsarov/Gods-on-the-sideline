using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragAndDropSystem
{
    public enum DragType
    {
        Limit,
        Endless,
        Simple
    }

    public enum DragCellType
    {
        Simple,
        DropOnly,
        DragOnly,
        Swap
    }

    public class DragSystem
    {
        private static DragSystem _instance;

        private MenuEx _activeMenu;
        public static void Initialize()
        {
            if (DragSystem._instance != null)
                DragSystem._instance = null;

            DragSystem._instance = new DragSystem();
            DragSystem._instance._descriptor = new Descriptor();
        }

        private iDraggableItem _item;
        private DragItem _draggedItem;

        private Descriptor _descriptor;

        private bool _isActive = false;
        public static bool IsActive
        {
            get { return DragSystem._instance._isActive; }
        }

        public static void ActivateItem(iDraggableItem item, DragItem dragged, DragCell cell)
        {
            DragSystem._instance._item = item;
            DragSystem._instance._draggedItem = dragged;
            DragSystem._instance._isActive = true;
            DragSystem._instance._descriptor.ItemTag = item.DragItemTag;
            DragSystem._instance._descriptor.ItemID = item.DataID;
            DragSystem._instance._descriptor.parentCell = cell;
        }

        public static void ActivateMenu(MenuEx menu)
        {
            DragSystem._instance._activeMenu = menu;
        }

        public static void MoveItem(Vector3 position)
        {
            DragSystem._instance._item.SetPosition = position;
        }

        public static void StartDrag()
        {
            if (DragSystem._instance._activeMenu == null)
                return;

            DragSystem._instance._activeMenu.MakeRaycast(false);
        }

        public static void EndDrag()
        {
            if (DragSystem._instance._activeMenu == null || DragSystem._instance._item == null)
                return;

            DragSystem._instance._activeMenu.MakeRaycast(true);
            DragSystem._instance._activeMenu.NotifyDragComplete();
            DragSystem._instance._isActive = false;
            DragSystem._instance._item.DestroyItem = false;
            DragSystem._instance._item = null;
        }


        public static void OnCellDrop(DragCell cell)
        {
            EndDrag();

            if (cell.CanPlace(DragSystem._instance._descriptor.ItemTag))
            {
                DragSystem._instance._descriptor.targetCell = cell;
                if (cell.CellType == DragCellType.Swap)
                    DragSystem._instance._descriptor.targetItemID = cell.GetItemID;

                cell.PlaceItem(DragSystem._instance._descriptor.ItemID);
                DragSystem._instance._activeMenu.ItemsChanged(DragSystem._instance._descriptor);

                DragSystem._instance._draggedItem.RemoveItem();
            }
        }
    }

    public class Descriptor
    {
        public string ItemTag;
        public string ItemID;
        public DragCell parentCell;
        public DragCell targetCell;
        public string targetItemID;
    }
}

