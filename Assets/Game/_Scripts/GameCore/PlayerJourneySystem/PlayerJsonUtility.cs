using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GameCore.PlayerJourneySystem
{
    public class PlayerJsonUtility
    {
        public static PlayerData GetPlayerDataFromJson()
        {
            try
            {
                var json = System.IO.File.ReadAllText(PlayerConstants.PLAYER_DATA_JSON_PATH);
                return JsonConvert.DeserializeObject<PlayerData>(json);
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public static async Task WriteToJsonAsync(PlayerData playerData)
        {
            var json = JsonConvert.SerializeObject(playerData, Formatting.Indented);
            await System.IO.File.WriteAllTextAsync(PlayerConstants.PLAYER_DATA_JSON_PATH, json);
        }
    }
}
