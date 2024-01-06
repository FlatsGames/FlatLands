using FlatLands.Architecture;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Equipments
{
    public abstract class BaseEquipmentItemConfig : MultitonScriptableObjectsByName<BaseEquipmentItemConfig>
    {
        [SerializeField, BoxGroup("Main Settings")] 
        private EquipmentItemBehaviour _equipmentItemPrefab;
        
        [SerializeField, BoxGroup("Main Settings")] 
        private EquipmentSlotType _equipmentSlotType;

        [SerializeField, BoxGroup("Main Settings")]
        private Vector3 _posOnBody;
        
        [SerializeField, BoxGroup("Main Settings")]
        private Vector3 _rotOnBody;
        
        public EquipmentItemBehaviour EquipmentItemPrefab => _equipmentItemPrefab;
        public EquipmentSlotType SlotType => _equipmentSlotType;

        public Vector3 PosOnBody => _posOnBody;
        public Vector3 RotOnBody => _rotOnBody;
    }
}