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
        [SerializeField] 
        private string _animatorSubLayer;

        [SerializeField] 
        private bool _hasBlockCooldown;

        [SerializeField, ShowIf(nameof(_hasBlockCooldown))] 
        private float _blockCooldownSeconds;
        
        [SerializeField]
        private string _blockStartAnimation;
        
        [SerializeField]
        private string _blockIdleAnimation;
        
        [SerializeField] 
        private List<string> _combatAnimations;

        public string AnimatorSubLayer => _animatorSubLayer;

        public bool HasBlockCooldown => _hasBlockCooldown;
        public float BlockCooldownSeconds => _blockCooldownSeconds;
        public string BlockStartAnimation => _blockStartAnimation;
        public string BlockIdleAnimation => _blockIdleAnimation;

        public IReadOnlyList<string> CombatAnimations => _combatAnimations;
        
        public abstract WeaponEquipmentSlotType Category { get; }
    }
}