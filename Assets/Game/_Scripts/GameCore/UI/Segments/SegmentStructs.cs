namespace GameCore.UI.Segments
{
    public struct SegmentData
    {
        public int LevelPoint;
        public string LevelName;
        public string LevelHighScore;

        public SegmentData(int levelPoint, string levelName, string levelHighScore)
        {
            LevelPoint = levelPoint;
            LevelName = levelName;
            LevelHighScore = levelHighScore;
        }
    }
}
