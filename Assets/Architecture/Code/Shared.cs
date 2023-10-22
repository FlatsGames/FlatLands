namespace FlatLands.Architecture
{
    public abstract class SharedObject : IShared
    {
        [Inject] protected Container _container;

        public virtual void Init() { }

        public virtual void Dispose() { }
    }
}