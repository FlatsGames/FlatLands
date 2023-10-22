namespace FlatLands.Architecture
{
    public interface ISceneLoader
    {
        public int LoadingSceneOrder { get; }
        public string GetLoadingSceneName();
    }
}

