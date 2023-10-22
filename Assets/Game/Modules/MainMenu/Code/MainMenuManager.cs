using FlatLands.Architecture;

namespace FlatLands.UI
{
    public sealed class MainMenuManager : SharedObject, ISceneLoader
    {
       
        
#region ISceneLoader

        public int LoadingSceneOrder => 10;
        public string GetLoadingSceneName() => "MainMenu";
        
#endregion
    }
}