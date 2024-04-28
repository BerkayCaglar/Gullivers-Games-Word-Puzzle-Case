using UnityEngine;

namespace GameCore.LevelSystem
{
    [System.Serializable]
    public class LevelData
    {
        public string title;
        public int levelPoint;
        public string[] correctAnswers;
        public LevelTileData[] tiles;
    }

    [System.Serializable]
    public class LevelTileData
    {
        public int id;
        public Vector3 position;
        public string character;
        public int[] children;
    }
}
