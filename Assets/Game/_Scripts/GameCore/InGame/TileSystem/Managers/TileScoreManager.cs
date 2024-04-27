using UnityEngine;

namespace GameCore.TileSystem.Managers
{
    public class TileScoreManager
    {
        public static int GetCharacterScore(string character)
        {
            var score = character.ToUpper() switch
            {
                "E" or "A" or "O" or "N" or "R" or "T" or "L" or "S" or "U" or "I" => 1,
                "D" or "G" => 2,
                "B" or "C" or "M" or "P" => 3,
                "F" or "H" or "V" or "W" or "Y" => 4,
                "K" => 5,
                "J" or "X" => 8,
                "Q" or "Z" => 10,
                _ => 0,
            };

            if (score == 0)
            {
                Debug.LogError($"<color=green>[TILE MANAGER]</color> -> Character <color=red>{character}</color> is not found in the dictionary");
            }

            return score;
        }

        public static int GetWordScore(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                Debug.LogError($"<color=green>[TILE MANAGER]</color> -> Word is empty");
                return 0;
            }

            var score = 0;
            foreach (var character in word)
            {
                score += GetCharacterScore(character.ToString());
            }

            score *= 10 * word.Length;

            return score;
        }
    }
}
