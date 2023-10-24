using System.Collections.Generic;
using FlatLands.Architecture;
using FlatLands.UI;

namespace FlatLands.Locations
{
    public sealed class LocationsManager : SharedObject, IGeneralSceneLoader
    {
        [Inject] private UIManager _uiManager;

        private Dictionary<string, LocationHierarchy> _loadedLocations;

        private (string, LocationHierarchy) CurLocation;
        private GeneralLocationConfig _config;

        public override void PreInit()
        {
            _loadedLocations = new Dictionary<string, LocationHierarchy>();
            _config = GeneralLocationConfig.Instance;
        }

        public override void Init()
        {
            
        }

        public override void Dispose()
        {
            
        }

        public void LoadLocation(LocationConfig location, bool changeLocation)
        {
            
        }

#region IGeneralSceneLoader

        public bool NeedLoad => _config.LoadDefaultLocation;

        public int LoadingSceneOrder => 100;
        
        public string GetLoadingSceneName()
        {
            return _config.StartLocation.SceneName;
        }
        
#endregion

    }
}
