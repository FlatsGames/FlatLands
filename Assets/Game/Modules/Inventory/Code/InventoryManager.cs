using System;
using FlatLands.Architecture;
using FlatLands.LocationsObjects;

namespace FlatLands.Inventory
{
    public class InventoryManager : SharedObject, IObjectUseHandler
    {
        public Type UseType => typeof(ItemUseLocationObject);
        public void ObjetUse(ILocationObject locationObject)
        {
            
        }
    }
}