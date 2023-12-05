using FlatLands.Architecture;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.GeneralCamera
{
    [CreateAssetMenu(
        menuName = "FlatLands/Cameras/" + nameof(GeneralCameraConfig), 
        fileName = nameof(GeneralCameraConfig))]
    public sealed class GeneralCameraConfig : SingletonScriptableObject<GeneralCameraConfig>
    {
        [SerializeField] 
        private CameraHierarchy _cameraPrefab;
        
        [SerializeField, BoxGroup("Layers")]
        private LayerMask _ignoreHitLayers;
        
        [SerializeField, BoxGroup("Pivot")] 
        private float _pivotXOffset;

        [SerializeField, BoxGroup("Pivot")] 
        private float _pivotXOffsetDuration;
        
        [SerializeField, BoxGroup("Zoom")] 
        private float _zoomDefault;
        
        [SerializeField, BoxGroup("Zoom")] 
        private float _zoomDuration;

        [SerializeField, BoxGroup("Settings")] 
        private float _followSpeed = 0.1f;

        [SerializeField, BoxGroup("Settings")] 
        private float _turnSmooth;
        
        [SerializeField, BoxGroup("Settings")] 
        private float _horizontalRotationSpeed;
        
        [SerializeField, BoxGroup("Settings")] 
        private float _verticalRotationSpeed;
        
        [SerializeField, BoxGroup("Settings"), MinMaxSlider(-70, 90)] 
        private Vector2 _angelLimit;
        
        public CameraHierarchy CameraPrefab => _cameraPrefab;
        public LayerMask IgnoreHitLayers => _ignoreHitLayers;
        
        public float PivotXOffset => _pivotXOffset;
        public float PivotXOffsetDuration => _pivotXOffsetDuration;
        public float ZoomDefault => _zoomDefault;
        public float ZoomDuration => _zoomDuration;
        
        public float FollowSpeed => _followSpeed;
        public float TurnSmooth => _turnSmooth;
        
        public float HorizontalRotationSpeed => _horizontalRotationSpeed;
        public float VerticalRotationSpeed => _verticalRotationSpeed;

        public Vector2 AngleLimit => _angelLimit;
    }
}