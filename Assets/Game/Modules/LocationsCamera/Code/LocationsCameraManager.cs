using System;
using FlatLands.Architecture;
using FlatLands.GeneralCamera;
using FlatLands.LocationsObjects;
using UnityEngine;

namespace FlatLands.LocationsCamera
{
	public sealed class LocationsCameraManager : SharedObject
	{
		[Inject] private GeneralCameraManager _cameraManager;
		
		public event Action<ILocationObject> OnLocationObjectHit;
		
		public override void Init()
		{
			_cameraManager.OnHit += HandleHit;
		}

		public override void Dispose()
		{
			_cameraManager.OnHit -= HandleHit;
		}

		private void HandleHit(RaycastHit hit)
		{
			if(hit.collider == null)
				return;

			var locationObject = hit.collider.GetComponent<ILocationObject>();
			if (locationObject == null)
				locationObject = hit.collider.GetComponentInParent<ILocationObject>();

			if(locationObject == null)
				return;
			
			OnLocationObjectHit?.Invoke(locationObject);
		}
	}
}
