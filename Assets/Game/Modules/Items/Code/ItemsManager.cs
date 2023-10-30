using System.Collections;
using System.Collections.Generic;
using FlatLands.Architecture;
using FlatLands.Locations;
using FlatLands.LocationsCamera;
using UnityEngine;

namespace FlatLands.Items
{
    public class ItemsManager : SharedObject
    {
        [Inject] private LocationsManager _locationsManager;
        [Inject] private LocationsCameraManager _locationsCameraManager;
        
        private Dictionary<string, ItemData> _itemsPair;
        private int _viewsCounter;
        private Transform _dropContainer;
        
        public List<ItemData> ItemsData { get; private set; }
        

        public override void Init()
        {
            // _locationsCameraManager.OnLocationObjectHit += 
            ItemsData = new List<ItemData>();
            _itemsPair = new Dictionary<string, ItemData>();
            ItemConfig.Init();
            var configs = ItemConfig.ByName;
            var itemsData = _locationsManager.CurLocation.Item2.GetData<ItemsLocationData>();
            _dropContainer = itemsData.DropContainer;
        }

        public ItemData CreatItemData(ItemConfig config, int startCount)
        {
            var newData = new ItemData(config, startCount);
            ItemsData.Add(newData);
            return newData;
        }

        public void DestroyItemData(ItemData itemData)
        {
            ItemsData.Remove(itemData);
        }

        public ItemView CreatItemView(ItemData itemData, Vector3 position)
        {
            var prefab = itemData.Config.ItemPrefab;
            var itemView = GameObject.Instantiate(prefab,position, Quaternion.identity);
            itemData.ItemView = itemView;
            itemView.SetId(_viewsCounter);
            _itemsPair.TryAdd(itemView.Id, itemData);
            _viewsCounter++;
            return itemView;
        }

        public void DestroyItemView(ItemData itemData)
        {
            if (!itemData.HaveView)
                return;
            GameObject.Destroy(itemData.ItemView.gameObject);
            itemData.ItemView = null;
        }
        
        
        
    }
}
