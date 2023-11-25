using DG.Tweening;
using FlatLands.Architecture;
using FlatLands.GameAttributes;
using FlatLands.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FlatLands.CharacterAttributes
{
    public sealed class UIStaminaHudElement : UIHudElement
    {
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private Slider _mainSlider;
        
        private AttributeRegeneratedData _attribute;
        private Sequence _sliderSequence;

        public void SetAttribute(AttributeRegeneratedData attributeData)
        {
            if (_attribute != null)
                _attribute.OnValueChangedDetails -= HandleValueChanged;
            
            _attribute = attributeData;
            _mainSlider.minValue = _attribute.MinValue;
            _mainSlider.maxValue = _attribute.MaxValue;
            _mainSlider.value = _attribute.Value;

            if (_attribute != null)
                _attribute.OnValueChangedDetails += HandleValueChanged;
        }
        
        private void HandleValueChanged(bool addValue, float prevValue, float newValue)
        {
            _sliderSequence?.Kill();
            _sliderSequence = DOTween.Sequence();
            
            _valueText.SetText($"{(int)_attribute.Value}/{(int)_attribute.MaxValue}");
            
            _sliderSequence.Append(_mainSlider.DoValue(newValue, 0.1f));
        }
    }
}