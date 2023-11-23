using System;
using System.Collections.Generic;
using FlatLands.Architecture;
using FlatLands.EntityControllable;

namespace FlatLands.Characters
{
    public sealed class CharacterGroup : IEntityControllableProvider
    {
        [Inject] private Container _container;
        
        public CharacterBehaviour CharacterBehaviour { get; }

        private Dictionary<Type, ICharacterProvider> _providers;
        private Dictionary<Type, ICharacterBehaviour> _behaviours;

        public CharacterGroup(CharacterBehaviour behaviour)
        {
            CharacterBehaviour = behaviour;
            
            _providers = new Dictionary<Type, ICharacterProvider>();
            _behaviours = new Dictionary<Type, ICharacterBehaviour>();
        }

        public void Init()
        {
            foreach (var pair in _providers)
            {
                _container.InjectAt(pair.Value);
            }
            
            foreach (var pair in _providers)
            {
                pair.Value.Init();
            }
        }
        
        public void Dispose()
        {
            foreach (var pair in _providers)
            {
                pair.Value.Dispose();
            }
        }

        public void EntityUpdate()
        {
            foreach (var pair in _providers)
            {
                pair.Value.HandleUpdate();
            }
        }

        public void EntityFixedUpdate()
        {
            foreach (var pair in _providers)
            {
                pair.Value.HandleFixedUpdate();
            }
        }

        public void AddProvider(ICharacterProvider provider)
        {
            var type = provider.GetType();
            _providers[type] = provider;
        }
        
        public void AddBehaviour(ICharacterBehaviour behaviour)
        {
            var type = behaviour.GetType();
            _behaviours[type] = behaviour;
        }

        public T GetProvider<T>() where T : ICharacterProvider
        {
            var type = typeof(T);
            if (!_providers.TryGetValue(type, out var provider))
                return default;

            return (T) provider;
        }
        
        public T GetBehaviour<T>() where T : ICharacterBehaviour
        {
            var type = typeof(T);
            if (!_behaviours.TryGetValue(type, out var behaviour))
                return default;

            return (T) behaviour;
        }
    }
}