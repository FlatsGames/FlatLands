using System.Collections.Generic;
using FlatLands.Architecture;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FlatLands.GeneralCamera
{
    public sealed class GeneralCameraManager : SharedObject
    {
        private Dictionary<string, Camera> _overlayCameras;
        
        public CameraHierarchy Hierarchy { get; private set; }
        
        internal void InvokeCameraCreated(CameraHierarchy hierarchy)
        {
            Hierarchy = hierarchy;
        }
        
        public override void Init()
        {
            _overlayCameras = new Dictionary<string, Camera>();
            RegisterOverlayCameras();
            ApplyOverlayCamerasToGeneral();
        }

        public override void Dispose()
        {
            
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
            var cameraData = Hierarchy.CameraComponent.GetUniversalAdditionalCameraData();
            foreach (var pair in _overlayCameras)
            {
                cameraData.cameraStack.Add(pair.Value);
            }
        }
    }
}