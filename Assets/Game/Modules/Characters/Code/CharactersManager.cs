using System;
using System.Collections.Generic;
using FlatLands.Architecture;
using FlatLands.EntityControllable;
using UnityEngine;

namespace FlatLands.Characters
{
    public sealed class CharactersManager : SharedObject
    {
        [Inject] private EntityControllableManager _controllableManager;
        
        private List<CharacterBehaviour> _allCharacters;

        public CharacterBehaviour CurrentCharacter { get; private set; }

        public event Action<CharacterBehaviour> OnCharacterCreated;
        
        private GeneralCharacterConfig _config;
        
        public override void Init()
        {
            _allCharacters = new List<CharacterBehaviour>();
            _config = GeneralCharacterConfig.Instance;
            
            CreateCharacter(_config.DefaultCharacterPrefab);
            _controllableManager.SetControllableEntity(CurrentCharacter);
        }

        public override void Dispose()
        {
            
        }

        private void CreateCharacter(CharacterBehaviour prefab, Transform parent = null, bool isMain = true)
        {
            var character = GameObject.Instantiate(prefab, parent);
            _allCharacters.Add(character);
            if (isMain)
                CurrentCharacter = character;
            
            OnCharacterCreated?.Invoke(character);
        }
    }
}