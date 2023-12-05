using System.Collections.Generic;

namespace FlatLands.GameAttributes
{
    public interface IGameAttributeHolder
    {
        public string HolderId { get; }
        public IReadOnlyDictionary<GameAttributeType, AttributeData> GetAttributes();

        public T GetAttribute<T>(GameAttributeType type) where T : AttributeData;
    }
}