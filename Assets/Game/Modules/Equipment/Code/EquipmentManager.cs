using System;
using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.Equipments
{
    public sealed class EquipmentManager : SharedObject
    {
        public event Action<EquipmentSlotType> OnEquipmentItemAdded;
        public event Action<EquipmentSlotType> OnEquipmentItemRemoved;
        
        public void AddEquipmentItemToProvider(BaseEquipmentItemConfig equipmentItemConfig, BaseEquipmentProvider targetProvider)
        {
            if(equipmentItemConfig == null 
               || targetProvider == null)
                return;

            var slotType = equipmentItemConfig.SlotType;
            if(!targetProvider.HasSlot(slotType))
                return;
            
            var itemInSlot = targetProvider.GetItemConfigInSlot(slotType);
            if (itemInSlot != null)
                RemoveEquipmentItemFromProvider(slotType, targetProvider);

            var slotParent = targetProvider.GetSlotBehaviour(slotType).SlotTrans;
            var createdItem = GameObject.Instantiate(equipmentItemConfig.EquipmentItemPrefab, slotParent, false);
            slotParent.localPosition = equipmentItemConfig.PosOnBody;
            slotParent.localRotation = Quaternion.Euler(equipmentItemConfig.RotOnBody);
            
            targetProvider.SetItemToSlot(slotType, equipmentItemConfig, createdItem);
            OnEquipmentItemAdded?.Invoke(slotType);
        }
        
        public void RemoveEquipmentItemFromProvider(EquipmentSlotType slotType, BaseEquipmentProvider targetProvider)
        {
            if(targetProvider == null)
                return;
            
            if(!targetProvider.HasSlot(slotType))
                return;
            
            var itemInSlot = targetProvider.GetItemBehaviourInSlot(slotType);
            if (itemInSlot == null)
                return;
            
            GameObject.Destroy(itemInSlot.gameObject);
            targetProvider.SetItemToSlot(slotType, null,null);
            
            var slotParent = targetProvider.GetSlotBehaviour(slotType).SlotTrans;
            slotParent.localPosition = Vector3.zero;
            slotParent.localRotation = Quaternion.identity;

            OnEquipmentItemRemoved?.Invoke(slotType);
        }
    }
}