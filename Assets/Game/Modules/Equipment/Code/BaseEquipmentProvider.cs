using System.Collections.Generic;
using UnityEngine;

namespace FlatLands.Equipments
{
    public abstract class BaseEquipmentProvider
    {
        public IReadOnlyDictionary<EquipmentSlotType, (BaseEquipmentItemConfig, EquipmentItemBehaviour)> EquipmentSlots => _equipmentSlots;

        protected abstract string AnimatorLayerName { get; }
        protected int AnimatorLayerIndex => _animator.GetLayerIndex(AnimatorLayerName);

        private readonly Dictionary<EquipmentSlotType, (BaseEquipmentItemConfig, EquipmentItemBehaviour)> _equipmentSlots;

        protected readonly BaseEquipmentProviderBehaviour _providerBehaviour;
        protected readonly Animator _animator;
        
        protected BaseEquipmentProvider(BaseEquipmentProviderBehaviour providerBehaviour, Animator animator)
        {
            _equipmentSlots = new Dictionary<EquipmentSlotType, (BaseEquipmentItemConfig, EquipmentItemBehaviour)>();
            
            _providerBehaviour = providerBehaviour;
            _animator = animator;
            
            CreateEquipmentSlots();
        }

        private void CreateEquipmentSlots()
        {
            var slots = _providerBehaviour.GetAllSlots();
            foreach (var slot in slots)
            {
                _equipmentSlots[slot] = (null, null);
            }
        }

#region Slot Items

        public bool HasSlot(EquipmentSlotType slotType)
        {
            return _equipmentSlots.ContainsKey(slotType);
        }
        
        public BaseEquipmentItemConfig GetItemConfigInSlot(EquipmentSlotType slotType)
        {
            if (!_equipmentSlots.TryGetValue(slotType, out var item))
                return default;

            return item.Item1;
        }

        public EquipmentItemBehaviour GetItemBehaviourInSlot(EquipmentSlotType slotType)
        {
            if (!_equipmentSlots.TryGetValue(slotType, out var item))
                return default;

            return item.Item2;
        }
        
        public EquipmentSlotBehaviour GetSlotBehaviour(EquipmentSlotType slotType)
        {
            return _providerBehaviour.GetBehaviour(slotType);
        }

        public void SetItemToSlot(EquipmentSlotType slotType, BaseEquipmentItemConfig newItemConfig, EquipmentItemBehaviour newItemBehaviour)
        {
            _equipmentSlots[slotType] = (newItemConfig, newItemBehaviour);
        }

#endregion


// #region Animator
//
//         public abstract IWeaponEquipmentSetting GetEquipmentSettings(EquipmentSlotType slotType);
//
//         //ActionTime range 0 to 1
//         private IEnumerator PlayEquipmentAnimation(string animName, float? actionTime = null, Action action = null, Action completeCallback = null)
//         {
//             yield return _animator.PlayAnimationWithAction(AnimatorLayerName, animName, actionTime, action,
//                 completeCallback);
//         }
//         
// #endregion
//
//
// #region Take / Remove
//
//         public void TakeWeaponToHands(EquipmentSlotType slotType, Action callback = null)
//         {
//             if(_handsCoroutine != null)
//                 UnityEventsProvider.CoroutineStop(_handsCoroutine);
//             
//             if (_currentSlot != null && IsHoldWeapon)
//             {
//                 if (_currentSlot.SlotType.Equals(slotType))
//                 {
//                     RemoveWeaponFromHands();
//                     return;
//                 }
//                 
//                 RemoveWeaponFromHands(EquipWeapon);
//             }
//             else
//             {
//                 EquipWeapon();
//             }
//             
//             void EquipWeapon()
//             {
//                 var slotBehaviour = ProviderBehaviour.GetBehaviour(slotType);
//                 _currentSlot = slotBehaviour;
//                 OnCurrentEquipmentWeaponChanged?.Invoke();
//                 HandleEquipmentWeaponChanged();
//                 
//                 if (_currentSlot == null)
//                 {
//                     Debug.LogError("[BaseEquipmentProvider] SlotBehaviour is null!");
//                     return;
//                 }
//
//                 var equipmentSettings = GetEquipmentSettings(_currentSlot.SlotType);
//                 var animName = equipmentSettings.TakeWeaponAnimationName;
//             
//                 _handsCoroutine = PlayEquipmentAnimation(
//                     animName,
//                     0.5f, 
//                     () =>
//                     {
//                         var handHolder = equipmentSettings.UseRightHand 
//                             ? ProviderBehaviour.RightHandHolder 
//                             : ProviderBehaviour.LeftHandHolder;
//                         
//                         var equippedWeapon = _currentSlot.PivotTrans;
//                         equippedWeapon.SetParent(handHolder);
//                         equippedWeapon.DOLocalMove(equipmentSettings.PosInHand, 0.2f);
//                         equippedWeapon.DOLocalRotate(equipmentSettings.RotInHand, 0.2f);
//                     },
//                     callback);
//                 
//                 UnityEventsProvider.CoroutineStart(_handsCoroutine);
//             }
//         }
//
//         public void RemoveWeaponFromHands(Action callback = null)
//         {
//             if(!IsHoldWeapon)
//             {
//                 _currentSlot = null;
//                 OnCurrentEquipmentWeaponChanged?.Invoke();
//                 HandleEquipmentWeaponChanged();
//                 callback?.Invoke();
//                 return;
//             }
//
//             var equipmentSettings = GetEquipmentSettings(_currentSlot.SlotType);
//             var animName = equipmentSettings.PutWeaponAnimationName;
//
//             _handsCoroutine = PlayEquipmentAnimation(
//                 animName, 
//                 0.5f, 
//                 () =>
//                 {
//                     var equippedWeapon = _currentSlot.PivotTrans;
//                     equippedWeapon.SetParent(_currentSlot.transform);
//                     equippedWeapon.DOLocalMove(Vector3.zero, 0.2f);
//                     equippedWeapon.DOLocalRotateQuaternion(Quaternion.identity, 0.2f);
//                 },
//                 () =>
//                 {
//                     _currentSlot = null;
//                     OnCurrentEquipmentWeaponChanged?.Invoke();
//                     HandleEquipmentWeaponChanged();
//                     callback?.Invoke();
//                 });
//             
//             UnityEventsProvider.CoroutineStart(_handsCoroutine);
//         }
//
//         protected virtual void HandleEquipmentWeaponChanged() { }
//         
// #endregion
//
//
// #region Hands Constrints
//
//         // public void FreeLeftHand(float duration = 0.3f, Action callback = null)
//         // {
//         //     var leftHandConstraint = _behaviour.LeftHandConstraint;
//         //     TweenTo(leftHandConstraint.weight, 0, duration,
//         //         (curWeight) =>
//         //         {
//         //             leftHandConstraint.weight = curWeight;
//         //         }, callback);
//         // }
//         //
//         // public void FreeRightHand(float duration = 0.3f, Action callback = null)
//         // {
//         //     var rightHandConstraint = _behaviour.RightHandConstraint;
//         //     TweenTo(rightHandConstraint.weight, 0, duration,
//         //         (curWeight) =>
//         //         {
//         //             rightHandConstraint.weight = curWeight;
//         //         }, callback);
//         // }
//
//         private void TweenTo(float startValue, float endValue, float duration, Action<float> onUpdate = null, Action onCompleted = null)
//         {
//             var curValue = startValue;
//             DOTween.To(() => curValue, setter => curValue = setter, endValue, duration)
//                 .OnUpdate(() =>
//                 {
//                     onUpdate?.Invoke(curValue);
//                 })
//                 .OnComplete(() =>
//                 {
//                     onCompleted?.Invoke();
//                 });
//         }
//
// #endregion
        
    }
}