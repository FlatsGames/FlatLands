using FlatLands.Architecture;
using FlatLands.CharacterAttributes;
using FlatLands.CharacterCombat;
using FlatLands.CharacterEquipment;
using FlatLands.CharacterLocomotion;
using FlatLands.Characters;
using FlatLands.Cursors;
using FlatLands.EntityControllable;
using FlatLands.GeneralCamera;

namespace FlatLands.CharactersMediator
{
	public sealed class CharactersMediatorManager : SharedObject
	{
		[Inject] private EntityControllableManager _controllableManager;
		[Inject] private CharactersManager _charactersManager;
		[Inject] private GeneralCameraManager _cameraManager;
		[Inject] private CursorManager _cursorManager;

		private CharacterGroup _characterGroup;
		private CharacterBehaviour _characterBehaviour;
		
		public override void Init()
		{
			StartCharacterLife();
			StartAttributeProvider();
			LocomotionProvider();
			EquipmentProvider();
			CombatProvider();
			
			_controllableManager.SetControllableEntity(_characterGroup, _characterBehaviour);
			var entityTarget = _controllableManager.CurrentControllableBehaviour.EntityTransform;
			_cameraManager.SetCameraTarget(entityTarget);
			
			_cursorManager.OnCursorStateChanged += HandleCursorStateChanged;
			_characterGroup.Init();
		}

		public override void Dispose()
		{
			_cursorManager.OnCursorStateChanged -= HandleCursorStateChanged;
			_characterGroup.Dispose();
		}

		private void StartCharacterLife()
		{
			_charactersManager.CreateDefaultCharacter();
			_characterGroup = _charactersManager.CurrentCharacter;
			_characterBehaviour = _characterGroup.CharacterBehaviour;
		}

		private void LocomotionProvider()
		{
			var cameraLook = _cameraManager.Hierarchy.CameraLook;
			
			var locomotionProvider = new CharacterLocomotionProvider();
			locomotionProvider.SetBehaviour(_characterBehaviour);
			locomotionProvider.SetRotateTarget(cameraLook);
			
			_charactersManager.CurrentCharacter.AddProvider(locomotionProvider);
		}

		private void EquipmentProvider()
		{
			var locomotionProvider = _characterGroup.GetProvider<CharacterLocomotionProvider>();
			
			var equipmentBehaviour = _characterBehaviour.GetComponent<CharacterEquipmentProviderBehaviour>();
			var equipmentProvider = new CharacterEquipmentProvider(locomotionProvider, equipmentBehaviour, _characterBehaviour.CharacterAnimator);
			_charactersManager.CurrentCharacter.AddBehaviour(equipmentBehaviour);
			_charactersManager.CurrentCharacter.AddProvider(equipmentProvider);
		}

		private void CombatProvider()
		{
			var equipmentBehaviour = _characterGroup.GetBehaviour<CharacterEquipmentProviderBehaviour>();
			var combatBehaviour = _characterBehaviour.GetComponent<CharacterCombatBehaviour>();
			var equipmentProvider = _characterGroup.GetProvider<CharacterEquipmentProvider>();
			var attributesProvider = _characterGroup.GetProvider<CharacterAttributesProvider>();
			
			var combatProvider = new CharacterCombatProvider(equipmentProvider, combatBehaviour, attributesProvider, _characterBehaviour.CharacterAnimator);
			
			_charactersManager.CurrentCharacter.AddBehaviour(equipmentBehaviour);
			_charactersManager.CurrentCharacter.AddProvider(combatProvider);
		}

		private void StartAttributeProvider()
		{
			var attributesProvider = new CharacterAttributesProvider();
			_charactersManager.CurrentCharacter.AddProvider(attributesProvider);
		}

		private void HandleCursorStateChanged()
		{
			_charactersManager.CurrentCharacter.GetProvider<CharacterLocomotionProvider>().IsActive = !_cursorManager.CursorActive;
		}
	}
}
