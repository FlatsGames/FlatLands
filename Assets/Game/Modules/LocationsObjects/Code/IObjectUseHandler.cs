using System;

namespace FlatLands.LocationsObjects
{
    public interface IObjectUseHandler
    {
        public Type UseType { get; }

        public void ObjetUse(ILocationObject locationObject);
    }
}