using UnityEngine;

namespace FlatLands.Equipments
{
    public interface IWeaponEquipmentSetting
    {
        public string TakeWeaponAnimationName { get; }
        public string PutWeaponAnimationName { get; }
        public Vector3 PosInRightHand { get; }
        public Vector3 RotInRightHand { get; }
    }
}