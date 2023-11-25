using System.Collections;
using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.CombatSystem
{
	public abstract class BaseCombatProvider
	{
		public int AnimatorExitBlockHash => Animator.StringToHash("ExitBlock");
		
		protected abstract bool IsHoldWeapon { get; }
		protected bool IsBlockActive { get; private set; }
		protected bool AttackInProgress { get; private set; }
		
		private BaseCombatBehaviour _combatBehaviour;
		private Animator _animator;
		private IEnumerator _attackRoutine;
		private IEnumerator _blockRoutine;
		private IEnumerator _blockCooldownRoutine;
		
		public BaseCombatProvider(BaseCombatBehaviour combatBehaviour, Animator animator)
		{
			_combatBehaviour = combatBehaviour;
			_animator = animator;
		}

		protected void Attack(BaseCombatConfig combatConfig, string animName)
		{
			if(AttackInProgress || !IsHoldWeapon)
				return;
			
			AttackInProgress = true;
			_attackRoutine = _animator.PlayAnimationWithAction(
				combatConfig.AnimatorSubLayer, 
				animName, 
				completeCallback: () =>
				{
					AttackInProgress = false;
				});
			UnityEventsProvider.CoroutineStart(_attackRoutine);
		}

		protected void EnterBlock(BaseCombatConfig combatConfig)
		{
			if(IsBlockActive)
				return;

			IsBlockActive = true;
			_blockRoutine = _animator.PlayAnimation(
				combatConfig.AnimatorSubLayer, 
				combatConfig.BlockStartAnimation);
			UnityEventsProvider.CoroutineStart(_blockRoutine);
		}
		
		protected void ExitBlock(BaseCombatConfig combatConfig)
		{
			if(!IsBlockActive)
				return;
			
			_animator.SetTrigger(AnimatorExitBlockHash);
			if(!combatConfig.HasBlockCooldown)
			{
				IsBlockActive = false;
				return;
			}

			_blockCooldownRoutine = BlockCooldown();
			UnityEventsProvider.CoroutineStart(_blockCooldownRoutine);
			
			IEnumerator BlockCooldown()
			{
				yield return new WaitForSeconds(combatConfig.BlockCooldownSeconds);
				IsBlockActive = false;
			}
		}
	}
}
