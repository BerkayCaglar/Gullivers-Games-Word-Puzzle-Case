using UnityEngine;

namespace GameCore.TileSystem.Architecture
{
    [CreateAssetMenu(fileName = "TileData", menuName = "TileSystem/TileData")]
    public class TileData : ScriptableObject
    {
        public int tileID;
        public string character;
        public int score;
    }
}
