using Microsoft.EntityFrameworkCore;

namespace DiscordBotTestAPI
{
    public class Helper
    {
        static IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

        public static async void WritePapyrusPlayerPositionsFile(PlayerPositionsDb db)
        {
            var filePath = config["PapyrusPlayerPositionsFile"];
            var text = "var playersData = {\"players\": [";

            var playerPositions = await db.PlayerPositions.ToListAsync();
            string addPlayerPosition;
            foreach (var playerPosition in playerPositions)
            {
                addPlayerPosition = "{\"name\": \"" + playerPosition.Name + "\",\"dimensionId\": " + playerPosition.DimensionId + ",\"position\": [" + playerPosition.XCoord + ",70," + playerPosition.ZCoord + "],\"color\": \"" + playerPosition.Color + "\",\"visible\": " + (playerPosition.Visible ? "true" : "false") + "},";
                text = text + addPlayerPosition;
            }

            text = text + "]};";

            File.WriteAllText(filePath, text);
        }

        public static async void WriteUnminedPlayerPositionsFile(PlayerPositionsDb db)
        {
            var filePath = config["UnminedPlayerPositionsFile"];
            var text = "UnminedCustomMarkers = {isEnabled: true,markers: [";

            var playerPositions = await db.PlayerPositions.ToListAsync();
            string addPlayerPosition;
            foreach (var playerPosition in playerPositions)
            {
                if(playerPosition.DimensionId != 0)
                {
                    break;
                }
                addPlayerPosition = "{x: " + playerPosition.XCoord + ",z: " + playerPosition.ZCoord + ",image: \"custom.pin.png\",imageAnchor: [0.5, 1],imageScale: 0.25,text: \"" + playerPosition.Name + "\",textColor: \"" + playerPosition.Color + "\", offsetX: 0,offsetY: 10,font: \"bold 18px Calibri, sans serif\",},";

                text += addPlayerPosition;
            }

            text = text + "]}";

            File.WriteAllText(filePath, text);
        }
    }
}
