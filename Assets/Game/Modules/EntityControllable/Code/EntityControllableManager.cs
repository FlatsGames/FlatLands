using System;
using FlatLands.Architecture;

namespace FlatLands.EntityControllable
{
    public sealed class EntityControllableManager : SharedObject
    {
        public IEntityControllableProvider CurrentControllableProvider { get; private set; }
        public IEntityControllableBehaviour CurrentControllableBehaviour { get; private set; }

        public event Action OnControllableEntityChanged;
        

        public void SetControllableEntity(IEntityControllableProvider provider, IEntityControllableBehaviour behaviour)
        {
            CurrentControllableProvider = provider;
            CurrentControllableBehaviour = behaviour;
            OnControllableEntityChanged?.Invoke();
        }
    }
}

