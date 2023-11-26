using System.Collections.Generic;
using FlatLands.Conditions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.CombatSystem
{
    public sealed class CombatInternalRangedAttackSetting : ICombatInternalSetting
    {
        [SerializeField, BoxGroup("Character Settings")] 
        private string _startAimingAnim;
        
        [SerializeField, BoxGroup("Character Settings")] 
        private string _endAimingAnim;
        
        [SerializeField, BoxGroup("Character Settings")] 
        private string _shootAnim;
        
        [SerializeField, BoxGroup("Weapon Settings")] 
        private string _startWeaponAimingAnim;
        
        [SerializeField, BoxGroup("Weapon Settings")] 
        private string _endWeaponAimingAnim;

        [SerializeField, BoxGroup("Weapon Settings")] 
        private string _shootWeaponAnim;

        [SerializeField, BoxGroup("Projectile Settings")]
        private WeaponProjectile _projectilePrefab;
        
        [SerializeField, BoxGroup("Projectile Settings")]
        private float _projectileForce;

        [SerializeField] 
        private List<BaseCondition> _startConditions = new List<BaseCondition>();
        
        [SerializeField] 
        private List<BaseCondition> _endConditions = new List<BaseCondition>();

        public string StartAimingAnim => _startAimingAnim;
        public string EndAimingAnim => _endAimingAnim;
        public string ShootAnim => _shootAnim;

        public string WeaponStartAimingAnim => _startWeaponAimingAnim;
        public string WeaponEndAimingAnim => _endWeaponAimingAnim;
        public string WeaponShootAnim => _shootWeaponAnim;

        public WeaponProjectile ProjectilePrefab => _projectilePrefab;
        public float ProjectileForce => _projectileForce;
        
        public IReadOnlyList<BaseCondition> StartConditions => _startConditions;
        public IReadOnlyList<BaseCondition> EndConditions => _endConditions;
    }
}