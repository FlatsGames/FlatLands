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

        [SerializeField]
        private Transform _cameraLook;

        [SerializeField, BoxGroup("Debug")] 
        private bool _showGizmos;

        public Camera CameraComponent => _camera;
        public Transform Pivot => _pivot;
        public Transform CameraLook => _cameraLook;
        
#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if(!_showGizmos)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_camera.transform.position, 0.1f);
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(_cameraLook.transform.position, 0.1f);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_camera.transform.position, _cameraLook.transform.position);
            Gizmos.DrawLine(_pivot.transform.position, transform.position);
        }
        
#endif
        
    }
}