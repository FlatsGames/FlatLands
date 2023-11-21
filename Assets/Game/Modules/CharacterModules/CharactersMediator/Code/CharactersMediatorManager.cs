using FlatLands.Architecture;
using FlatLands.CharacterCombat;
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

		private CharacterEquipmentProvider _equipmentProvider;
		private CharacterCombatProvider _combatProvider;
		
		public override void Init()
		{
			StartCharacterLife();
			StartEquipmentProvider();
			StartCombatProvider();
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

			_equipmentProvider = new CharacterEquipmentProvider(characterEquipmentBehaviour, characterBehaviour.CharacterAnimator);
			_equipmentProvider.Init();
		}

		private void StartCombatProvider()
		{
			var characterBehaviour = _charactersManager.CurrentCharacter.Behaviour;
			var characterCombatBehaviour = characterBehaviour.GetComponent<CharacterCombatBehaviour>();

			_combatProvider = new CharacterCombatProvider(_equipmentProvider, characterCombatBehaviour, characterBehaviour.CharacterAnimator);
			_combatProvider.Init();
		}
	}
}
