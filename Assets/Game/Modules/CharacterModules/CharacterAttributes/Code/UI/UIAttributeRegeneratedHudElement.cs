﻿using DG.Tweening;
using FlatLands.Architecture;
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
            _mainSlider.minValue = _attribute.MinValue;
            _mainSlider.maxValue = _attribute.MaxValue;
            _mainSlider.value = _attribute.Value;
            
            _internalSlider.minValue = _attribute.MinValue;
            _internalSlider.maxValue = _attribute.MaxValue;
            _internalSlider.value = _attribute.Value;
            
            if (_attribute != null)
                _attribute.OnValueChangedDetails += HandleValueChanged;
        }

        private void HandleValueChanged(bool addValue, float prevValue, float newValue)
        {
            _sliderSequence?.Kill();
            _sliderSequence = DOTween.Sequence();

            if (addValue)
            {
                _internalSlider.value = 0;
                _sliderSequence.Append(_mainSlider.DoValue(newValue, 0.3f));
                _sliderSequence.AppendCallback(() => _internalSlider.value = 0);
                return;
            }

            _internalSlider.value = prevValue;
            _mainSlider.value = newValue;
            _sliderSequence.AppendInterval(0.3f);
            _sliderSequence.Append(_internalSlider.DoValue(newValue, 0.3f));
            _sliderSequence.AppendCallback(() => _internalSlider.value = 0);
        }

        // private float _debugValue = 100;
        //
        // private void Start()
        // {
        //     _mainSlider.minValue = 0;
        //     _mainSlider.maxValue = 100;
        //     _mainSlider.value = _debugValue;
        //     
        //     _internalSlider.minValue = 0;
        //     _internalSlider.maxValue = 100;
        //     _internalSlider.value = _debugValue;
        // }
        //
        // [Button]
        // private void AddValue()
        // {
        //     HandleValueChanged(true, _debugValue, _debugValue + 10);
        //     _debugValue += 10;
        // }
        //
        // [Button]
        // private void RemoveValue()
        // {
        //     HandleValueChanged(false, _debugValue, _debugValue - 10);
        //     _debugValue -= 10;
        // }
    }
}