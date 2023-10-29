using System;
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
        public bool IsActive { get; private set; }

        public event Action<RaycastHit> OnHit;

        private Transform _cameraTarget;
        private GeneralCameraConfig _config;
        
        private Vector3 _cameraFollowVelocity;
        private float _pivotFollowVelocity;
        
        private float _smoothX;
        private float _smoothY;

        private float _smoothXVelocity;
        private float _smoothYVelocity;

        private float _lookAngle;
        private float _titleAngle;

        internal void InvokeCameraCreated(CameraHierarchy hierarchy)
        {
            Hierarchy = hierarchy;
        }
        
        public override void Init()
        {
            _overlayCameras = new Dictionary<string, Camera>();
            _config = GeneralCameraConfig.Instance;
            
            RegisterOverlayCameras();
            ApplyOverlayCamerasToGeneral();

            UnityEventsProvider.OnFixedUpdate += OnFixedUpdate;
            UnityEventsProvider.OnUpdate += OnUpdate;
            IsActive = true;
        }

        public override void Dispose()
        {
            UnityEventsProvider.OnFixedUpdate -= OnFixedUpdate;
            UnityEventsProvider.OnUpdate -= OnUpdate;
        }
        
        private void OnUpdate()
        {
            UpdateHits();
        }
        
        private void OnFixedUpdate()
        {
            UpdateCameraMovement();
        }

#region Main

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
        
        public void SetCameraTarget(Transform target)
        {
            _cameraTarget = target;
        }

        public void SetCameraActive(bool active)
        {
            IsActive = active;
        }

#endregion


#region Movements
        
        private void UpdateCameraMovement()
        {
            if(_cameraTarget == null)
                return;
            
            if(!IsActive)
                return;

            UpdateCameraPosition();
            UpdateCameraRotation();
            UpdatePivot();
        }

        private void UpdatePivot()
        {
            var pivotSide = _config.DefaultPivotType == CameraPivotType.Right
                ? _config.PivotOffset
                : -_config.PivotOffset;

            var localPosition = Hierarchy.Pivot.localPosition;
            var newSmoothPos = Mathf.SmoothDamp(localPosition.x, pivotSide, ref _pivotFollowVelocity, _config.PivotOffsetSpeed);
            
            localPosition = new Vector3(newSmoothPos, localPosition.y, localPosition.z);
            Hierarchy.Pivot.localPosition = localPosition;
        }

        private void UpdateCameraPosition()
        {
            var newCameraPos = Vector3.SmoothDamp(Hierarchy.transform.position, _cameraTarget.transform.position,
                ref _cameraFollowVelocity, _config.FollowSpeed);
            
            Hierarchy.transform.position = newCameraPos;
        }

        private void UpdateCameraRotation()
        {
           var mouseX = Input.GetAxis("Mouse X");
           var mouseY = Input.GetAxis("Mouse Y");
           
           var turnSmooth = _config.TurnSmooth;
           
           if (turnSmooth > 0)
           {
               _smoothX = Mathf.SmoothDamp(_smoothX, mouseX, ref _smoothXVelocity, turnSmooth);
               _smoothY = Mathf.SmoothDamp(_smoothY, mouseY, ref _smoothYVelocity, turnSmooth);
           }
           else
           {
               _smoothX = mouseX;
               _smoothY = mouseY;
           }

           _lookAngle += _smoothX * _config.HorizontalRotationSpeed;
           Hierarchy.transform.rotation = Quaternion.Euler(0, _lookAngle, 0);

           _titleAngle -= _smoothY *  _config.VerticalRotationSpeed;
           _titleAngle = Mathf.Clamp(_titleAngle,  _config.AngleLimit.x,  _config.AngleLimit.y);
           Hierarchy.Pivot.localRotation = Quaternion.Euler(_titleAngle, 0, 0);
        }
        
#endregion


#region Hits

        private void UpdateHits()
        {
            var cameraTransform = Hierarchy.CameraComponent.transform;
            var startPos = cameraTransform.position;
            var startDirection = cameraTransform.forward;

            RaycastHit hit;
            Physics.Raycast(startPos, startDirection, out hit, 100, ~_config.IgnoreHitLayers);
            OnHit?.Invoke(hit);
        }

#endregion

    }
}