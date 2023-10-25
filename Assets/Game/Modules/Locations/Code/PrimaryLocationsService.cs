using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.Locations
{
    public sealed class PrimaryLocationsService : PrimarySharedObject, IGeneralSceneLoader
    {
        [Inject] private LocationsManager _locationsManager;

        private GeneralLocationConfig _config;

        public override void Init()
        {
            _config = GeneralLocationConfig.Instance;
        }

#region IGeneralSceneLoader

        public bool NeedLoad => _config.LoadDefaultLocation;

        public int LoadingSceneOrder => 100;
        
        public string GetLoadingSceneName()
        {
            return _config.StartLocation.SceneName;
        }

        public void InvokeSceneLoaded()
        {
            var hierarchy = GameObject.FindObjectOfType<LocationHierarchy>();
            _locationsManager.InvokeSceneLoaded(_config.StartLocation.SceneName, hierarchy);
        }

#endregion

    }
}