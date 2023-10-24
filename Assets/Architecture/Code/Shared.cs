namespace FlatLands.Architecture
{
    public abstract class SharedObject : IShared
    {
        protected Container _container { get; private set; }

        void IShared.SetContainer(Container container)
        {
            _container = container;
        }
        
        public virtual void PreInit() { }

        public virtual void Init() { }

        public virtual void Dispose() { }
    }
}