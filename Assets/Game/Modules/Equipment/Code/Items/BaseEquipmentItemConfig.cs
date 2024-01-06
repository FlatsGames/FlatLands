using FlatLands.Architecture;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Equipments
{
    public abstract class BaseEquipmentItemConfig : MultitonScriptableObjectsByName<BaseEquipmentItemConfig>
    {
        [SerializeField, BoxGroup("Main Settings")] private EquipmentItemBehaviour _equipmentItemPrefab;
        [SerializeField, BoxGroup("Main Settings")] private EquipmentSlotType _equipmentSlotType;

        public EquipmentItemBehaviour EquipmentItemPrefab => _equipmentItemPrefab;
        public EquipmentSlotType SlotType => _equipmentSlotType;
    }
}