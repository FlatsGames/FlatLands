using System.Collections.Generic;

namespace FlatLands.GameAttributes
{
    public interface IGameAttributeHolder
    {
        public string HolderId { get; }
        public IReadOnlyDictionary<GameAttributeType, AttributeData> GetAttributes();
    }
}