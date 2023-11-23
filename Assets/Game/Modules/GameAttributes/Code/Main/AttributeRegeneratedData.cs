using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.GameAttributes
{
    public sealed class AttributeRegeneratedData : AttributeRangedData
    {
        [SerializeField] private float _delayBeforeRegenerated;
        [SerializeField] private float _regeneratedValue;
        
        public float DelayBeforeRegenerate { get; private set; }
        public float RegeneratedValue { get; private set; }

        private float _currentDelay;
        
        public void SetRegeneratedDelay(float delay)
        {
            DelayBeforeRegenerate = delay;
        }
        
        public void SetRegeneratedByValue(float value)
        {
            RegeneratedValue = value;
        }
        
        public void SetRegeneratedByPercent(float percent)
        {
            RegeneratedValue = MaxValue * percent / 100;
        }

        protected override void InvokeUpdate()
        {
            if (!CanAddValue(RegeneratedValue))
                return;
            
            if (_currentDelay > DelayBeforeRegenerate)
            {
                _currentDelay = 0;
                AddValue(RegeneratedValue);
                return;
            }

            _currentDelay += UnityEventsProvider.DeltaTime;
        }
    }
}