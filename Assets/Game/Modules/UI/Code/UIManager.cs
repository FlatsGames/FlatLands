using FlatLands.Architecture;

namespace FlatLands.UI
{
    public sealed class UIManager : SharedObject, IGeneralSceneLoader
    {
        public override void Init()
        {
            
        }

        public override void Dispose()
        {
           
        }

#region ISceneLoader

        public bool NeedLoad => true;
        public int LoadingSceneOrder => 20;
        public string GetLoadingSceneName() => "UIScene";
        
#endregion

    }
}

