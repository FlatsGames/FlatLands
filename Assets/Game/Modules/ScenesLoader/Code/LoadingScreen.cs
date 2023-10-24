using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FlatLands.Loader
{
    public sealed class LoadingScreen : SerializedMonoBehaviour
    {
        [SerializeField, BoxGroup("Main Settings")] 
        private Camera _loadingCamera;
        
        [SerializeField, BoxGroup("Main Settings")] 
        private Slider _loadingSlider;
        
        [SerializeField, BoxGroup("Main Settings")] 
        private TMP_Text _loadingText;
        
        [SerializeField, BoxGroup("Main Settings")] 
        private CanvasGroup _canvasGroup;
        
        [SerializeField, BoxGroup("Main Settings"), Space] 
        private Image _firstLoadingImage;
        
        [SerializeField, BoxGroup("Main Settings")] 
        private Image _secondLoadingImage;
        
        [SerializeField, BoxGroup("Main Settings")] 
        private Image _thirdLoadingImage;

        [SerializeField, BoxGroup("Animation Settings")]
        private float _fadeDuration;
        
        [SerializeField, BoxGroup("Animation Settings")]
        private float _animationDuration;

        private Sequence _loadingSequence;
        
        public void ShowScreen(Action callback = null)
        {
            _loadingCamera.gameObject.SetActive(true);
            _canvasGroup.gameObject.SetActive(true);
            
            PlayAnimation();
            UpdateProgress(0);
            var sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(1, _fadeDuration));
            sequence.OnComplete(() => callback?.Invoke());
        }
        
        public void HideScreen(Action callback = null)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(0, _fadeDuration));
            sequence.OnComplete(() =>
            {
                _loadingCamera.gameObject.SetActive(false);
                _canvasGroup.gameObject.SetActive(false);
                callback?.Invoke();
            });
        }

        public void UpdateProgress(float progress, float maxProgress = 1)
        {
            var curProgress = Mathf.Clamp(progress, 0, 100) * 100;
            
            _loadingSlider.maxValue = maxProgress;
            _loadingSlider.value = curProgress;
            _loadingText.SetText($"{curProgress} %");
        }

        private void PlayAnimation()
        {
            _loadingSequence?.Kill();
            
            _loadingSequence = DOTween.Sequence();
            _loadingSequence.Append(_firstLoadingImage.transform.DOLocalRotate(new Vector3(0, 0, 180), _animationDuration / 6, RotateMode.FastBeyond360));
            _loadingSequence.Append(_secondLoadingImage.transform.DOLocalRotate(new Vector3(0, 0, -180), _animationDuration / 6, RotateMode.FastBeyond360));
            _loadingSequence.Append(_thirdLoadingImage.transform.DOLocalRotate(new Vector3(0, 0, 180), _animationDuration / 6, RotateMode.FastBeyond360));
            _loadingSequence.Append(_firstLoadingImage.transform.DOLocalRotate(new Vector3(0, 0, 0), _animationDuration / 6, RotateMode.FastBeyond360));
            _loadingSequence.Append(_secondLoadingImage.transform.DOLocalRotate(new Vector3(0, 0, 0), _animationDuration / 6, RotateMode.FastBeyond360));
            _loadingSequence.Append(_thirdLoadingImage.transform.DOLocalRotate(new Vector3(0, 0, 0), _animationDuration / 6, RotateMode.FastBeyond360));
            _loadingSequence.SetLoops(-1);
        }
    }
}