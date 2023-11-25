using FlatLands.Architecture;
using UnityEngine;

namespace FlatLands.GameAttributes
{
    public sealed class AttributeRegeneratedData : AttributeRangedData
    {
        [SerializeField] private float _delayBeforeRegenerated;
        [SerializeField] private float _regeneratedValue;

        public bool CanRegenerated { get; private set; } = true;
        public float DelayBeforeRegenerate { get; private set; }
        public float RegeneratedValue { get; private set; }

        private float _currentDelay;

        public void SetCanRegenerated(bool canRegenerated)
        {
            CanRegenerated = canRegenerated;
        }
        
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

        internal override void Init()
        {
            DelayBeforeRegenerate = _delayBeforeRegenerated;
            RegeneratedValue = _regeneratedValue;
            
            base.Init();
        }

        protected override void InvokeUpdate()
        {
            if(!CanRegenerated)
                return;
            
            if (MaxValue <= Value)
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