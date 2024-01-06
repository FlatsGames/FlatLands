using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlatLands.Locations;
using UnityEngine;

namespace FlatLands.Items
{
    public class ItemsLocationData : ILocationData
    {
        [SerializeField] 
        private Transform _dropContainer;
        
        [SerializeField]
        private List<ItemView> _itemsViewsOnScene;
        public List<ItemView> itemsViewsOnScene => _itemsViewsOnScene;
        public Transform DropContainer => _dropContainer;
        
        public void Refresh(GameObject hierarchyObject)
        {
            _itemsViewsOnScene = new List<ItemView>();
            _itemsViewsOnScene = hierarchyObject.GetComponentsInChildren<ItemView>().ToList();
        }

    }
}
