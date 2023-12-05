using UnityEngine;

namespace FlatLands.EntityControllable
{
    public interface IEntityControllableBehaviour
    {
        public string Name { get; }

        public Transform EntityTransform { get; }
    }
}