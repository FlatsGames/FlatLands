using System;
using UnityEngine;

namespace FlatLands.GameAttributes
{
    public class AttributeRangedData : AttributeData
    {
        [SerializeField] private bool _applyDefaultValue;
        
        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue;

        public float MaxValue => _maxValue;
        public float MinValue => _minValue;

        internal override void Init()
        {
            if(_applyDefaultValue)
                Value = MaxValue;
            
            base.Init();
        }

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

        public override void AddValue(float amount)
        {
            var prevValue = Value;
            if (Value + amount > MaxValue)
            {
                Value = MaxValue;
            }
            else
            {
                Value += amount;
            }
            
            InvokeValueChanged(true, prevValue);
        }

        public override void RemoveValue(float amount)
        {
            var prevValue = Value;
            if (Value - amount < MinValue)
            {
                Value = MinValue;
            }
            else
            {
                Value -= amount;
            }
            
            InvokeValueChanged(false, prevValue);
        }
    }
}