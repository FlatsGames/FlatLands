namespace FlatLands.Characters
{
    public interface ICharacterProvider
    {
        public void Init();
        public void Dispose();
        public void HandleUpdate();
        public void HandleFixedUpdate();
    }
}