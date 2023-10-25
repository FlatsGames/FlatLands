using UnityEngine;

namespace FlatLands.EntityControllable
{
    public interface IEntityControllable
    {
        public string Name { get; }

        public Transform EntityTransform { get; }
        
        public void EntityUpdate();
    }
}