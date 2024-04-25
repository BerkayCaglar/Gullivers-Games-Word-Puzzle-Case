using UnityEngine;
using DG.Tweening;
using GameCore.SingletonSystem;
using System.Threading.Tasks;
using System;

namespace GameCore.UI
{
    public class CameraUIAnimations : PersistentSingleton<CameraUIAnimations>
    {
        [Header("Rect Transforms")]
        [Space(5)]
        [SerializeField] private RectTransform _fadeInRectTransform;
        [SerializeField] private RectTransform _fadeOutRectTransform;

        public async Task StartFadeInFadeOutAnimation(Func<Task> onFadeInFadeOutCompleted = null)
        {
            ResetSequence();
            await FadeInFadeOut();
            await onFadeInFadeOutCompleted();
        }

        public async Task StartFadeInFadeOutAnimationForLobby(Action onFadeInCompleted = null, Action onFadeOutCompleted = null)
        {
            ResetSequence(fadeInTargetScale: 0f, fadeOutTargetScale: 1f);

            await FadeIn(targetScale: 1f, duration: 0.3f);
            onFadeInCompleted?.Invoke();

            await FadeOut(targetScale: 0f, duration: 0.3f);
            onFadeOutCompleted?.Invoke();
        }

        private async Task FadeIn(float targetScale = 20f, float duration = 0.8f)
        {
            _fadeInRectTransform.gameObject.SetActive(true);

            var Sequence = DOTween.Sequence();

            Sequence.Append(_fadeInRectTransform.DOScale(targetScale, duration)
                .SetEase(Ease.OutFlash));

            Sequence.AppendCallback(() => _fadeInRectTransform.gameObject.SetActive(false));

            await Sequence.AsyncWaitForCompletion();
        }

        private async Task FadeInFadeOut(float targetScale = 20f, float duration = 0.8f)
        {
            _fadeInRectTransform.gameObject.SetActive(true);

            var Sequence = DOTween.Sequence();

            Sequence.Append(_fadeInRectTransform.DOScale(targetScale, duration)
                .SetEase(Ease.OutFlash));

            Sequence.AppendCallback(() => _fadeInRectTransform.gameObject.SetActive(false));

            await Sequence.AsyncWaitForCompletion();

            _ = FadeOut();
        }

        private Task FadeOut(float targetScale = 0f, float duration = 0.8f)
        {
            _fadeOutRectTransform.gameObject.SetActive(true);

            var Sequence = DOTween.Sequence();

            Sequence.Append(_fadeOutRectTransform.DOScale(targetScale, duration)
                .SetEase(Ease.InFlash));

            Sequence.AppendCallback(() => _fadeOutRectTransform.gameObject.SetActive(false));

            return Sequence.AsyncWaitForCompletion();
        }

        private void ResetSequence(float fadeInTargetScale = 0f, float fadeOutTargetScale = 20f)
        {
            _fadeInRectTransform.localScale = Vector3.one * fadeInTargetScale;
            _fadeOutRectTransform.localScale = Vector3.one * fadeOutTargetScale;
            DOTween.Kill(Tuple.Create(_fadeInRectTransform, _fadeOutRectTransform));
        }
    }
}
