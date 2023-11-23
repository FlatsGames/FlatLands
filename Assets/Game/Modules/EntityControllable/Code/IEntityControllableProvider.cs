using UnityEngine;

namespace FlatLands.EntityControllable
{
    public interface IEntityControllableProvider
    {
        public void EntityUpdate();

        public void EntityFixedUpdate();
    }
}