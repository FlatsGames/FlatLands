using FlatLands.Architecture;
using FlatLands.EntityControllable;
using FlatLands.GeneralCamera;
using UnityEngine;

namespace FlatLands.CharactersMediator
{
	public sealed class CharactersMediatorManager : SharedObject
	{
		[Inject] private EntityControllableManager _controllableManager;
		[Inject] private GeneralCameraManager _cameraManager;

		public override void Init()
		{
			StartCharacterLife();
		}

		public override void Dispose()
		{
			
		}

		private void StartCharacterLife()
		{
			var entityTarget = _controllableManager.CurrentControllableEntity.EntityTransform;
			_cameraManager.SetCameraTarget(entityTarget);
		}
	}
}
