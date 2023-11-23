namespace FlatLands.GameAttributes
{
    public sealed class AttributeRangedData : BaseRangedAttributeData
    {
        public AttributeRangedData(
            GameAttributeType type, 
            float minValue, 
            float maxValue, 
            float baseValue = default) 
            : base(type, minValue, maxValue, baseValue) { }
    }
}