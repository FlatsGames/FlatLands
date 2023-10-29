using FlatLands.Architecture;
using FlatLands.Characters;
using FlatLands.EntityControllable;
using FlatLands.GeneralCamera;

namespace FlatLands.CharactersMediator
{
	public sealed class CharactersMediatorManager : SharedObject
	{
		[Inject] private EntityControllableManager _controllableManager;
		[Inject] private CharactersManager _charactersManager;
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
			 var entityTarget = _controllableManager.CurrentControllableBehaviour.EntityTransform;
			 _cameraManager.SetCameraTarget(entityTarget);
			//_charactersManager.CurrentCharacter.SetCharacterLookTarget(_cameraManager.Hierarchy.CameraLook, _cameraManager.Hierarchy.transform);
			
			_charactersManager.CurrentCharacter.Init();
		}
	}
}
