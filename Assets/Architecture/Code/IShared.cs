namespace FlatLands.Architecture
{
    public interface IShared : ISharedInterface
    {
        void Init();

        void Dispose();
    }
}