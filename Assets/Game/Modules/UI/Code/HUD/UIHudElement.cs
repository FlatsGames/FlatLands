using UnityEngine;

namespace FlatLands.UI
{
    public abstract class UIHudElement : UIElement
    {
        [SerializeField]
        private UIHudElementType _hudElementType;

        public UIHudElementType HudElementType => _hudElementType;
    }
}