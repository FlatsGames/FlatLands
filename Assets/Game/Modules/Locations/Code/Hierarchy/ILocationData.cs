using UnityEngine;

namespace FlatLands.Locations
{
    public interface ILocationData
    {
        void Refresh(GameObject hierarchyObject);
    }
}