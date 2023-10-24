namespace FlatLands.Architecture
{
    public interface IShared : ISharedInterface
    {
        internal void SetContainer(Container container);
        
        void Init();

        void Dispose();
    }
}