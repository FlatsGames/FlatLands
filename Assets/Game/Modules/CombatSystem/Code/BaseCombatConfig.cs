using System;
using System.Collections.Generic;
using FlatLands.Architecture;
using FlatLands.Equipments;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.CombatSystem
{
    public abstract class BaseCombatConfig : MultitonScriptableObjectsByName<BaseCombatConfig>
    {
        [SerializeField, FoldoutGroup("Main Settings")] 
        private string _animatorSubLayer;
        
        [SerializeField, FoldoutGroup("Internal Settings")] 
        private List<ICombatInternalSetting> _internalSettings 
            = new List<ICombatInternalSetting>();

        public string AnimatorSubLayer => _animatorSubLayer;
        public abstract WeaponEquipmentSlotType Category { get; }

        private Dictionary<Type, ICombatInternalSetting> _internalSettingsByTypes;

        protected override void Initialize()
        {
            _internalSettingsByTypes = new Dictionary<Type, ICombatInternalSetting>();
            foreach (var internalSetting in _internalSettings)
            {
                var type = internalSetting.GetType();
                if (_internalSettingsByTypes.ContainsKey(type))
                {
                    Debug.LogError($"[BaseCombatConfig] Setting with type: {type} already exist!");
                    continue;
                }

                _internalSettingsByTypes[type] = internalSetting;
            }
            
            base.Initialize();
        }
        
        public T GetInternalSetting<T>() where T : ICombatInternalSetting
        {
            var type = typeof(T);
            if(!_internalSettingsByTypes.TryGetValue(type, out var setting))
                return default;

            return (T) setting;
        }
    }
}