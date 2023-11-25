using FlatLands.CombatSystem;
using FlatLands.Equipments;
using UnityEngine;

namespace FlatLands.CharacterCombat
{
    [CreateAssetMenu(
        menuName = "FlatLands/Characters/" + nameof(CharacterCombatConfig), 
        fileName = nameof(CharacterCombatConfig))]
    public sealed class CharacterCombatConfig : BaseCombatConfig
    {
        [SerializeField] private WeaponEquipmentSlotType _animCategory;

        public override WeaponEquipmentSlotType Category => _animCategory;
    }
}