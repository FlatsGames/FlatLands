using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.GeneralCamera
{
    public sealed class CameraHierarchy : SerializedMonoBehaviour
    {
        [SerializeField] 
        private Camera _camera;

        [SerializeField] 
        private Transform _pivot;

        public Camera CameraComponent => _camera;
        public Transform Pivot => _pivot;

        public void SetCameraActive(bool isActive)
        {
            _camera.gameObject.SetActive(isActive);
        }
    }
}