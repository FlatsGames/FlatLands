using System;
using FlatLands.Architecture;
using FlatLands.LocationsObjects;
using UnityEngine;

namespace FlatLands.Inventory
{
    public class InventoryManager : SharedObject, IObjectUseHandler
    {

        [Inject] private InventoryModel _inventoryModel;
        private InventoryConfig _config;
        

        public override void Init()
        {
            _config = InventoryConfig.Instance;
            AddDefaultSlots();
        }

        public override void Dispose()
        {
            
        }

        private void AddDefaultSlots()
        {
            _inventoryModel.AddSlots(_config.SlotsCont);
            
        }

        #region IObjectUseHandler
        
        public Type UseType => typeof(ItemUseLocationObject);
        public void ObjetUse(ILocationObject locationObject)
        {
            
        }
        
        #endregion
       
        
    }
}