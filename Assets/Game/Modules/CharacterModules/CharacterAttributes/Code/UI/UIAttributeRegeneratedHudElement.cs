using DG.Tweening;
using FlatLands.GameAttributes;
using FlatLands.UI;
using UnityEngine;
using UnityEngine.UI;

namespace FlatLands.CharacterAttributes
{
    public sealed class UIAttributeRegeneratedHudElement : UIHudElement
    {
        [SerializeField] private Slider _mainSlider;
        [SerializeField] private Slider _internalSlider;
        
        private AttributeRegeneratedData _attribute;
        private Sequence _sliderSequence;

        public override void Init()
        {
            
        }

        public override void Dispose()
        {
            if (_attribute != null)
                _attribute.OnValueChangedDetails -= HandleValueChanged;
        }

        public void SetAttribute(AttributeRegeneratedData attributeData)
        {
            if (_attribute != null)
                _attribute.OnValueChangedDetails -= HandleValueChanged;
            
            _attribute = attributeData;
            
            if (_attribute != null)
                _attribute.OnValueChangedDetails += HandleValueChanged;
        }

        private void HandleValueChanged(bool addValue, float prevValue, float newValue)
        {
            
        }
    }
}