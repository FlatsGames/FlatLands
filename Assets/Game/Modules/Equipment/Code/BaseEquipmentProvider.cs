using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.Equipments
{
    public abstract class BaseEquipmentProvider
    {
        protected abstract string WeaponAnimatorLayerName { get; }
        protected abstract string WeaponAnimatorSubMachineName { get; }
        
        protected int WeaponAnimatorLayerIndex => _animator.GetLayerIndex(WeaponAnimatorLayerName);

        public bool IsHoldWeapon => _currentSlot != null;

        private readonly Dictionary<WeaponEquipmentSlotType, Dictionary<WeaponAnimationType, WeaponEquipmentStateBehaviour>> _weaponEquipmentAnimations;
        protected readonly BaseEquipmentBehaviour _behaviour;
        protected readonly Animator _animator;

        private EquipmentSlotBehaviour _currentSlot;
        private IEnumerator _handsCoroutine;
        
        protected BaseEquipmentProvider(BaseEquipmentBehaviour behaviour, Animator animator)
        {
            _behaviour = behaviour;
            _animator = animator;

            _weaponEquipmentAnimations =
                new Dictionary<WeaponEquipmentSlotType,
                    Dictionary<WeaponAnimationType, WeaponEquipmentStateBehaviour>>();

            ApplyWeaponAnimationStates();
        }

#region Animator

        public abstract IWeaponEquipmentSetting GetEquipmentSettings(WeaponEquipmentSlotType slotType);

        private void ApplyWeaponAnimationStates()
        {
            var behaviours = _animator.GetBehaviours<WeaponEquipmentStateBehaviour>();
            foreach (var equipmentBehaviour in behaviours)
            {
                if (!_weaponEquipmentAnimations.ContainsKey(equipmentBehaviour.SlotType))
                {
                    _weaponEquipmentAnimations[equipmentBehaviour.SlotType] =
                        new Dictionary<WeaponAnimationType, WeaponEquipmentStateBehaviour>();
                }
                
                _weaponEquipmentAnimations[equipmentBehaviour.SlotType]
                    .Add(equipmentBehaviour.AnimationType, equipmentBehaviour);
            }
        }

        private WeaponEquipmentStateBehaviour GetAnimatorStateBehaviour(WeaponEquipmentSlotType slotType, WeaponAnimationType animationType)
        {
            if (!_weaponEquipmentAnimations.TryGetValue(slotType, out var slotPairs))
                return default;

            if (!slotPairs.TryGetValue(animationType, out var stateBehaviour))
                return default;

            return stateBehaviour;
        }

        //ActionTime range 0 to 1
        private IEnumerator PlayEquipmentAnimation(string animName, float? actionTime = null, Action action = null, Action completeCallback = null)
        {
            yield return _animator.PlayAnimationWithAction(
                animName, 
                WeaponAnimatorSubMachineName, 
                actionTime, 
                action,
                completeCallback);
        }
        
#endregion


#region Take / Remove

        public void TakeWeaponToHands(WeaponEquipmentSlotType slotType, Action callback = null)
        {
            if(_handsCoroutine != null)
                UnityEventsProvider.CoroutineStop(_handsCoroutine);
            
            if (_currentSlot != null && IsHoldWeapon)
            {
                if (_currentSlot.SlotType.Equals(slotType))
                {
                    RemoveWeaponFromHands();
                    return;
                }
                
                RemoveWeaponFromHands(EquipWeapon);
            }
            else
            {
                EquipWeapon();
            }
            
            void EquipWeapon()
            {
                var slotBehaviour = _behaviour.GetBehaviour(slotType);
                _currentSlot = slotBehaviour;
                
                if (_currentSlot == null)
                {
                    Debug.LogError("[BaseEquipmentProvider] SlotBehaviour is null!");
                    return;
                }

                var equipmentSettings = GetEquipmentSettings(_currentSlot.SlotType);
                var animName = equipmentSettings.TakeWeaponAnimationName;
            
                _handsCoroutine = PlayEquipmentAnimation(
                    animName,
                    0.5f, 
                    () =>
                    {
                        var handHolder = _behaviour.RightHandHolder;
                        var equippedWeapon = _currentSlot.PivotTrans;
                        equippedWeapon.SetParent(handHolder);
                        equippedWeapon.DOLocalMove(equipmentSettings.PosInRightHand, 0.2f);
                        equippedWeapon.DOLocalRotate(equipmentSettings.RotInRightHand, 0.2f);
                    },
                    callback);
                
                UnityEventsProvider.CoroutineStart(_handsCoroutine);
            }
        }

        public void RemoveWeaponFromHands(Action callback = null)
        {
            if(!IsHoldWeapon)
            {
                _currentSlot = null;
                callback?.Invoke();
                return;
            }

            var equipmentSettings = GetEquipmentSettings(_currentSlot.SlotType);
            var animName = equipmentSettings.PutWeaponAnimationName;

            _handsCoroutine = PlayEquipmentAnimation(
                animName, 
                0.5f, 
                () =>
                {
                    var equippedWeapon = _currentSlot.PivotTrans;
                    equippedWeapon.SetParent(_currentSlot.transform);
                    equippedWeapon.DOLocalMove(Vector3.zero, 0.2f);
                    equippedWeapon.DOLocalRotateQuaternion(Quaternion.identity, 0.2f);
                },
                () =>
                {
                    _currentSlot = null;
                    callback?.Invoke();
                });
            
            UnityEventsProvider.CoroutineStart(_handsCoroutine);
        }
        
#endregion


#region Hands Constrints

        public void FreeLeftHand(float duration = 0.3f, Action callback = null)
        {
            var leftHandConstraint = _behaviour.LeftHandConstraint;
            TweenTo(leftHandConstraint.weight, 0, duration,
                (curWeight) =>
                {
                    leftHandConstraint.weight = curWeight;
                }, callback);
        }
        
        public void FreeRightHand(float duration = 0.3f, Action callback = null)
        {
            var rightHandConstraint = _behaviour.RightHandConstraint;
            TweenTo(rightHandConstraint.weight, 0, duration,
                (curWeight) =>
                {
                    rightHandConstraint.weight = curWeight;
                }, callback);
        }

        private void TweenTo(float startValue, float endValue, float duration, Action<float> onUpdate = null, Action onCompleted = null)
        {
            var curValue = startValue;
            DOTween.To(() => curValue, setter => curValue = setter, endValue, duration)
                .OnUpdate(() =>
                {
                    onUpdate?.Invoke(curValue);
                })
                .OnComplete(() =>
                {
                    onCompleted?.Invoke();
                });
        }

#endregion
        
    }
}