using System;
using System.Collections;
using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.CombatSystem
{
	public abstract class BaseCombatProvider<TAnimEnum>
		where TAnimEnum : Enum
	{
		protected abstract bool IsHoldWeapon { get; }
		
		private BaseCombatBehaviour _combatBehaviour;
		private bool _attackInProgress;
		private Animator _animator;
		private IEnumerator _attackRoutine;
		
		public BaseCombatProvider(BaseCombatBehaviour combatBehaviour, Animator animator)
		{
			_combatBehaviour = combatBehaviour;
			_animator = animator;
		}

		protected void Attack(BaseCombatConfig<TAnimEnum> combatConfig, TAnimEnum animAttackType)
		{
			if(_attackInProgress || !IsHoldWeapon)
				return;
			
			_attackInProgress = true;
			var animName = animAttackType.ToString();
			_attackRoutine = _animator.PlayAnimationWithAction(
				combatConfig.AnimatorSubLayer, 
				animName, 
				completeCallback: () =>
				{
					_attackInProgress = false;
				});
			UnityEventsProvider.CoroutineStart(_attackRoutine);
		}
	}
}
