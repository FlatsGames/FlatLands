namespace FlatLands.Architecture
{
    public interface IGeneralSceneLoader
    {
        public bool NeedLoad { get; }
        public int LoadingSceneOrder { get; }
        public string GetLoadingSceneName();
        public void InvokeSceneLoaded();

    }
}

