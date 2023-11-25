using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.CombatSystem
{
    public sealed class CombatInternalMeleeAttackSetting : ICombatInternalSetting
    {
        [SerializeField, FoldoutGroup("Block Settings")] 
        private bool _hasBlockCooldown;

        [SerializeField, ShowIf(nameof(_hasBlockCooldown)), FoldoutGroup("Block Settings")] 
        private float _blockCooldownSeconds;
        
        [SerializeField, FoldoutGroup("Block Settings")]
        private string _blockStartAnimation;

        [SerializeField, FoldoutGroup("Combat Settings")]
        private float _combatCost;
        
        [SerializeField, FoldoutGroup("Combat Settings")] 
        private List<string> _attackAnimations;

        public float AttackCost => _combatCost;
        public bool HasBlockCooldown => _hasBlockCooldown;
        public float BlockCooldownSeconds => _blockCooldownSeconds;
        public string BlockStartAnimation => _blockStartAnimation;

        public IReadOnlyList<string> AttackAnimations => _attackAnimations;
    }
}