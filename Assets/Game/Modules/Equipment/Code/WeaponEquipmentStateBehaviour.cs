using System;
using UnityEngine;
using UnityEngine.Animations;

namespace FlatLands.Equipments
{
    public enum WeaponAnimationType
    {
        Take = 10,
        Hold = 20,
        Put = 30,
    }
    
    public sealed class WeaponEquipmentStateBehaviour : StateMachineBehaviour
    {
        [SerializeField] private WeaponEquipmentSlotType _slotType;
        [SerializeField] private WeaponAnimationType _animationType;

        public WeaponEquipmentSlotType SlotType => _slotType;
        public WeaponAnimationType AnimationType => _animationType;

        public event Action<AnimatorStateInfo> OnStateEntered;
        public event Action<AnimatorStateInfo> OnStateExited;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex, controller);
            OnStateEntered?.Invoke(stateInfo);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
        {
            base.OnStateExit(animator, stateInfo, layerIndex, controller);
            OnStateExited?.Invoke(stateInfo);
        }
    }
}