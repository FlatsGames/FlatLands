using UnityEngine;
using UnityEngine.Animations;

namespace FlatLands.Characters
{
	public sealed class CharacterWeaponStateMachineBehaviour : StateMachineBehaviour
	{

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
			AnimatorControllerPlayable controller)
		{
			
			base.OnStateEnter(animator, stateInfo, layerIndex, controller);
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
		{
			
			base.OnStateExit(animator, stateInfo, layerIndex, controller);
		}
	}
}
