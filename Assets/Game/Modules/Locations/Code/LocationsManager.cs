using FlatLands.Architecture;
using FlatLands.UI;

namespace FlatLands.Locations
{
    public sealed class LocationsManager : SharedObject
    {
        [Inject] private UIManager _uiManager;

        private GeneralLocationConfig _config;

        public override void Init()
        {
            _config = GeneralLocationConfig.Instance;
        }

        public override void Dispose()
        {
            
        }
    }
}
