using System;
using FlatLands.Architecture;

namespace FlatLands.EntityControllable
{
    public sealed class EntityControllableManager : SharedObject
    {
        public IEntityControllable CurrentControllableEntity { get; private set; }

        public event Action OnControllableEntityChanged;
        
        public override void Init()
        {
            UnityEventsProvider.OnUpdate += HandleUpdate;
        }

        public override void Dispose()
        {
            UnityEventsProvider.OnUpdate -= HandleUpdate;
        }

        private void HandleUpdate()
        {
            CurrentControllableEntity?.EntityUpdate();
        }

        public void SetControllableEntity(IEntityControllable entity)
        {
            CurrentControllableEntity = entity;
            OnControllableEntityChanged?.Invoke();
        }
    }
}

