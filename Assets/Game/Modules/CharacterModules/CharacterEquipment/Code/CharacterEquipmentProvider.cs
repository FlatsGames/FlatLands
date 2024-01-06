using FlatLands.CharacterLocomotion;
using FlatLands.Characters;
using FlatLands.Equipments;
using UnityEngine;

namespace FlatLands.CharacterEquipment
{
    public sealed class CharacterEquipmentProvider : BaseEquipmentProvider, ICharacterProvider
    {
        protected override string AnimatorLayerName => "EquipmentLayer";

        private CharacterEquipmentConfig _config;
        private CharacterLocomotionProvider _characterLocomotionProvider;

        public CharacterEquipmentProvider(CharacterLocomotionProvider characterLocomotionProvider, BaseEquipmentProviderBehaviour providerBehaviour, Animator animator)
            : base(providerBehaviour, animator)
        {
            _characterLocomotionProvider = characterLocomotionProvider;
        }

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

        // public override IWeaponEquipmentSetting GetEquipmentSettings(EquipmentSlotType slotType)
        // { 
        //     return _config.GetWeaponEquipmentSettings(slotType);
        // }

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

        private void TakeWeapon(EquipmentSlotType slotType)
        {
            //TakeWeaponToHands(slotType);
        }

        protected void HandleEquipmentWeaponChanged()
        {
            // var newLocomotionType = IsHoldWeapon 
            //         ? CharacterLocomotionType.OnlyStrafe 
            //         : CharacterLocomotionType.OnlyFree;

            //_characterLocomotionProvider.LocomotionType = newLocomotionType;
        }
    }
}