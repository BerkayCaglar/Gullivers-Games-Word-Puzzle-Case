using System.Collections;
using DG.Tweening;
using GameCore.ScoreSystem;
using TMPro;
using UnityEngine;

namespace GameCore.UI.Text
{
    public class ScoreTextAnimator : MonoBehaviour
    {
        private TextMeshProUGUI _scoreText;

        private void Awake()
        {
            _scoreText = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            StartCoroutine(AnimateScoreCoroutine(RuntimeScoreManager.Instance.GetCurrentScore()));
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator AnimateScoreCoroutine(int score)
        {
            var currentScore = int.Parse(_scoreText.text);
            var scoreDifference = score - currentScore;
            var scoreStep = scoreDifference / 100;
            for (int i = 0; i < 10; i++)
            {
                currentScore += scoreStep;
                _scoreText.text = currentScore.ToString();
                yield return new WaitForSeconds(0.1f);
            }
            _scoreText.text = score.ToString();
        }

        public void AnimateScoreWithTween(int score)
        {
            var currentScore = int.Parse(_scoreText.text);
            DOTween.To(() => currentScore, x => _scoreText.text = x.ToString(), score, 1f);
        }
    }
}
