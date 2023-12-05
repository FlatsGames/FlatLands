using DG.Tweening;
using FlatLands.Architecture;
using FlatLands.GameAttributes;
using FlatLands.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FlatLands.CharacterAttributes
{
    public sealed class UIHealthHudElement : UIHudElement
    {
        [SerializeField] private Slider _mainSlider;
        [SerializeField] private Slider _internalSlider;
        [SerializeField] private TMP_Text _valueText;
        
        private AttributeRangedData _attribute;
        private Sequence _sliderSequence;

        private float _lastPrevValue;
        private bool _isActive;
        
        public override void Init()
        {
            
        }

        public override void Dispose()
        {
            if (_attribute != null)
                _attribute.OnValueChangedDetails -= HandleValueChanged;
        }

        public void SetAttribute(AttributeData attributeData)
        {
            var at = attributeData as AttributeRangedData;
            
            if (_attribute != null)
                _attribute.OnValueChangedDetails -= HandleValueChanged;
            
            _attribute = at;
            _mainSlider.minValue = _attribute.MinValue;
            _mainSlider.maxValue = _attribute.MaxValue;
            _mainSlider.value = _attribute.Value;
            
            _internalSlider.minValue = _attribute.MinValue;
            _internalSlider.maxValue = _attribute.MaxValue;
            _internalSlider.value = _attribute.Value;
            
            _valueText.SetText($"{_attribute.Value}/{_attribute.MaxValue}");
            
            if (_attribute != null)
                _attribute.OnValueChangedDetails += HandleValueChanged;
        }

        private void HandleValueChanged(bool addValue, float prevValue, float newValue)
        {
            _sliderSequence?.Kill();
            _sliderSequence = DOTween.Sequence();
            
            _valueText.SetText($"{(int)_attribute.Value}/{(int)_attribute.MaxValue}");
            if (addValue)
            {
                _internalSlider.value = 0;
                _sliderSequence.Append(_mainSlider.DoValue(newValue, 0.3f));
                _sliderSequence.AppendCallback(() => _internalSlider.value = 0);
                return;
            }
            
            if(!_isActive)
                _lastPrevValue = prevValue;
            
            _internalSlider.value = _lastPrevValue;
            _mainSlider.value = newValue;
            _isActive = true;
            _sliderSequence.AppendInterval(0.3f);
            _sliderSequence.Append(_internalSlider.DoValue(newValue, 0.3f));
            _sliderSequence.AppendCallback(() =>
            {
                _internalSlider.value = 0;
                _isActive = false;
            });
        }
    }
}