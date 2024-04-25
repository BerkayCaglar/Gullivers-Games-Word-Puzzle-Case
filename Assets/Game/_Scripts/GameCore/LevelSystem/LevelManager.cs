using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.SingletonSystem;

namespace GameCore.LevelSystem
{
    [CreateAssetMenu(fileName = "LevelManager", menuName = "ScriptableObjects/LevelManager", order = 1)]
    public class LevelManager : ScriptableResourceSingleton<LevelManager>
    {
        public LevelData[] levels;

        public LevelData[] GetLevels()
        {
            return levels;
        }

        public LevelData GetLevelData(int level)
        {
            return levels[level];
        }

        public int GetLevelCount()
        {
            return levels.Length;
        }

        public void SetLevels(LevelData[] levels)
        {
            this.levels = levels;
        }

        public void Setup(List<LevelData> levelDatas)
        {
            levels = levelDatas.ToArray();
        }
    }
}
