using FlatLands.Architecture;

namespace FlatLands.UI
{
    public sealed class PrimaryMainMenuService : SharedObject, IGeneralSceneLoader
    {
        [Inject] private MainMenuManager _mainMenuManager;
        
#region IGeneralSceneLoader

        public bool NeedLoad => true;
        public int LoadingSceneOrder => 10;
        public string GetLoadingSceneName() => "Menu";
        public void InvokeSceneLoaded()
        {
            _mainMenuManager.InvokeMainMenuLoaded();
        }

#endregion

    }
}