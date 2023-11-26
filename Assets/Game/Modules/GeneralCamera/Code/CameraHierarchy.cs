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
        private Transform _xOffset;

        [SerializeField]
        private Transform _cameraLook;

        [SerializeField, BoxGroup("Debug")] 
        private bool _showGizmos;

        public Camera CameraComponent => _camera;
        public Transform Pivot => _pivot;
        public Transform XOffset => _xOffset;
        public Transform CameraLook => _cameraLook;

#if UNITY_EDITOR

        private float _debugDistance;
        
        public void SetDebugHit(float distance)
        {
            _debugDistance = distance;
        }
        
        private void OnDrawGizmos()
        {
            if(!_showGizmos)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_camera.transform.position, 0.05f);
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(_cameraLook.transform.position, 0.05f);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_camera.transform.position, _cameraLook.transform.position);
            Gizmos.DrawLine(_pivot.transform.position, transform.position);
            
            Gizmos.DrawSphere(_pivot.transform.position, 0.05f);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(_camera.transform.position, _camera.transform.forward * _debugDistance);
        }
        
#endif
        
    }
}