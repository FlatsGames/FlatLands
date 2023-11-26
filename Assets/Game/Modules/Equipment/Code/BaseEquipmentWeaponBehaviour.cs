using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Equipments
{
    public abstract class BaseEquipmentWeaponBehaviour : SerializedMonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public Animator WeaponAnimator => _animator;
    }
}