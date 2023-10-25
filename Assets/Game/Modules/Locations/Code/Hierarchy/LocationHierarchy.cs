using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Locations
{
    public sealed class LocationHierarchy : SerializedMonoBehaviour
    {
        [SerializeField]
        private List<ILocationData> _locationDatas = new ();

        public T GetData<T>() where T : ILocationData
        {
            var type = typeof(T);
            foreach (var data in _locationDatas)
            {
                if (data.GetType() == type)
                    return (T) data;
            }

            return default;
        }
    }
}