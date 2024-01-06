using FlatLands.LocationsObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Items
{
    public class ItemView : MonoBehaviour, ILocationObject
    {
        [SerializeField]
        private ItemConfig _itemConfig;
        public ItemConfig Config => _itemConfig;
        
        [SerializeField]
        private int _count = 1;
        public int Count => _count;

        public string Id { get; }
        public GameObject LocationObject => gameObject;
    }
}