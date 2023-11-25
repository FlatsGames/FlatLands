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
        
        [SerializeField, FoldoutGroup("Block Settings")] 
        private bool _hasBlockCooldown;

        [SerializeField, ShowIf(nameof(_hasBlockCooldown)), FoldoutGroup("Block Settings")] 
        private float _blockCooldownSeconds;
        
        [SerializeField, FoldoutGroup("Block Settings")]
        private string _blockStartAnimation;

        [SerializeField, FoldoutGroup("Combat Settings")]
        private float _combatCost;
        
        [SerializeField, FoldoutGroup("Combat Settings")] 
        private List<string> _combatAnimations;

        public string AnimatorSubLayer => _animatorSubLayer;
        public float CombatCost => _combatCost;
        public bool HasBlockCooldown => _hasBlockCooldown;
        public float BlockCooldownSeconds => _blockCooldownSeconds;
        public string BlockStartAnimation => _blockStartAnimation;

        public IReadOnlyList<string> CombatAnimations => _combatAnimations;
        
        public abstract WeaponEquipmentSlotType Category { get; }
    }
}