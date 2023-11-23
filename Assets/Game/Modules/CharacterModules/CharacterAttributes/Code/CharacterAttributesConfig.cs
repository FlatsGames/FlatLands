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
    public sealed class CharacterAttributesConfig : SingletonScriptableObject<CharacterAttributesConfig>, IGameAttributeHolder
    {
        [SerializeField] private Dictionary<GameAttributeType, AttributeData> _attributes;
        public string HolderId => nameof(CharacterAttributesConfig);

        public IReadOnlyDictionary<GameAttributeType, AttributeData> GetAttributes() => _attributes;
    }
}