using FlatLands.Characters;
using FlatLands.Equipments;
using UnityEngine;

namespace FlatLands.CharacterEquipment
{
    public sealed class CharacterEquipmentBehaviour : BaseEquipmentBehaviour, ICharacterBehaviour
    {
        public Transform EntityTransform => transform;
    }
}