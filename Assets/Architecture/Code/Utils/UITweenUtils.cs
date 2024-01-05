using System;
using DG.Tweening;
using UnityEngine.UI;

namespace FlatLands.Architecture
{
    public static class UITweenUtils
    {
        public static Tweener DoValue(this Slider slider, float toValue, float duration, Action<float> onUpdate = null, Action onCompleted = null)
        {
            var curValue = slider.value;
            return DOTween
                .To(() => curValue, setter => curValue = setter, toValue, duration)
                .OnUpdate((() => slider.value = curValue));
        }
    }
}