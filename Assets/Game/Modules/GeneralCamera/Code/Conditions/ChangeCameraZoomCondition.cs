using FlatLands.Architecture;
using FlatLands.Conditions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.GeneralCamera
{
    public sealed class ChangeCameraZoomCondition : BaseCondition
    {
        [Inject] private GeneralCameraManager _generalCameraManager;
        
        [SerializeField] 
        private ChangeCameraZoomType _zoomType;
        
        [SerializeField, ShowIf("@_zoomType == ChangeCameraZoomType.CustomZoom")] 
        private float _newCameraZoom;
        
        [SerializeField, ShowIf("@_zoomType == ChangeCameraZoomType.CustomZoom")] 
        private float _newCameraZoomDuration;
        
        public override bool CanApply => true;

        public override void ApplyCondition()
        {
            if (_zoomType == ChangeCameraZoomType.DefaultZoom)
            {
                _generalCameraManager.SetDefaultZoom();
            }
            else
            {
                _generalCameraManager.SetCustomZoom(_newCameraZoom, _newCameraZoomDuration);
            }
        }
    }

    internal enum ChangeCameraZoomType
    {
        DefaultZoom = 0,
        CustomZoom = 10,
    }
}