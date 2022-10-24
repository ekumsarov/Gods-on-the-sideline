using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DragAndDropSystem;

namespace DragAndDropSystem
{
    public abstract class DragCell : UIItem, IDropHandler
    {
        #region Base parameters

        // Allowed DragAndDropItems
        private List<string> _allowedItems;
        [SerializeField] DragCellType _cellType = DragCellType.Simple;
        public DragCellType CellType
        {
            get { return this._cellType; }
        }

        private bool _cellLocked = false;
        public void LockCell()
        {
            _cellLocked = true;
        }

        public void UnlockCell()
        {
            _cellLocked = false;
        }

        #endregion

        public void OnDrop(PointerEventData pointer)
        {
            DragSystem.OnCellDrop(this);
        }

        public virtual bool CanPlace(string itemTag)
        {
            if (this._cellLocked == true)
                return false;

            if (this.CellType == DragCellType.DragOnly)
                return false;

            if (_allowedItems == null || _allowedItems.Count == 0)
                return true;

            return !_allowedItems.Any(tag => tag.Equals(itemTag));
        }

        public abstract void PlaceItem(string item);

        public abstract string GetItemID { get; }

        public void AddAllowedItem(string id)
        {
            if (_allowedItems.Any(item => item.Equals(id)))
                return;

            _allowedItems.Add(id);
        }

        public void RemoveAllowedItem(string id)
        {
            if (_allowedItems.Any(item => item.Equals(id)))
                return;

            _allowedItems.Remove(id);
        }

        public override void Selected(bool enter)
        {
            if (enter)
            {
                if (DragSystem.IsActive && this._frameType == FrameType.Selectable)
                    this.Frame = true;
            }
            else
            {
                if (this._frameType == FrameType.Selectable)
                    this.Frame = false;
            }
        }
    }
}

