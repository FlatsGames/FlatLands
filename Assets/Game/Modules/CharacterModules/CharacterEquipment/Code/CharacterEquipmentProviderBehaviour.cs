using FlatLands.Characters;
using FlatLands.Equipments;
using UnityEngine;

namespace FlatLands.CharacterEquipment
{
    public sealed class CharacterEquipmentProviderBehaviour : BaseEquipmentProviderBehaviour, ICharacterBehaviour
    {
        public Transform EntityTransform => transform;
    }
}