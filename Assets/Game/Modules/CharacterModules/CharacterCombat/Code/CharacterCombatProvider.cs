using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlatLands.Architecture;
using FlatLands.CharacterAttributes;
using FlatLands.CharacterEquipment;
using FlatLands.Characters;
using FlatLands.CombatSystem;
using FlatLands.Conditions;
using FlatLands.Equipments;
using FlatLands.GameAttributes;
using UnityEngine;

namespace FlatLands.CharacterCombat
{
	public sealed class CharacterCombatProvider : BaseCombatProvider, ICharacterProvider
	{
		private const int Left_Mouse_Button = 0;
		private const int Right_Mouse_Button = 1;
		
		[Inject] private GameAttributesManager _gameAttributesManager;
		[Inject] private ConditionsController _conditionsController;

		private readonly CharacterEquipmentProvider _characterEquipmentProvider;
		private readonly CharacterAttributesProvider _characterAttributesProvider;
		private Dictionary<WeaponEquipmentSlotType, CharacterCombatConfig> _equipmentToSlots;

		protected override bool IsHoldWeapon => _characterEquipmentProvider?.IsHoldWeapon ?? false;
		protected bool IsBlockActive { get; private set; }

		private CharacterCombatConfig _currentConfig;
		private AttributeRegeneratedData _staminaAttribute;
		
		private IEnumerator _blockRoutine;
		private IEnumerator _blockCooldownRoutine;
		
		public CharacterCombatProvider(
			CharacterEquipmentProvider equipmentProvider,
			CharacterCombatBehaviour combatBehaviour,
			CharacterAttributesProvider characterAttributesProvider,
			Animator animator)
			: base(combatBehaviour, animator)
		{
			_characterEquipmentProvider = equipmentProvider;
			_characterAttributesProvider = characterAttributesProvider;
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

			_staminaAttribute = _gameAttributesManager
				.GetAttributeHolder(CharacterAttributesProvider.CharacterHolderId)
				.GetAttribute<AttributeRegeneratedData>(GameAttributeType.Stamina);

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
			{
				ApplyAttack();
				ApplyShoot();
			}
			
			if(Input.GetMouseButton(Right_Mouse_Button))
			{
				EnterBlock();
				EnterAiming();
			}
			
			if(Input.GetMouseButtonUp(Right_Mouse_Button))
			{
				ExitBlock();
				ExitAiming();
			}
		}

#region Simple Attack

		private void ApplyAttack()
		{
			var meleeInternalConfig = _currentConfig.GetInternalSetting<CombatInternalMeleeAttackSetting>();
			if(meleeInternalConfig != null)
			{
				if (!_staminaAttribute.CanRemoveValue(meleeInternalConfig.AttackCost))
					return;

				var maxAnimCount = meleeInternalConfig.AttackAnimations.Count();
				var randomAttackIndex = Random.Range(0, maxAnimCount);
				var animationName = meleeInternalConfig.AttackAnimations[randomAttackIndex];

				if (!AttackInProgress)
					_staminaAttribute.RemoveValue(meleeInternalConfig.AttackCost);

				Attack(_currentConfig, animationName);
			}
		}

#endregion


#region Block

		private void EnterBlock()
		{
			var meleeInternalConfig = _currentConfig.GetInternalSetting<CombatInternalMeleeAttackSetting>();
			if(meleeInternalConfig != null)
			{
				if (IsBlockActive)
					return;

				IsBlockActive = true;
				_blockRoutine = _animator.PlayAnimation(
					_currentConfig.AnimatorSubLayer,
					meleeInternalConfig.BlockStartAnimation);
				UnityEventsProvider.CoroutineStart(_blockRoutine);
			}
		}
		
		private void ExitBlock()
		{
			var meleeInternalConfig = _currentConfig.GetInternalSetting<CombatInternalMeleeAttackSetting>();
			if(meleeInternalConfig != null)
			{
				if (!IsBlockActive)
					return;

				_animator.SetTrigger(AnimatorExitBlockHash);
				if (!meleeInternalConfig.HasBlockCooldown)
				{
					IsBlockActive = false;
					return;
				}

				_blockCooldownRoutine = BlockCooldown();
				UnityEventsProvider.CoroutineStart(_blockCooldownRoutine);

				IEnumerator BlockCooldown()
				{
					yield return new WaitForSeconds(meleeInternalConfig.BlockCooldownSeconds);
					IsBlockActive = false;
				}
			}
		}
		
#endregion


#region Ranged Attack

		protected bool IsRangedAiming { get; private set; }

		private void EnterAiming()
		{
			if(IsRangedAiming)
				return;
			
			var rangedInternalConfig = _currentConfig.GetInternalSetting<CombatInternalRangedAttackSetting>();
			if(rangedInternalConfig == null)
				return;

			IsRangedAiming = true;
			var startAimingRoutine = _animator.PlayAnimation(
				_currentConfig.AnimatorSubLayer, 
				rangedInternalConfig.StartAimingAnim);

			_conditionsController.ApplyConditions(rangedInternalConfig.StartConditions);
			
			UnityEventsProvider.CoroutineStart(startAimingRoutine);
		}
		
		private void ExitAiming()
		{
			if(!IsRangedAiming)
				return;
			
			var rangedInternalConfig = _currentConfig.GetInternalSetting<CombatInternalRangedAttackSetting>();
			if(rangedInternalConfig == null)
				return;
			
			var endAimingRoutine = _animator.PlayAnimation(
				_currentConfig.AnimatorSubLayer, 
				rangedInternalConfig.EndAimingAnim, 
				() =>
				{
					IsRangedAiming = false;
				});
			
			_conditionsController.ApplyConditions(rangedInternalConfig.EndConditions);
			
			UnityEventsProvider.CoroutineStart(endAimingRoutine);
		}

		private void ApplyShoot()
		{
			if(!IsRangedAiming)
				return;
			
			var rangedInternalConfig = _currentConfig.GetInternalSetting<CombatInternalRangedAttackSetting>();
			if(rangedInternalConfig == null)
				return;
			
			var shootRoutine = _animator.PlayAnimation(
				_currentConfig.AnimatorSubLayer, 
				rangedInternalConfig.ShootAnim);
			UnityEventsProvider.CoroutineStart(shootRoutine);
			Debug.LogError("Shoot");
		}
		
#endregion

		private void HandleEquipmentWeaponChanged()
		{
			if(!IsHoldWeapon)
				return;

			_equipmentToSlots.TryGetValue(_characterEquipmentProvider.CurrentEquipmentWeapon, out _currentConfig);
		}
	}
}
