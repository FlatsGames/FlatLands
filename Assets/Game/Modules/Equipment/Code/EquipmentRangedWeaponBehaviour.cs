using UnityEngine;

namespace FlatLands.Equipments
{
    public sealed class EquipmentRangedWeaponBehaviour : BaseEquipmentWeaponBehaviour
    {
        [SerializeField] 
        private Transform _projectileContainer;

        public Transform ProjectileContainer => _projectileContainer;
    }
}