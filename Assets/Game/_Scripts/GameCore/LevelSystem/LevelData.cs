using UnityEngine;

namespace GameCore.LevelSystem
{
    [System.Serializable]
    public class LevelData
    {
        public string title;
        public int level;
        public LevelTileData[] tiles;
    }

    [System.Serializable]
    public class LevelTileData
    {
        public string id;
        public Vector3 position;
        public string character;
        public string[] children;
    }
}
