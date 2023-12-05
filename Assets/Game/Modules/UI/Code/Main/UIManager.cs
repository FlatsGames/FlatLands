using System;
using System.Collections.Generic;
using FlatLands.Architecture;
using FlatLands.Cursors;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FlatLands.UI
{
    public sealed class UIManager : SharedObject, IOverlayCameraHolder
    {
        [Inject] private CursorManager _cursorManager;
        
        public UIHierarchy Hierarchy => _hierarchy;

        private List<UIElement> _uiElements;

        private UIHierarchy _hierarchy;
        private GeneralUIConfig _config;

        public UIManager()
        {
            _uiElements = new List<UIElement>();
            _windows = new Dictionary<UIWindowType, UIWindow>();
            _windowsByType = new Dictionary<Type, UIWindow>();
            _showedWindows = new List<UIWindow>();
            _hudElements = new Dictionary<UIHudElementType, UIHudElement>();
        }
        
        internal void InvokeSceneLoaded(UIHierarchy hierarchy)
        {
            _hierarchy =  hierarchy;
        }
        
        public override void Init()
        {
            _config = GeneralUIConfig.Instance;

            InitHudElements();
            CreateWindows();
            _cursorManager.HideCursor();

            UnityEventsProvider.OnUpdate += HandleUpdate;
        }

        public override void Dispose()
        {
            UnityEventsProvider.OnUpdate -= HandleUpdate;
            foreach (var uiElement in _uiElements)
            {
                uiElement.Dispose();
            }
        }
        
        private void InitUIElement(UIElement element)
        {
            _container.InjectAt(element);
            element.Init();
            _uiElements.Add(element);
        }

        private void HandleUpdate()
        {
            foreach (var uiElement in _uiElements)
            {
                uiElement.HandleUpdate();
            }
        }

#region Windows

        private Dictionary<UIWindowType, UIWindow> _windows;
        private Dictionary<Type, UIWindow> _windowsByType;
        private List<UIWindow> _showedWindows;
        public event Action<UIWindow> OnWindowShowed;
        public event Action<UIWindow> OnWindowHided;

        private void CreateWindows()
        {
            foreach (var pair in _config.Windows)
            {
                if(!pair.Value.PreloadPrefab)
                    continue;

                var window = GetOrCreateWindow(pair.Key);
                window.Hide();
            }
        }

        public void Show(UIWindowType type, IUIModel model = null)
        {
            var window = GetOrCreateWindow(type);
            window.SetModel(model);
            window.Show();
            _showedWindows.Add(window);
            OnWindowShowed?.Invoke(window);
            _cursorManager.ShowCursor();
        }
        
        public void Hide(UIWindowType type)
        {
            var window = GetOrCreateWindow(type);
            window.Hide();
            _showedWindows.Remove(window);
            OnWindowHided?.Invoke(window);
            _cursorManager.HideCursor();
        }

        private UIWindow GetOrCreateWindow(UIWindowType type)
        {
            if (_windows.TryGetValue(type, out var window))
                return window;
                    
            if(!_config.Windows.TryGetValue(type, out var windowSettings))
                return default;
                    
            var prefab = windowSettings.Prefab;
            if (!Hierarchy.LayerGroups.TryGetValue(UILayerGroupType.Windows, out var group))
                return default;
            
            window = Object.Instantiate(prefab, group.transform);
            window.SetWindowType(type);
            InitUIElement(window);
            _windows[type] = window;
            _windowsByType[windowSettings.PrefabType] = window;
            return window;
        }

        public T GetWindow<T>() where T : UIWindow
        {
            var type = typeof(T);
            if (!_windowsByType.TryGetValue(type, out var window))
                return default;

            return (T)window;
        }
        
        public UIWindow GetWindow(UIWindowType type)
        {
            if (!_windows.TryGetValue(type, out var window))
                return default;

            return window;
        }
        
#endregion


#region HUD

        private Dictionary<UIHudElementType, UIHudElement> _hudElements;

        private void InitHudElements()
        {
            if (!Hierarchy.LayerGroups.TryGetValue(UILayerGroupType.Hud, out var hudLayer))
                return;

            foreach (var uiElement in hudLayer.LayerGroupElements)
            {
                var hudElement = uiElement as UIHudElement;
                if(hudElement == null)
                    continue;

                _hudElements[hudElement.HudElementType] = hudElement;
                
                InitUIElement(uiElement);
            }
        }

        public T GetHudElement<T>(UIHudElementType type) where T : UIHudElement
        {
            if (!_hudElements.TryGetValue(type, out var hudElement))
                return default;

            return (T)hudElement;
        }
        
        public UIHudElement GetHudElement(UIHudElementType type)
        {
            if (!_hudElements.TryGetValue(type, out var hudElement))
                return default;

            return hudElement;
        }

#endregion


#region IOverlayCameraHolder

        public string Id => "UICamera";

        public Camera GetOverlayCamera =>  _hierarchy.UICamera;

#endregion

    }
}

