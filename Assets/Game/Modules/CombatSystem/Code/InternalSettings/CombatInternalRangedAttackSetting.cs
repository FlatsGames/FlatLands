using UnityEngine;

namespace FlatLands.CombatSystem
{
    public sealed class CombatInternalRangedAttackSetting : ICombatInternalSetting
    {
        [SerializeField] private string _startAimingAnim;
        [SerializeField] private string _holdAimingAnim;
        [SerializeField] private string _endAimingAnim;

        public string StartAimingAnim => _startAimingAnim;
        public string HoldAimingAnim => _holdAimingAnim;
        public string EndAimingAnim => _endAimingAnim;
    }
}