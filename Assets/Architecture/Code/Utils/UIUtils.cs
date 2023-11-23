using System;
using DG.Tweening;
using UnityEngine.UI;

namespace FlatLands.Architecture
{
    public static class UIUtils
    {
        public static void DoValue(float startValue, float endValue, float duration, Action<float> onUpdate = null, Action onCompleted = null)
        {
            var curValue = startValue;
            DOTween.To(() => curValue, setter => curValue = setter, endValue, duration)
                .OnUpdate(() =>
                {
                    onUpdate?.Invoke(curValue);
                })
                .OnComplete(() =>
                {
                    onCompleted?.Invoke();
                });
        }
        
        public static Tweener DoValue(this Slider slider, float toValue, float duration)
        {
            var curValue = slider.value;
            return DOTween
                .To(() => curValue, setter => curValue = setter, toValue, duration)
                .OnUpdate((() => slider.value = curValue));
        }
    }
}