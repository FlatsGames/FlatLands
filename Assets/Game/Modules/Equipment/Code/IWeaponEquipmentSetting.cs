using UnityEngine;

namespace FlatLands.Equipments
{
    public interface IWeaponEquipmentSetting
    {
        public string TakeWeaponAnimationName { get; }
        public string PutWeaponAnimationName { get; }
        
        public bool UseRightHand { get; }
        public Vector3 PosInHand { get; }
        public Vector3 RotInHand { get; }
    }
}