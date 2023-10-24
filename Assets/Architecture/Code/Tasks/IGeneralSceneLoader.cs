namespace FlatLands.Architecture
{
    public interface IGeneralSceneLoader
    {
        public int LoadingSceneOrder { get; }
        public string GetLoadingSceneName();
    }
}

