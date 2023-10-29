using UnityEngine;

namespace FlatLands.EntityControllable
{
    public interface IEntityControllableProvider
    {
        public void EntityUpdate();

        public void EntityFixedUpdate();
    }

    public interface IEntityControllableBehaviour
    {
        public string Name { get; }

        public Transform EntityTransform { get; }
    }
}