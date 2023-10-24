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
        private bool _loadDefaultLocation = true;
        
        [SerializeField, ShowIf("@_loadDefaultLocation")] 
        private LocationConfig _startLocation;

        public bool LoadDefaultLocation => _loadDefaultLocation;
        public LocationConfig StartLocation => _startLocation;
    }
}