using System.Collections;
using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.CombatSystem
{
	public abstract class BaseCombatProvider
	{
		public int AnimatorExitBlockHash => Animator.StringToHash("ExitBlock");
		
		protected abstract bool IsHoldWeapon { get; }
		protected bool AttackInProgress { get; private set; }
		
		protected Animator _animator;
		
		private BaseCombatBehaviour _combatBehaviour;
		private IEnumerator _attackRoutine;

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
	}
}
