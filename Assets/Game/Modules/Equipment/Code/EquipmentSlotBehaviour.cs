﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Equipments
{
    public sealed class EquipmentSlotBehaviour : SerializedMonoBehaviour
    {
        [SerializeField] private EquipmentSlotType _slotType;
        [SerializeField] private Transform _pivotTransform;
        [SerializeField] private BaseEquipmentWeaponBehaviour _equipmentWeaponBehaviour;

        public EquipmentSlotType SlotType => _slotType;
        public Transform PivotTrans => _pivotTransform;
        public BaseEquipmentWeaponBehaviour WeaponBehaviour => _equipmentWeaponBehaviour;
    }
}