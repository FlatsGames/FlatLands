using System.Collections;
using System.Collections.Generic;
using FlatLands.Architecture;
using FlatLands.Locations;
using UnityEngine;

namespace FlatLands.Items
{
    public class ItemsManager : SharedObject
    {
        [Inject] private LocationsManager _locationsManager;

        private Transform _dropContainer;
        
        public override void Init()
        {
            ItemConfig.Init();
            var configs = ItemConfig.ByName;
            var itemsData = _locationsManager.CurLocation.Item2.GetData<ItemsLocationData>();
            _dropContainer = itemsData.DropContainer;
        }
        
        
    }
}
