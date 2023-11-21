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
        private string _animatorSubLayer;
        
        [SerializeField] 
        private List<TAnimEnum> _combatAnimations;
        
        public string AnimatorSubLayer => _animatorSubLayer;
        public IReadOnlyList<TAnimEnum> CombatAnimations => _combatAnimations;

        public abstract WeaponEquipmentSlotType Category { get; }
    }
}