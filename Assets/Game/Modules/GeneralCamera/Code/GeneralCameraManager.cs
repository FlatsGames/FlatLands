using System;
using System.Collections.Generic;
using DG.Tweening;
using FlatLands.Architecture;
using FlatLands.Cursors;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FlatLands.GeneralCamera
{
    public sealed class GeneralCameraManager : SharedObject
    {
        [Inject] private CursorManager _cursorManager;

        private Dictionary<string, Camera> _overlayCameras;

        public CameraHierarchy Hierarchy { get; private set; }
        public bool IsActive { get; private set; }
        public event Action<RaycastHit> OnHit;

        private Transform _cameraTarget;
        private GeneralCameraConfig _config;
        
        private Vector3 _cameraFollowVelocity;
        private float _pivotFollowVelocity;

        private float _mouseX;
        private float _mouseY;
        
        private float _smoothX;
        private float _smoothY;

        private float _smoothXVelocity;
        private float _smoothYVelocity;

        private float _lookAngle;
        private float _titleAngle;

        private float _currentFollowSpeed;
        
        private Sequence _pivotSequence;
        private Sequence _zoomSequence;

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

            _cursorManager.OnCursorStateChanged += HandleCursorStateChanged;
            UnityEventsProvider.OnFixedUpdate += OnFixedUpdate;
            SetDefaultXOffset();
            SetDefaultZoom();
            SetDefaultFollowSpeed();
            IsActive = true;
        }

        public override void Dispose()
        {
            _cursorManager.OnCursorStateChanged -= HandleCursorStateChanged;
            UnityEventsProvider.OnFixedUpdate -= OnFixedUpdate;
        }

        private void OnFixedUpdate()
        {
            UpdateCameraMovement();
            UpdateHits();
        }

        private void HandleCursorStateChanged()
        {
            IsActive = !_cursorManager.CursorActive;
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

            UpdateInput();
            UpdateCameraPosition();
            UpdateCameraRotation();
        }

        private void UpdateInput()
        {
            if(!IsActive)
                return;

            _mouseX = Input.GetAxis("Mouse X");
            _mouseY = Input.GetAxis("Mouse Y");
        }
        
        private void UpdateCameraPosition()
        {
            var newCameraPos = Vector3.SmoothDamp(Hierarchy.transform.position, _cameraTarget.transform.position,
                ref _cameraFollowVelocity, _currentFollowSpeed);
            
            Hierarchy.transform.position = newCameraPos;
        }

        private void UpdateCameraRotation()
        {
            var turnSmooth = _config.TurnSmooth;
           
           if (turnSmooth > 0)
           {
               _smoothX = Mathf.SmoothDamp(_smoothX, _mouseX, ref _smoothXVelocity, turnSmooth);
               _smoothY = Mathf.SmoothDamp(_smoothY, _mouseY, ref _smoothYVelocity, turnSmooth);
           }
           else
           {
               _smoothX = _mouseX;
               _smoothY = _mouseY;
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

            var ray = new Ray(startPos, startDirection);
            RaycastHit hit;
            Physics.SphereCast(ray, 0.2f, out hit, 10, ~_config.IgnoreHitLayers);

            OnHit?.Invoke(hit);

#if UNITY_EDITOR
            Hierarchy.SetDebugHit(10);
#endif
            
        }

#endregion


#region Changes by Conditions

        internal void SetCustomFollowSpeed(float newSpeed)
        {
            _currentFollowSpeed = newSpeed;
        }
                
        internal void SetDefaultFollowSpeed()
        {
            _currentFollowSpeed = _config.FollowSpeed;
        }

        internal void SetCustomXOffset(float xOffsetPos, float duration, Action completed = null)
        {
            SetXOffset(xOffsetPos, duration, completed);
        }
                
        internal void SetDefaultXOffset()
        {
            SetXOffset(_config.PivotXOffset, _config.PivotXOffsetDuration);
        }

        private void SetXOffset(float xOffsetPos, float duration, Action completed = null)
        {
            _pivotSequence?.Kill();
            _pivotSequence = DOTween.Sequence();
            _pivotSequence.Append(Hierarchy.XOffset.DOLocalMove(new Vector3(xOffsetPos, 0, 0), duration));
            _pivotSequence.OnComplete(() =>
            {
                _pivotSequence?.Kill();
                completed?.Invoke();
            });
        }

        internal void SetCustomZoom(float newZoom, float duration, Action completed = null)
        {
            SetZoom(newZoom, duration, completed);
        }
                        
        internal void SetDefaultZoom()
        {
            SetZoom(_config.ZoomDefault, _config.ZoomDuration);
        }

        private void SetZoom(float newZoom, float duration, Action completed = null)
        {
            _zoomSequence?.Kill();
            _zoomSequence = DOTween.Sequence();
            _zoomSequence.Append(Hierarchy.CameraComponent.transform.DOLocalMove(new Vector3(0, 0, newZoom), duration));
            _zoomSequence.OnComplete(() =>
            {
                _zoomSequence?.Kill();
                completed?.Invoke();
            });
        }

#endregion

    }
}