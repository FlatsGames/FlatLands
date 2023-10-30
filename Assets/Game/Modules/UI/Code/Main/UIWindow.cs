using System;
using Sirenix.OdinInspector;

namespace FlatLands.UI
{
    public abstract class UIWindow : SerializedMonoBehaviour
    {
        public UIWindowType WindowType { get; private set; }
        
        public bool IsWindowActive { get; private set; }
        
        public event Action<UIWindow> OnShow;
        public event Action<UIWindow> OnHide;

        internal virtual void Init(UIWindowType type)
        {
            WindowType = type;
        }

        internal virtual void Dispose()
        {
            
        }
        
        internal virtual void SetModel(IUIModel model)
        {
            
        }
        
        internal virtual void Show()
        {
            if(IsWindowActive)
                return;
            
            IsWindowActive = true;
            OnShow?.Invoke(this);
        }

        internal virtual void Hide()
        {
            if(!IsWindowActive)
                return;
            
            IsWindowActive = false;
            OnHide?.Invoke(this);
        }
    }
}