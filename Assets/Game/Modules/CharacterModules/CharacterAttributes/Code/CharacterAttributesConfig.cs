using System.Collections.Generic;
using System.Linq;
using FlatLands.Architecture;
using FlatLands.GameAttributes;
using UnityEngine;

namespace FlatLands.CharacterAttributes
{
    [CreateAssetMenu(
        menuName = "FlatLands/Characters/" + nameof(CharacterAttributesConfig),
        fileName = nameof(CharacterAttributesConfig))]
    public sealed class CharacterAttributesConfig : SingletonScriptableObject<CharacterAttributesConfig>
    {
        [SerializeField] private Dictionary<GameAttributeType, AttributeData> _attributes;

        public IReadOnlyDictionary<GameAttributeType, AttributeData> Attributes => _attributes;
    }
}