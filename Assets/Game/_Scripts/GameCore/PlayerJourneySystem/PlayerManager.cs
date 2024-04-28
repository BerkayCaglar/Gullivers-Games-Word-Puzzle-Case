using UnityEngine;
using GameCore.SingletonSystem;
using MyBox;
using System.Threading.Tasks;

namespace GameCore.PlayerJourneySystem
{
    [CreateAssetMenu(fileName = "PlayerManager", menuName = "ScriptableObjects/PlayerManager", order = 1)]
    public class PlayerManager : ScriptableResourceSingleton<PlayerManager>
    {
        public async Task SetCurrentPlayerLevel(int level)
        {
            var playerData = new PlayerData
            {
                playerLevel = level,
                playingLevel = GetCurrentPlayingLevel()
            };
            await PlayerJsonUtility.WriteToJsonAsync(playerData);
        }

        public async Task SetCurrentPlayingLevel(int level)
        {
            var playerData = new PlayerData
            {
                playerLevel = GetCurrentPlayerLevel(),
                playingLevel = level
            };
            await PlayerJsonUtility.WriteToJsonAsync(playerData);
        }

        public int GetCurrentPlayerLevel()
        {
            return PlayerJsonUtility.GetPlayerDataFromJson().playerLevel;
        }

        public int GetCurrentPlayingLevel()
        {
            return PlayerJsonUtility.GetPlayerDataFromJson().playingLevel;
        }

        public bool IsCurrentPlayerLevelEnoughToUnlockLevel(int level)
        {
            return GetCurrentPlayerLevel() >= level;
        }

        public async Task Setup()
        {
            var playerData = PlayerJsonUtility.GetPlayerDataFromJson();
            if (playerData == null)
            {
                playerData = new PlayerData() { playerLevel = 1, playingLevel = 0 };
                await PlayerJsonUtility.WriteToJsonAsync(playerData);
            }
        }

        [ButtonMethod]
        public async void ResetPlayerData()
        {
            var playerData = new PlayerData { playerLevel = 1, playingLevel = 0 };
            await PlayerJsonUtility.WriteToJsonAsync(playerData);
        }
    }
}
