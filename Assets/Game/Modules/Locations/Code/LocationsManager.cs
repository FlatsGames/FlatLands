using FlatLands.Architecture;
using FlatLands.UI;

namespace FlatLands.Locations
{
    public sealed class LocationsManager : SharedObject, ISceneLoader
    {
        [Inject] private UIManager _uiManager;
        
#region ISceneLoader

        public int LoadingSceneOrder => 50;
        public string GetLoadingSceneName() => "TestLocation";
        
#endregion
    }
}
