using FlatLands.EntityControllable;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Characters
{
    public sealed class CharacterBehaviour : SerializedMonoBehaviour, IEntityControllable
    {
        
        
#region IEntityControllable

        public string Name => gameObject.name;

        public Transform EntityTransform => transform;

        public void EntityUpdate()
        {
            
        }

#endregion

    }
}

