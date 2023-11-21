using System.Collections.Generic;
using System.Linq;
using FlatLands.Architecture;
using FlatLands.Equipments;
using UnityEngine;

namespace FlatLands.CharacterEquipment
{
    [CreateAssetMenu(
        menuName = "FlatLands/Characters/" + nameof(CharacterEquipmentConfig), 
        fileName = nameof(CharacterEquipmentConfig))]
    public sealed class CharacterEquipmentConfig : SingletonScriptableObject<CharacterEquipmentConfig>
    {
        [SerializeField] 
        private Dictionary<KeyCode, WeaponEquipmentSlotType> _weaponInputKeys;
        
        [SerializeField] 
        private Dictionary<WeaponEquipmentSlotType, CharacterEquipmentWeaponPair> _weaponEquipmentSettings;

        public IReadOnlyList<KeyCode> WeaponInputKeys => _weaponInputKeys.Keys.ToList();

        public WeaponEquipmentSlotType GetWeaponInputKey(KeyCode keyType)
        {
            if (!_weaponInputKeys.TryGetValue(keyType, out var slotType))
                return default;

            return slotType;
        }

        public IWeaponEquipmentSetting GetWeaponEquipmentSettings(WeaponEquipmentSlotType slotType)
        {
            if (!_weaponEquipmentSettings.TryGetValue(slotType, out var settings))
                return default;

            return settings;
        }
    }

    internal sealed class CharacterEquipmentWeaponPair : IWeaponEquipmentSetting
    {
        [SerializeField] private string _takeWeaponAnimationName;
        [SerializeField] private string _putWeaponAnimationName;

        [SerializeField] private Vector3 _posInRightHand;
        [SerializeField] private Vector3 _rotInRightHand;

        public string TakeWeaponAnimationName => _takeWeaponAnimationName;
        public string PutWeaponAnimationName => _putWeaponAnimationName;

        public Vector3 PosInRightHand => _posInRightHand;
        public Vector3 RotInRightHand => _rotInRightHand;
    }
}