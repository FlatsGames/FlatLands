using FlatLands.Characters;
using FlatLands.CombatSystem;
using UnityEngine;

namespace FlatLands.CharacterCombat
{
    public sealed class CharacterCombatBehaviour : BaseCombatBehaviour, ICharacterBehaviour
    {
        public Transform EntityTransform => transform;
    }
}