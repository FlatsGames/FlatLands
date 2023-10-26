using System.Collections.Generic;
using FlatLands.Architecture;
using FlatLands.UI;

namespace FlatLands.Locations
{
    public sealed class LocationsManager : SharedObject
    {
        [Inject] private UIManager _uiManager;

        private Dictionary<string, LocationHierarchy> _loadedLocations;

        public (string, LocationHierarchy) CurLocation { get; private set; }
        private GeneralLocationConfig _config;

        internal void InvokeSceneLoaded(string sceneName, LocationHierarchy hierarchy)
        {
            CurLocation = (sceneName, hierarchy);
        }
        
        public override void Init()
        {
            
        }

        public override void Dispose()
        {
            
        }
    }
}
