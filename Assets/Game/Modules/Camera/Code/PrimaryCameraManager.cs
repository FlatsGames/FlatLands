using System.Collections.Generic;
using FlatLands.Architecture;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FlatLands.GeneralCamera
{
    public sealed class PrimaryCameraManager : PrimarySharedObject
    {
        [Inject] private GeneralCameraManager _cameraManager;
        
        private Dictionary<string, Camera> _overlayCameras;

        private CameraHierarchy _hierarchy;
        private GeneralCameraConfig _config;

        public override void Init()
        {
            _config = GeneralCameraConfig.Instance;
            _overlayCameras = new Dictionary<string, Camera>();
        
            CreateGeneralCamera();
            RegisterOverlayCameras();
            ApplyOverlayCamerasToGeneral();
            _cameraManager.InvokeCameraCreated(_hierarchy);
        }
        
        private void CreateGeneralCamera()
        {
            _hierarchy = GameObject.Instantiate(_config.CameraPrefab);
        }
        
        private void RegisterOverlayCameras()
        {
            var cameras = _container.GetAll<IOverlayCameraHolder>();
            foreach (var holders in cameras)
            {
                if(holders.GetOverlayCamera == null)
                    continue;
                
                _overlayCameras[holders.Id] = holders.GetOverlayCamera;
            }
        }

        private void ApplyOverlayCamerasToGeneral()
        {
            var cameraData = _hierarchy.CameraComponent.GetUniversalAdditionalCameraData();
            foreach (var pair in _overlayCameras)
            {
                cameraData.cameraStack.Add(pair.Value);
            }
        }
    }
}