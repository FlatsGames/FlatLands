using System.Collections.Generic;
using System.Linq;
using FlatLands.CharacterEquipment;
using FlatLands.Characters;
using FlatLands.CombatSystem;
using FlatLands.Equipments;
using UnityEngine;

namespace FlatLands.CharacterCombat
{
	public sealed class CharacterCombatProvider : BaseCombatProvider, ICharacterProvider
	{
		private const int Left_Mouse_Button = 0;
		private const int Right_Mouse_Button = 1;
		
		private readonly CharacterEquipmentProvider _characterEquipmentProvider;
		private Dictionary<WeaponEquipmentSlotType, CharacterCombatConfig> _equipmentToSlots;

		protected override bool IsHoldWeapon => _characterEquipmentProvider?.IsHoldWeapon ?? false;

		private CharacterCombatConfig _currentConfig;
		
		public CharacterCombatProvider(
			CharacterEquipmentProvider equipmentProvider,
			CharacterCombatBehaviour combatBehaviour,
			Animator animator)
			: base(combatBehaviour, animator)
		{
			_characterEquipmentProvider = equipmentProvider;
		}
		
		public void Init()
		{
			CharacterCombatConfig.Init();
			_equipmentToSlots = new Dictionary<WeaponEquipmentSlotType, CharacterCombatConfig>();
			var configs = CharacterCombatConfig.Objects;
			foreach (var config in configs)
			{
				if(config is not CharacterCombatConfig combatConfig)
					continue;
				
				_equipmentToSlots[config.Category] = combatConfig;
			}

			_characterEquipmentProvider.OnCurrentEquipmentWeaponChanged += HandleEquipmentWeaponChanged;
		}
		
		public void Dispose()
		{
			_characterEquipmentProvider.OnCurrentEquipmentWeaponChanged -= HandleEquipmentWeaponChanged;
		}
		
		public void HandleUpdate()
		{
			UpdateInput();
		}
		
		public void HandleFixedUpdate()
		{
			
		}

		private void UpdateInput()
		{
			if(!IsHoldWeapon)
				return;
			
			if(Input.GetMouseButtonDown(Left_Mouse_Button))
				ApplyAttack();
			
			if(Input.GetMouseButton(Right_Mouse_Button))
				ActivateBlock();
			
			if(Input.GetMouseButtonUp(Right_Mouse_Button))
				DeactivateBlock();
		}

		private void ApplyAttack()
		{
			var maxAnimCount = _currentConfig.CombatAnimations.Count();
			var randomAttackIndex = Random.Range(0, maxAnimCount);
			var attackName = _currentConfig.CombatAnimations[randomAttackIndex];
				
			Attack(_currentConfig, attackName);
		}

		private void ActivateBlock()
		{
			EnterBlock(_currentConfig);
		}
		
		private void DeactivateBlock()
		{
			ExitBlock(_currentConfig);
		}

		private void HandleEquipmentWeaponChanged()
		{
			if(!IsHoldWeapon)
				return;

			_equipmentToSlots.TryGetValue(_characterEquipmentProvider.CurrentEquipmentWeapon, out _currentConfig);
		}
	}
}
