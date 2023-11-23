using System.Linq;
using FlatLands.CharacterEquipment;
using FlatLands.Characters;
using FlatLands.CombatSystem;
using UnityEngine;

namespace FlatLands.CharacterCombat
{
	public sealed class CharacterCombatProvider : BaseCombatProvider<CharacterCombatAnimations>, ICharacterProvider
	{
		private readonly CharacterEquipmentProvider _characterEquipmentProvider;

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
		}
		
		public void Dispose()
		{
			
		}
		
		public void HandleUpdate()
		{
			if(Input.GetMouseButtonDown(0))
			{
				var config = CharacterCombatConfig.ByName["CharacterCombat_LeftLegSlot"];
				var maxAnimCount = config.CombatAnimations.Count();
				var randomAttackIndex = Random.Range(0, maxAnimCount);
				var attackName = config.CombatAnimations[randomAttackIndex];
				
				Attack(config, attackName);
			}
		}
		
		public void HandleFixedUpdate()
		{
			
		}
	}
}
