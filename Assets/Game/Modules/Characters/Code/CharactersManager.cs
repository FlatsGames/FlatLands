using System;
using FlatLands.Architecture;
using FlatLands.EntityControllable;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FlatLands.Characters
{
    public sealed class CharactersManager : SharedObject
    {
        [Inject] private EntityControllableManager _controllableManager;
        
        public CharacterProvider CurrentCharacter { get; private set; }

        public event Action<CharacterProvider> OnCharacterCreated;
        
        private CharacterConfig _config;
        
        public override void Init()
        {
            _config = CharacterConfig.Instance;
            
            CreateDefaultCharacter(_config);
            _controllableManager.SetControllableEntity(CurrentCharacter, CurrentCharacter.Behaviour);
        }

        public override void Dispose()
        {
            
        }

        private void CreateDefaultCharacter(CharacterConfig config,  bool isMain = true)
        {
            var provider = CreateCharacterProvider(config);
            if (isMain)
            {
                CurrentCharacter = provider;
            }
            
            OnCharacterCreated?.Invoke(provider);
        }

        private CharacterProvider CreateCharacterProvider(CharacterConfig config, bool spawnCharacter = true)
        {
            var character = new CharacterProvider(config);
            if (spawnCharacter)
            {
                var behaviour = SpawnCharacterBehaviour(config);
                character.SetBehaviour(behaviour);
            }

            return character;
        }

        private CharacterBehaviour SpawnCharacterBehaviour(CharacterConfig config, Transform parent = null)
        {
            return Object.Instantiate(config.CharacterPrefab, parent);
        }
    }
}