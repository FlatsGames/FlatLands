using System.Collections;
using System.Collections.Generic;
using FlatLands.Locations;
using UnityEngine;

namespace FlatLands.Items
{
    public class ItemsLocationData : ILocationData
    {
        [SerializeField] 
        private Transform _dropContainer;
        public Transform DropContainer => _dropContainer;
        
        public void Refresh(GameObject hierarchyObject)
        {
            
        }
    }
}
