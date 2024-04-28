using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameCore.InGame.TileSystem.Managers.Answer;
using GameCore.PlayerJourneySystem;
using GameCore.TileSystem.Architecture;
using GameCore.TileSystem.Managers;
using UnityEngine;

namespace GameCore.GameFlowSystem
{
    public class GameManager : MonoBehaviour
    {
        private static GameState _gameState = GameState.Playing;
        private string _currentFirstCharacter;

        public static GameState GetGameState()
        {
            return _gameState;
        }

        private void Awake()
        {
            _gameState = GameState.Playing;
            TileActions.OnAnswerTilesChanged += OnAnswerTilesChanged;
        }

        private void OnDestroy()
        {
            TileActions.OnAnswerTilesChanged -= OnAnswerTilesChanged;
        }

        private void OnAnswerTilesChanged()
        {
            if (_gameState == GameState.GameOver)
            {
                StopAllCoroutines();
                return;
            }

            StopAllCoroutines();
            StartCoroutine(CheckGameOver());
        }

        public IEnumerator CheckGameOver()
        {
            yield return new WaitForSeconds(0.5f);

            if (RuntimeTileManager.Instance.IsAllTilesUsed())
            {
                EndTheGame();
                yield break;
            }

            var unlockedTiles = RuntimeTileManager.Instance.GetUnlockedTiles().OrderBy(x => x.GetTileElements().GetLayer()).ToList();
            var isAnswerTilesEmpty = AnswerTilesManager.Instance.IsAnswerTilesEmpty();

            Task<string[]> GPWAT;
            if (isAnswerTilesEmpty)
            {
                _currentFirstCharacter = string.Empty;
                GPWAT = PossibleWordsSystem.PossibleWordsGenerator.GetPossibleWordsAsync(unlockedTiles.Select(x => x.GetTileElements().GetCharacter()).ToArray(), checkOneTime: true);
            }
            else
            {
                var answer = AnswerTilesManager.Instance.GetAnswer();
                var firstCharacter = answer[0].ToString();
                if (firstCharacter == _currentFirstCharacter) yield break; // Check if the first character is the same as the previous one (to prevent unnecessary API calls)
                _currentFirstCharacter = firstCharacter;
                GPWAT = PossibleWordsSystem.PossibleWordsGenerator.GetPossibleWordsAsync(firstCharacter, unlockedTiles.Select(x => x.GetTileElements().GetCharacter()).ToArray(), checkOneTime: true);
            }

            yield return new WaitUntil(() => GPWAT.IsCompleted);
            var possibleWordsFromUnlockedTiles = GPWAT.Result;

            if (possibleWordsFromUnlockedTiles.Length != 0) yield break;
            GPWAT.Dispose();

            var allTiles = RuntimeTileManager.Instance.GetAllTiles().Where(x => x.GetTileState() != TileState.Used).OrderBy(x => x.GetTileElements().GetLayer()).ToList();

            var hasWord = false;
            foreach (var tile in allTiles)
            {
                var sameLayerTiles = allTiles.Where(x => x.GetTileElements().GetLayer() == tile.GetTileElements().GetLayer()).ToList();

                foreach (var layerTiles in sameLayerTiles)
                {
                    var startsWith = layerTiles.GetTileElements().GetCharacter();
                    List<string> letters = new()
                    {
                        startsWith
                    };
                    letters.AddRange(layerTiles.GetTileElements().GetAllChildTiles().Select(x => x.GetTileElements().GetCharacter()));
                    GPWAT = PossibleWordsSystem.PossibleWordsGenerator.GetPossibleWordsAsync(startsWith, letters.ToArray(), checkOneTime: true);
                    yield return new WaitUntil(() => GPWAT.IsCompleted);
                    var possibleWordsFromSameLayerTiles = GPWAT.Result;

                    if (possibleWordsFromSameLayerTiles.Length != 0)
                    {
                        hasWord = true;
                        break;
                    }
                }

                if (hasWord) break;
            }

            if (hasWord) yield break;

            GPWAT.Dispose();
            EndTheGame();
        }

        private async void SetPlayerLevel()
        {
            var playerLevel = PlayerManager.Instance.GetCurrentPlayerLevel();
            var currentLevel = PlayerManager.Instance.GetCurrentPlayingLevel();
            if (currentLevel == playerLevel)
            {
                await PlayerManager.Instance.SetCurrentPlayerLevel(playerLevel + 1);
            }
        }

        private void EndTheGame()
        {
            GameActions.GameOver();
            _gameState = GameState.GameOver;
            SetPlayerLevel();
        }
    }
}
