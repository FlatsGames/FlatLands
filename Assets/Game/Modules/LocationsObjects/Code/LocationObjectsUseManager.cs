using System;
using System.Collections.Generic;
using FlatLands.Architecture;
using FlatLands.GeneralCamera;
using UnityEngine;

namespace FlatLands.LocationsObjects
{
    public class LocationObjectsUseManager : SharedObject
    {
        [Inject] private GeneralCameraManager _cameraManager;

        private LocationObjectsUseConfig _config;
        private Dictionary<Type, IObjectUseHandler> _useHandlers;
        private ILocationObject _targetObject;
        
        public event Action<ILocationObject> OnLocationObjectHit;
        
        public override void Init()
        {
            _config = LocationObjectsUseConfig.Instance;
            _useHandlers = new Dictionary<Type, IObjectUseHandler>();
            var handlers = _container.GetAll<IObjectUseHandler>();
            foreach (var handler in handlers)
            {
                _useHandlers.Add(handler.UseType, handler);
            }
            UnityEventsProvider.OnUpdate += HandleUpdate;
            _cameraManager.OnHit += HandleHit;
        }

        public override void Dispose()
        {
            UnityEventsProvider.OnUpdate -= HandleUpdate;
            _cameraManager.OnHit -= HandleHit;
        }

        private void HandleUpdate()
        {
            foreach (var useConfig in _config.UseVariants)
            {
                if (!Input.GetKeyDown(useConfig.UseKey))
                    continue;
                
                if (!_useHandlers.TryGetValue(useConfig.UseType, out var handler))
                    continue;
                
                if (_targetObject == null)
                    continue;
                
                handler.ObjetUse(_targetObject);
            }
        }
        
        private void HandleHit(RaycastHit hit)
        {
            if(hit.collider == null)
                return;

            _targetObject = hit.collider.GetComponent<ILocationObject>() 
                            ?? hit.collider.GetComponentInParent<ILocationObject>();

            OnLocationObjectHit?.Invoke(_targetObject);
        }
    }
}
