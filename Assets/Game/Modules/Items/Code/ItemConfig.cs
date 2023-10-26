using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.Items
{
    [CreateAssetMenu(
        menuName = "FlatLands/Items/" + nameof(ItemConfig), 
        fileName = nameof(ItemConfig))]
    public sealed class ItemConfig : MultitonScriptableObjectsByName<ItemConfig>
    {
        [SerializeField] 
        private ItemView _itemPrefab;
        public ItemView ItemPrefab => _itemPrefab; 
    
        [SerializeField]
        private Sprite _icon;
        public Sprite Icon => _icon;

        [SerializeField] 
        private int _maxCount;
        public int MaxCount => _maxCount;

        [SerializeField, TextArea(5, 10)] 
        private string _description;
        public string Description => _description;
    }
}