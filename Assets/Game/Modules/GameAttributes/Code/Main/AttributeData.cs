namespace FlatLands.GameAttributes
{
    public sealed class AttributeData : BaseAttributeData
    {
        public AttributeData(
            GameAttributeType type, 
            float baseValue = default) 
            : base(type, baseValue) { }
    }
}