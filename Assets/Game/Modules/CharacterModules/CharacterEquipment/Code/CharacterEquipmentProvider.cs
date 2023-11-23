using FlatLands.Architecture;
using FlatLands.Characters;
using FlatLands.Equipments;
using UnityEngine;

namespace FlatLands.CharacterEquipment
{
    public sealed class CharacterEquipmentProvider : BaseEquipmentProvider, ICharacterProvider
    {
        protected override string WeaponAnimatorLayerName => "WeaponLayer";
        protected override string WeaponAnimatorSubMachineName => "Take Put Weapon";

        private CharacterEquipmentConfig _config;
        
        public CharacterEquipmentProvider(BaseEquipmentBehaviour behaviour, Animator animator) : base(behaviour, animator) { }

        public void Init()
        { 
            _config = CharacterEquipmentConfig.Instance;
        }

        public void Dispose() 
        {
         
        }

        public void HandleUpdate()
        { 
            UpdateInput();
        }

        public void HandleFixedUpdate()
        {
         
        }

        public override IWeaponEquipmentSetting GetEquipmentSettings(WeaponEquipmentSlotType slotType)
        { 
            return _config.GetWeaponEquipmentSettings(slotType);
        }

        private void UpdateInput()
        {
            foreach (var key in _config.WeaponInputKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    var slotType = _config.GetWeaponInputKey(key);
                    TakeWeapon(slotType);
                }
            }
        }

        private void TakeWeapon(WeaponEquipmentSlotType slotType)
        {
            TakeWeaponToHands(slotType);
        }
    }
}