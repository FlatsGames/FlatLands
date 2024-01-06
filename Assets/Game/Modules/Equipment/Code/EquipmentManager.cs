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

            var parent = targetProvider.GetSlotBehaviour(slotType).PivotTrans;
            var createdItem = GameObject.Instantiate(equipmentItemConfig.EquipmentItemPrefab, parent);
            targetProvider.SetItemToSlot(slotType, equipmentItemConfig, createdItem);
            OnEquipmentItemAdded?.Invoke(slotType);
        }
        
        public void RemoveEquipmentItemFromProvider(EquipmentSlotType equipmentSlotType, BaseEquipmentProvider targetProvider)
        {
            if(targetProvider == null)
                return;
            
            if(!targetProvider.HasSlot(equipmentSlotType))
                return;
            
            var itemInSlot = targetProvider.GetItemBehaviourInSlot(equipmentSlotType);
            if (itemInSlot == null)
                return;
                
            GameObject.Destroy(itemInSlot);
            targetProvider.SetItemToSlot(equipmentSlotType, null,null);
            OnEquipmentItemRemoved?.Invoke(equipmentSlotType);
        }
    }
}