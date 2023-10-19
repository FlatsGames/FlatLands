namespace FlatLands.Architecture
{
    public static class GlobalContainer
    {
        private static Container MainContainer;
        
        static internal void SetContainer(Container container)
        {
            MainContainer = container;
        }

        public static void InjectAt(object target)
        {
            MainContainer.InjectAt(target);
        }

        public static Container GetContainer()
        {
            return MainContainer;
        }
    }
}