using System;
using System.Collections.Generic;
using FlatLands.Architecture;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FlatLands.UI
{
    public sealed class UIManager : SharedObject, IOverlayCameraHolder
    {
        private Dictionary<UIWindowType, UIWindow> _windows;
        private Dictionary<Type, UIWindow> _windowsByType;
        private List<UIWindow> _showedWindows;

        public UIHierarchy Hierarchy => _hierarchy;

        public event Action<UIWindow> OnWindowShow;
        public event Action<UIWindow> OnWindowHide;
        
        private UIHierarchy _hierarchy;
        private GeneralUIConfig _config;
        
        internal void InvokeSceneLoaded(UIHierarchy hierarchy)
        {
            _hierarchy =  hierarchy;
        }
        
        public override void Init()
        {
            _windows = new Dictionary<UIWindowType, UIWindow>();
            _windowsByType = new Dictionary<Type, UIWindow>();
            _showedWindows = new List<UIWindow>();
            
            _config = GeneralUIConfig.Instance;
            CreateWindows();
        }

        public override void Dispose()
        {
            foreach (var pair in _windows)
            {
                pair.Value.Dispose();
            }
        }

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


#region Show / Hide

        public void Show(UIWindowType type, IUIModel model = null)
        {
            var window = GetOrCreateWindow(type);
            window.SetModel(model);
            window.Show();
            _showedWindows.Add(window);
            OnWindowShow?.Invoke(window);
        }
        
        public void Hide(UIWindowType type)
        {
            var window = GetOrCreateWindow(type);
            window.Hide();
            _showedWindows.Remove(window);
            OnWindowHide?.Invoke(window);
        }

#endregion
        

#region Get

        private UIWindow GetOrCreateWindow(UIWindowType type)
        {
            if (_windows.TryGetValue(type, out var window))
                return window;
                    
            if(!_config.Windows.TryGetValue(type, out var windowSettings))
                return default;
                    
            var prefab = windowSettings.Prefab;
            window = Object.Instantiate(prefab, Hierarchy.WindowsLayer);
            window.Init(type);
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


#region IOverlayCameraHolder

        public string Id => "UICamera";

        public Camera GetOverlayCamera =>  _hierarchy.UICamera;

#endregion

    }
}

