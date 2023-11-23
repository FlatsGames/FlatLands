using Sirenix.OdinInspector;

namespace FlatLands.UI
{
    public abstract class UIElement : SerializedMonoBehaviour
    {
        public virtual void Init() { }
        public virtual void Dispose() { }
        
        public virtual void HandleUpdate() {}
    }
}