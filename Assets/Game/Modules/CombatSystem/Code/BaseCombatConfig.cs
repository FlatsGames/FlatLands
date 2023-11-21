using System;
using System.Collections.Generic;
using FlatLands.Architecture;
using FlatLands.Equipments;
using UnityEngine;

namespace FlatLands.CombatSystem
{
    public abstract class BaseCombatConfig<TAnimEnum> : MultitonScriptableObjectsByName<BaseCombatConfig<TAnimEnum>>
        where TAnimEnum : Enum
    {
        [SerializeField] 
        private Dictionary<TAnimEnum, string> _combatAnimations;

        public IReadOnlyDictionary<TAnimEnum, string> CombatAnimations => _combatAnimations;
        
        public abstract WeaponEquipmentSlotType Category { get; }
    }
}