using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Equipments
{
    public abstract class BaseEquipmentWeaponBehaviour : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public Animator WeaponAnimator => _animator;
    }
}