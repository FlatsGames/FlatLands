using UnityEngine;

namespace FlatLands.CombatSystem
{
    public sealed class CombatInternalRangedAttackSetting : ICombatInternalSetting
    {
        [SerializeField] private string _startAimingAnim;
        [SerializeField] private string _endAimingAnim;
        [SerializeField] private string _shootAnim;

        public string StartAimingAnim => _startAimingAnim;
        public string EndAimingAnim => _endAimingAnim;
        public string ShootAnim => _shootAnim;
    }
}