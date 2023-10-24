namespace FlatLands.Architecture
{
    public interface IShared : ISharedInterface
    {
        internal void SetContainer(Container container);

        void PreInit();
        
        void Init();

        void Dispose();
    }
}