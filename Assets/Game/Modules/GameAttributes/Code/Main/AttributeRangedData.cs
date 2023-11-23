using System;
using UnityEngine;

namespace FlatLands.GameAttributes
{
    public class AttributeRangedData : AttributeData
    {
        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue;

        public float MaxValue => _maxValue;
        public float MinValue => _minValue;

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
    }
}