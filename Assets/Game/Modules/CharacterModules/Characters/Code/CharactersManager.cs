using System;
using FlatLands.Architecture;
using FlatLands.EntityControllable;
using Object = UnityEngine.Object;

namespace FlatLands.Characters
{
    public sealed class CharactersManager : SharedObject
    {
        [Inject] private EntityControllableManager _controllableManager;
        
        public CharacterGroup CurrentCharacter { get; private set; }
        public event Action OnCharacterCreated;

        private GeneralCharacterConfig _config;
        
        public override void Init()
        {
            _config = GeneralCharacterConfig.Instance;
        }

        public override void Dispose()
        {
            
        }

        public void CreateDefaultCharacter()
        {
            var prefab = _config.DefaultCharacterPrefab;
            var behaviour = Object.Instantiate(prefab);
            CurrentCharacter = new CharacterGroup(behaviour);
            _container.InjectAt(CurrentCharacter);
            OnCharacterCreated?.Invoke();
        }
    }
}