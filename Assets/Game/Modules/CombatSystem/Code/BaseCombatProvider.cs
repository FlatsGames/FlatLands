using UnityEngine;

namespace FlatLands.CombatSystem
{
	public abstract class BaseCombatProvider
	{
		private Animator _animator;
		
		public BaseCombatProvider(Animator animator)
		{
			_animator = animator;
		}
		
		
	}
}
