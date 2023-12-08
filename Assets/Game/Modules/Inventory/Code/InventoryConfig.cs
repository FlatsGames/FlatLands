using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.Inventory
{
    [CreateAssetMenu(
        menuName = "FlatLands/Inventory/" + nameof(InventoryConfig), 
        fileName = nameof(InventoryConfig))]
    public class InventoryConfig : SingletonScriptableObject<InventoryConfig>
    {
        [SerializeField]
        private int _slotsCount;

        public int SlotsCont => _slotsCount;
    }
}