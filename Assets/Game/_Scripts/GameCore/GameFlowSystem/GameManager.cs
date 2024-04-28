using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameCore.InGame.TileSystem.Managers.Answer;
using GameCore.Managers;
using GameCore.TileSystem.Architecture;
using GameCore.TileSystem.Managers;
using UnityEngine;

namespace GameCore.GameFlowSystem
{
    public class GameManager : MonoBehaviour
    {
        private static GameState _gameState = GameState.Playing;
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

        private CancellationTokenSource cts = new();
        private async void OnAnswerTilesChanged()
        {
            if (_gameState == GameState.GameOver)
            {
                CheckForStopTheTask();
                return;
            }

            await CheckGameOver(cts);

            void CheckForStopTheTask()
            {
                cts.Cancel();
                cts.Dispose();
            }
        }

        public async Task CheckGameOver(CancellationTokenSource cts)
        {
            await Task.Delay(500, cts.Token);

            if (RuntimeTileManager.Instance.IsAllTilesUsed())
            {
                EndTheGame();
                return;
            }

            var unlockedTiles = RuntimeTileManager.Instance.GetUnlockedTiles().OrderBy(x => x.GetTileElements().GetLayer()).ToList();
            var isAnswerTilesEmpty = AnswerTilesManager.Instance.IsAnswerTilesEmpty();

            Task<string[]> GPWAT;
            if (isAnswerTilesEmpty)
                GPWAT = PossibleWordsSystem.PossibleWordsGenerator.GetPossibleWordsAsync(unlockedTiles.Select(x => x.GetTileElements().GetCharacter()).ToArray(), checkOneTime: true);
            else
            {
                var answer = AnswerTilesManager.Instance.GetAnswer();
                var firstCharacter = answer[0].ToString();
                GPWAT = PossibleWordsSystem.PossibleWordsGenerator.GetPossibleWordsAsync(firstCharacter, unlockedTiles.Select(x => x.GetTileElements().GetCharacter()).ToArray(), checkOneTime: true);
            }

            await Task.WhenAny(GPWAT, Task.Delay(5000, cts.Token));
            var possibleWordsFromUnlockedTiles = GPWAT.Result;
            Debug.Log($"Possible Words From Unlocked Tiles: {string.Join(", ", possibleWordsFromUnlockedTiles)}");

            if (possibleWordsFromUnlockedTiles.Length != 0) return;
            GPWAT.Dispose();

            var allTiles = RuntimeTileManager.Instance.GetAllTiles().Where(x => x.GetTileState() != TileState.Used).OrderBy(x => x.GetTileElements().GetLayer()).ToList();
            //var sameLayerTiles = allTiles.Where(x => x.GetTileElements().GetLayer() == allTiles.First().GetTileElements().GetLayer()).ToList();
            //var letters = allTiles.Select(x => x.GetTileElements().GetCharacter()).ToArray();

            var hasWord = false;
            foreach (var tile in allTiles)
            {
                var sameLayerTiles = allTiles.Where(x => x.GetTileElements().GetLayer() == tile.GetTileElements().GetLayer()).ToList();

                foreach (var layerTiles in sameLayerTiles)
                {
                    var startsWith = layerTiles.GetTileElements().GetCharacter();
                    var letters = layerTiles.GetTileElements().GetAllChildTiles().Select(x => x.GetTileElements().GetCharacter()).ToArray();
                    letters = letters.Append(layerTiles.GetTileElements().GetCharacter()).ToArray();

                    GPWAT = PossibleWordsSystem.PossibleWordsGenerator.GetPossibleWordsAsync(startsWith, letters, checkOneTime: true);
                    await Task.WhenAny(GPWAT, Task.Delay(5000, cts.Token));
                    var possibleWordsFromSameLayerTiles = GPWAT.Result;
                    Debug.Log($"Possible Words From Same Layer Tiles: {string.Join(", ", possibleWordsFromSameLayerTiles)}");

                    if (possibleWordsFromSameLayerTiles.Length != 0)
                    {
                        hasWord = true;
                        break;
                    }
                }

                if (hasWord) break;
            }

            /*
            var hasWord = false;
            foreach (var tile in sameLayerTiles)
            {
                var startsWith = tile.GetTileElements().GetCharacter();
                var letters = tile.GetTileElements().GetAllChildTiles().Select(x => x.GetTileElements().GetCharacter()).ToArray();
                letters = letters.Append(tile.GetTileElements().GetCharacter()).ToArray();

                GPWAT = PossibleWordsSystem.PossibleWordsGenerator.GetPossibleWordsAsync(startsWith, letters, checkOneTime: true);
                await Task.WhenAny(GPWAT, Task.Delay(5000, cts.Token));
                var possibleWordsFromSameLayerTiles = GPWAT.Result;
                Debug.Log($"Possible Words From Same Layer Tiles: {string.Join(", ", possibleWordsFromSameLayerTiles)}");

                if (possibleWordsFromSameLayerTiles.Length != 0)
                {
                    hasWord = true;
                    break;
                }
            }
            */

            if (hasWord) return;

            GPWAT.Dispose();
            EndTheGame();
        }

        private void SetPlayerLevel()
        {
            var playerLevel = PlayerManager.Instance.GetCurrentPlayerLevel();
            var currentLevel = PlayerManager.Instance.GetCurrentPlayingLevel();
            if (currentLevel == playerLevel)
            {
                PlayerManager.Instance.SetCurrentPlayerLevel(playerLevel + 1);
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
