using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.UI
{
    public sealed class UIHierarchy : SerializedMonoBehaviour
    {
        [SerializeField] 
        private Camera _uiCamera;

        [SerializeField] 
        private Canvas _canvas;

        [SerializeField] 
        private Transform _windowsLayer;
        
        [SerializeField] 
        private Transform _hudLayer;

        public Camera UICamera => _uiCamera;
    }
}