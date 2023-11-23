using System;

namespace FlatLands.GameAttributes
{
    public abstract class BaseRangedAttributeData : BaseAttributeData
    {
        public float MaxValue { get; }
        public float MinValue { get; }

        public bool CanAddValue(float amount)
        {
            if (Value + amount > MaxValue)
                return false;
			
            return true;
        }
		
        public bool CanRemoveValue(float amount)
        {
            if (Value - amount < MinValue)
                return false;

            return true;
        }

        protected BaseRangedAttributeData(GameAttributeType type, float minValue, float maxValue, float baseValue = default) : base(type, baseValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}