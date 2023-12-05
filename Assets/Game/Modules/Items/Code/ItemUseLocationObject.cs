using System;
using FlatLands.LocationsObjects;

namespace FlatLands.Items
{
    public class ItemUseLocationObject : BaseUseLocationObject
    {
        public override Type UseType => typeof(ItemUseLocationObject);
    }
}