using FlatLands.Architecture;

namespace FlatLands.UI
{
    public sealed class UIManager : SharedObject, ISceneLoader
    {
        public override void Init()
        {
            
        }

        public override void Dispose()
        {
           
        }

#region ISceneLoader

        public int LoadingSceneOrder => 20;
        public string GetLoadingSceneName() => "UIScene";
        
#endregion

    }
}

