using FlatLands.Architecture;
using FlatLands.Equipments;
using UnityEngine;

namespace FlatLands.CharacterEquipment
{
    public sealed class CharacterEquipmentProvider : BaseEquipmentProvider
    {
        protected override string WeaponAnimatorLayerName => "WeaponLayer";
        protected override string WeaponAnimatorSubMachineName => "Take Put Weapon";

        private CharacterEquipmentConfig _config;
        
        public CharacterEquipmentProvider(BaseEquipmentBehaviour behaviour, Animator animator) : base(behaviour, animator) { }

         public void Init()
         {
             _config = CharacterEquipmentConfig.Instance;
             UnityEventsProvider.OnUpdate += OnUpdate;
         }
         
         public void Dispose()
         {
             UnityEventsProvider.OnUpdate -= OnUpdate;
         }

         private void OnUpdate()
         {
             UpdateInput();
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