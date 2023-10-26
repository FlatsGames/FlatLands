using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Items
{
    public class ItemData : MonoBehaviour
    {
        public ItemConfig Config { get; }
        public int Count { get; set; }
        public ItemView ItemView { get; set; }

        public bool HaveView => ItemView != null;
    
        public ItemData (ItemConfig itemConfig, int startCount)
        {
            Config = itemConfig;
            Count = startCount;
        }
    }
    
}