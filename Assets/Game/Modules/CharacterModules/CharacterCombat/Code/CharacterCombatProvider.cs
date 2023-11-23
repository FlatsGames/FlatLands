using System.Linq;
using FlatLands.Architecture;
using FlatLands.CharacterEquipment;
using FlatLands.CombatSystem;
using UnityEngine;

namespace FlatLands.CharacterCombat
{
	public sealed class CharacterCombatProvider : BaseCombatProvider<CharacterCombatAnimations>
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
			UnityEventsProvider.OnUpdate += HandleUpdate;
		}
		
		public void Dispose()
		{
			UnityEventsProvider.OnUpdate -= HandleUpdate;
		}

		private void HandleUpdate()
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
	}
}
