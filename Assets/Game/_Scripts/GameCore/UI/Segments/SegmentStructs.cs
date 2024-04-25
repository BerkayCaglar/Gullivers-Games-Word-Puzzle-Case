namespace GameCore.UI.Segments
{
    public struct SegmentData
    {
        public int LevelPoint;
        public int LevelHighScore;
        public string LevelName;

        public SegmentData(int levelPoint, string levelName, int levelHighScore)
        {
            LevelPoint = levelPoint;
            LevelName = levelName;
            LevelHighScore = levelHighScore;
        }
    }
}
