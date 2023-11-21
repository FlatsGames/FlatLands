using FlatLands.Architecture;
using FlatLands.CharacterEquipment;
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
			StartEquipmentProvider();
		}

		public override void Dispose()
		{
			
		}

		private void StartCharacterLife()
		{
			 var entityTarget = _controllableManager.CurrentControllableBehaviour.EntityTransform;
			 _cameraManager.SetCameraTarget(entityTarget);
			_charactersManager.CurrentCharacter.SetRotateTarget(_cameraManager.Hierarchy.CameraLook);
			_charactersManager.CurrentCharacter.Init();
		}

		private void StartEquipmentProvider()
		{
			var characterBehaviour = _charactersManager.CurrentCharacter.Behaviour;
			var characterEquipmentBehaviour = characterBehaviour.GetComponent<CharacterEquipmentBehaviour>();

			var equipmentProvider = new CharacterEquipmentProvider(characterEquipmentBehaviour, characterBehaviour.CharacterAnimator);
			equipmentProvider.Init();

		}
	}
}
