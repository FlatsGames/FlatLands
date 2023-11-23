using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.UI
{
    public class UILayerGroup : SerializedMonoBehaviour
    {
        [SerializeField] private UILayerGroupType _layerGroupType;
        [SerializeField] private List<UIElement> _layerGroupElements;

        public UILayerGroupType LayerGroupType => _layerGroupType;
        public IReadOnlyList<UIElement> LayerGroupElements => _layerGroupElements;

        [Button]
        internal void GetLayerElements()
        {
            _layerGroupElements = gameObject.GetComponentsInChildren<UIElement>().ToList();
        }
    }
}