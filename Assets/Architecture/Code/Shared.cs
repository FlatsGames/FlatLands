namespace FlatLands.Architecture
{
    public abstract class SharedObject : IShared
    {
        [Inject] public Container container;

        public virtual void Init()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}