using System;
using DG.Tweening;

namespace FlatLands.Architecture
{
    public static class TweenUtils
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
    }
}