using System.Collections.Generic;

namespace FlatLands.GameAttributes
{
    public interface IGameAttributeHolder
    {
        public IReadOnlyDictionary<GameAttributeType, BaseAttributeData> GetAttributes();
    }
}