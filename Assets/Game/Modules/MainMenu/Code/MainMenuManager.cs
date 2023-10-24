using FlatLands.Architecture;

namespace FlatLands.UI
{
    public sealed class MainMenuManager : SharedObject, IGeneralSceneLoader
    {
       
        
#region ISceneLoader

        public bool NeedLoad => true;
        public int LoadingSceneOrder => 10;
        public string GetLoadingSceneName() => "Menu";
        public void InvokeSceneLoaded()
        {
            
        }

    #endregion
    }
}