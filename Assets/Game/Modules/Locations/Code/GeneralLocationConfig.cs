using System.Collections.Generic;
using System.Linq;
using FlatLands.Architecture;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Locations
{
    [CreateAssetMenu(
        menuName = "FlatLands/Locations/Main Config", 
        fileName = nameof(GeneralLocationConfig))]
    public sealed class GeneralLocationConfig : SingletonScriptableObject<GeneralLocationConfig>
    {
        [SerializeField] 
        private LocationConfig _startLocation;
        
        [SerializeField] 
        private List<LocationConfig> _locations;

        [Button]
        private void FindLocations()
        {
            _locations = new List<LocationConfig>();
            _locations = Resources.LoadAll<LocationConfig>("").ToList();
        }
    }
}