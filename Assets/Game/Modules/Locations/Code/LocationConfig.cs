using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Locations
{
    [CreateAssetMenu(
        menuName = "FlatLands/Locations/new Location Config", 
        fileName = nameof(LocationConfig))]
    public sealed class LocationConfig : SerializedScriptableObject
    {
        public string SceneName => name;
    }
}