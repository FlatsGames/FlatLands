using System.Collections.Generic;
using FlatLands.Architecture;
using FlatLands.UI;

namespace FlatLands.Locations
{
    public sealed class LocationsManager : SharedObject
    {
        [Inject] private UIManager _uiManager;

        private Dictionary<string, LocationHierarchy> _loadedLocations;

        private (string, LocationHierarchy) CurLocation;
        private GeneralLocationConfig _config;

        public override void Init()
        {
            
        }

        public override void Dispose()
        {
            
        }
        
        internal void InvokeSceneLoaded(string sceneName, LocationHierarchy hierarchy)
        {
            CurLocation = (sceneName, hierarchy);
        }
    }
}
