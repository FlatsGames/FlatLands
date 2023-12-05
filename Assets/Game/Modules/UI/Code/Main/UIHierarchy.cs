using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.UI
{
    public sealed class UIHierarchy : SerializedMonoBehaviour
    {
        [SerializeField] 
        private Camera _uiCamera;

        [SerializeField] 
        private Canvas _uiCanvas;

        [SerializeField] 
        private Dictionary<UILayerGroupType, UILayerGroup> _layerGroups;

        public Camera UICamera => _uiCamera;
        public Canvas UICanvas => _uiCanvas;
        public IReadOnlyDictionary<UILayerGroupType, UILayerGroup> LayerGroups => _layerGroups;

        [Button]
        private void GetGroupElements()
        {
            _layerGroups = new Dictionary<UILayerGroupType, UILayerGroup>();
            var layerGroups = gameObject.GetComponentsInChildren<UILayerGroup>();
            foreach (var group in layerGroups)
            {
                group.GetLayerElements();
                _layerGroups[group.LayerGroupType] = group;
            }
        }
    }
}