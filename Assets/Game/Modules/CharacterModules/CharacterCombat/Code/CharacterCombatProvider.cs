using System.Collections.Generic;
using System.Linq;
using FlatLands.CharacterEquipment;
using FlatLands.Characters;
using FlatLands.CombatSystem;
using FlatLands.Equipments;
using UnityEngine;

namespace FlatLands.CharacterCombat
{
	public sealed class CharacterCombatProvider : BaseCombatProvider<CharacterCombatAnimations>, ICharacterProvider
	{
		private const int Left_Mouse_Button = 0;
		private const int Right_Mouse_Button = 1;
		
		private readonly CharacterEquipmentProvider _characterEquipmentProvider;
		private Dictionary<WeaponEquipmentSlotType, CharacterCombatConfig> _equipmentToSlots;

		protected override bool IsHoldWeapon => _characterEquipmentProvider?.IsHoldWeapon ?? false;

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
		}
		
		public void Dispose()
		{
			
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
				ApplyBlock();
		}

		private void ApplyAttack()
		{
			if(!_equipmentToSlots.TryGetValue(_characterEquipmentProvider.CurrentEquipmentWeapon, out var combatConfig))
				return;
			
			var maxAnimCount = combatConfig.CombatAnimations.Count();
			var randomAttackIndex = Random.Range(0, maxAnimCount);
			var attackName = combatConfig.CombatAnimations[randomAttackIndex];
				
			Attack(combatConfig, attackName);
		}

		private void ApplyBlock()
		{
			
		}
	}
}
