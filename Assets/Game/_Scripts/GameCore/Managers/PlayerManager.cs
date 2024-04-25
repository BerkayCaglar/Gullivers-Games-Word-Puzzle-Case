using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.SingletonSystem;

namespace GameCore.Managers
{
    [CreateAssetMenu(fileName = "PlayerManager", menuName = "ScriptableObjects/PlayerManager", order = 1)]
    public class PlayerManager : ScriptableResourceSingleton<PlayerManager>
    {
        public int CurrentLevel;

        public void SetCurrentLevel(int level)
        {
            CurrentLevel = level;
        }

        public int GetCurrentLevel()
        {
            return CurrentLevel;
        }

        public bool IsCurrentLevelEnoughToUnlockLevel(int level)
        {
            return CurrentLevel >= level;
        }
    }
}
