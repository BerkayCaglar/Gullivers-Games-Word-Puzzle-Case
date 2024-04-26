using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.SingletonSystem;

namespace GameCore.Managers
{
    [CreateAssetMenu(fileName = "PlayerManager", menuName = "ScriptableObjects/PlayerManager", order = 1)]
    public class PlayerManager : ScriptableResourceSingleton<PlayerManager>
    {
        public int CurrentPlayerLevel;
        public int CurrentPlayingLevel;

        public void SetCurrentPlayerLevel(int level)
        {
            CurrentPlayerLevel = level;
        }

        public void SetCurrentPlayingLevel(int level)
        {
            CurrentPlayingLevel = level;
        }

        public int GetCurrentPlayerLevel()
        {
            return CurrentPlayerLevel;
        }

        public int GetCurrentPlayingLevel()
        {
            return CurrentPlayingLevel;
        }

        public bool IsCurrentPlayerLevelEnoughToUnlockLevel(int level)
        {
            return CurrentPlayerLevel >= level;
        }
    }
}
