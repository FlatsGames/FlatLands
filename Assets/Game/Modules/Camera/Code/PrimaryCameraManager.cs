using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.GeneralCamera
{
    public sealed class PrimaryCameraManager : PrimarySharedObject
    {
        [Inject] private GeneralCameraManager _cameraManager;

        private CameraHierarchy _hierarchy;
        private GeneralCameraConfig _config;

        public override void Init()
        {
            _config = GeneralCameraConfig.Instance;
            
            CreateGeneralCamera();
            _cameraManager.InvokeCameraCreated(_hierarchy);
        }
        
        private void CreateGeneralCamera()
        {
            _hierarchy = GameObject.Instantiate(_config.CameraPrefab);
        }
    }
}