using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.Characters
{
    [CreateAssetMenu(
        menuName = "FlatLands/Characters/" + nameof(GeneralCharacterConfig), 
        fileName = nameof(GeneralCharacterConfig))]
    public sealed class GeneralCharacterConfig : SingletonScriptableObject<GeneralCharacterConfig>
    {
        [SerializeField] 
        private CharacterBehaviour _defaultCharacterPrefab;
        
        public CharacterBehaviour DefaultCharacterPrefab => _defaultCharacterPrefab;
    }
}