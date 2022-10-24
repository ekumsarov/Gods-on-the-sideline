using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DragAndDropSystem
{
    public abstract class DragItem : UIItem, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private bool _isDraggable = true;
        [SerializeField] private bool _alwaysVisible = false;
        [SerializeField] private DragType _dragType = DragType.Simple;
        [ConditionalField("_dragType", DragType.Limit)] [SerializeField] private int _limit = 1;

        private int _currentLimit;

        public override void Setting()
        {
            base.Setting();
            _currentLimit = _limit;
        }

        public void SetupLimit(int limit)
        {
            this._limit = limit;
            this._currentLimit = _limit;
        }

        public bool EnableDrag
        {
            set { _isDraggable = value; }
        }

        public bool AlwaysVisible
        {
            set { _alwaysVisible = value; }
        }

        public delegate void SimpleDelegate();
        public SimpleDelegate OnDestroy;
        public SimpleDelegate OnEnd;

        protected abstract void CreateDragItem(PointerEventData pointer);

        public void OnBeginDrag(PointerEventData pointer)
        {
            if (!_isDraggable)
                return;

            _handleDrop = false;
            this.CreateDragItem(pointer);

            if (!_alwaysVisible)
                SetAlpha(0f);
        }

        public void OnDrag(PointerEventData pointer)
        {
            if (!_isDraggable)
                return;

            DragSystem.MoveItem(pointer.position);
        }

        public void OnEndDrag(PointerEventData pointer)
        {
            if (!_isDraggable)
                return;

            if (_handleDrop == false)
            {
                OnEnd?.Invoke();
                SetAlpha(1f);
            }
                

            DragSystem.EndDrag();
        }

        private bool _handleDrop = false;
        public void RemoveItem()
        {
            if (_dragType == DragType.Endless)
                return;

            if (_dragType == DragType.Limit && _currentLimit > 0)
            {
                _currentLimit -= 1;
                return;
            }

            _handleDrop = true;
            _currentLimit = _limit;
            
            if(_alwaysVisible == false)
                this.Visible = false;
            
            OnDestroy?.Invoke();
            //Destroy(this);
        }
    }
}


