using System.Collections.Generic;
using DG.Tweening;
using FlatLands.Architecture;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FlatLands.GeneralCamera
{
    public sealed class GeneralCameraManager : SharedObject
    {
        private Dictionary<string, Camera> _overlayCameras;
        
        public CameraHierarchy Hierarchy { get; private set; }
        public bool IsActive { get; private set; }

        private Transform _cameraTarget;
        
        internal void InvokeCameraCreated(CameraHierarchy hierarchy)
        {
            Hierarchy = hierarchy;
        }
        
        public override void Init()
        {
            _overlayCameras = new Dictionary<string, Camera>();
            RegisterOverlayCameras();
            ApplyOverlayCamerasToGeneral();

            UnityEventsProvider.OnUpdate += OnUpdate;
            IsActive = true;
        }

        public override void Dispose()
        {
            UnityEventsProvider.OnUpdate -= OnUpdate;
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

        private void OnUpdate()
        {
            if(_cameraTarget == null)
                return;
            
            if(!IsActive)
                return;

            UpdateCameraPosition();
            UpdateCameraRotation();
        }
        
        public void SetCameraTarget(Transform target)
        {
            _cameraTarget = target;
        }

        public void SetCameraActive(bool active)
        {
            IsActive = active;
        }

        private void UpdateCameraPosition()
        {
            Hierarchy.transform.DOMove(_cameraTarget.position, 0.1f);
        }

        private void UpdateCameraRotation()
        {
            
        }
    }
}