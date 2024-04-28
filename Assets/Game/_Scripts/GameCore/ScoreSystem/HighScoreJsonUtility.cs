using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace GameCore.ScoreSystem
{
    public class HighScoreJsonUtility
    {
        public static List<HighScoreData> GetHighScoresFromJson()
        {
            try
            {
                var json = System.IO.File.ReadAllText(HighScoreConstants.HIGH_SCORE_DATABASE_JSON_PATH);
                return JsonConvert.DeserializeObject<List<HighScoreData>>(json);
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public static void WriteToJson(List<HighScoreData> highScores)
        {
            var json = JsonConvert.SerializeObject(highScores, Formatting.Indented);
            System.IO.File.WriteAllText(HighScoreConstants.HIGH_SCORE_DATABASE_JSON_PATH, json);
        }
    }
}
