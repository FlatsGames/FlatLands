﻿using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.UI
{
    public abstract class UIWindow : UIElement
    {
        [SerializeField, BoxGroup("Main Settings")] 
        private GameObject _root;

        [SerializeField, BoxGroup("Main Settings")]
        private Canvas _canvas;
        
        public UIWindowType WindowType { get; private set; }
        
        public bool IsWindowActive { get; private set; }
        
        public event Action<UIWindow> OnWindowShow;
        public event Action<UIWindow> OnWindowHide;

        internal virtual void SetModel(IUIModel model) { }

        internal void SetWindowType(UIWindowType type)
        {
            WindowType = type;
        }

        internal void Show()
        {
            _root.gameObject.SetActive(true);
            
            if(IsWindowActive)
                return;

            IsWindowActive = true;
            OnShow();
            OnWindowShow?.Invoke(this);
        }

        protected virtual void OnShow() { }

        internal void Hide()
        {
            _root.gameObject.SetActive(false);
            
            if(!IsWindowActive)
                return;

            IsWindowActive = false;
            OnHide();
            OnWindowHide?.Invoke(this);
        }
        
        protected virtual void OnHide() { }
    }
}