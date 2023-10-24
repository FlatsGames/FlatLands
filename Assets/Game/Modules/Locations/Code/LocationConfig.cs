using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.Locations
{
    [CreateAssetMenu(
        menuName = "FlatLands/Locations/new Location Config", 
        fileName = nameof(LocationConfig))]
    public sealed class LocationConfig : MultitonScriptableObjectsByName<LocationConfig>
    {
        public string SceneName => name;
    }
}