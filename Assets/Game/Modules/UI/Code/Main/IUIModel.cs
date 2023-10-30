using System;

namespace FlatLands.UI
{
    public interface IUIModel
    {
        public event Action OnChanged;
    }
}