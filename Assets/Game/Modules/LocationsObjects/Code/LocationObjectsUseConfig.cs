using System.Collections.Generic;
using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.LocationsObjects
{
    [CreateAssetMenu(
        menuName = "FlatLands/UseConfig/" + nameof(LocationObjectsUseConfig), 
        fileName = nameof(LocationObjectsUseConfig))]
    public class LocationObjectsUseConfig : SingletonScriptableObject<LocationObjectsUseConfig>
    {
        [SerializeField] 
        private List<BaseUseLocationObject> _useVariants;
        public IReadOnlyList<BaseUseLocationObject> UseVariants => _useVariants;

    }
}