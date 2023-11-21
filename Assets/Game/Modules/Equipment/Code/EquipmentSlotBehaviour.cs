using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Equipments
{
    public sealed class EquipmentSlotBehaviour : SerializedMonoBehaviour
    {
        [SerializeField] private WeaponEquipmentSlotType _slotType;
        [SerializeField] private Transform _pivotTransform;

        public WeaponEquipmentSlotType SlotType => _slotType;
        public Transform PivotTrans => _pivotTransform;
    }
}