using System;
using FlatLands.LocationsObjects;

namespace FlatLands.Inventory
{
    public class ItemUseLocationObject : BaseUseLocationObject
    {
        public override Type UseType => typeof(ItemUseLocationObject);
    }
}