using System.Collections.Generic;
using FlatLands.Conditions;
using UnityEngine;

namespace FlatLands.CombatSystem
{
    public sealed class CombatInternalRangedAttackSetting : ICombatInternalSetting
    {
        [SerializeField] 
        private string _startAimingAnim;
        
        [SerializeField] 
        private string _endAimingAnim;
        
        [SerializeField] 
        private string _shootAnim;

        [SerializeField] 
        private List<BaseCondition> _startConditions = new List<BaseCondition>();
        
        [SerializeField] 
        private List<BaseCondition> _endConditions = new List<BaseCondition>();

        public string StartAimingAnim => _startAimingAnim;
        public string EndAimingAnim => _endAimingAnim;
        public string ShootAnim => _shootAnim;

        public IReadOnlyList<BaseCondition> StartConditions => _startConditions;
        public IReadOnlyList<BaseCondition> EndConditions => _endConditions;
    }
}