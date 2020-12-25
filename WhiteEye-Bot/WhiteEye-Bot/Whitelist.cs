using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Discord.Commands;
using System.Reflection;

namespace WhiteEye_Bot
{
    public class Whitelist
    {
        //UserJoin WhiteList Check
        public static Task CheckWhiteList(SocketGuildUser user)
        {
            if (!IsWhiteListed(user.Guild.Id, user.Id))
            {
                Console.WriteLine(user.Nickname + " was Kicked from the server as he was not whitelisted!");

                user.KickAsync();
            }

            return Task.CompletedTask;
        }

        //Returns if the User is on the Whitelist or not
        public static bool IsWhiteListed(ulong guildID, ulong userID)
        {
            Console.WriteLine("Checking Whitelist...");

            string guildPath = Bot.dataPath + "/" + guildID + ".json";

            if (!File.Exists(guildPath))
            {
                File.Create(guildPath).Dispose();

                WhitelistData whitelist = new WhitelistData
                {
                    userIDs = new List<ulong>()
                };

                JsonSerializer s = new JsonSerializer();

                using (StreamWriter sw = new StreamWriter(guildPath))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    s.Serialize(writer, whitelist);
                }
            }

            WhitelistData data = JsonConvert.DeserializeObject<WhitelistData>(File.ReadAllText(guildPath));

            if (data.userIDs.Contains(userID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Add User to WhiteList Function
        public static void AddWhiteList(ulong guildID, ulong userID)
        {
            string guildPath = Bot.dataPath + "/" + guildID + ".json";

            if (!File.Exists(guildPath))
            {
                File.Create(guildPath).Dispose();

                WhitelistData whitelist = new WhitelistData
                {
                    userIDs = new List<ulong>()
                };

                JsonSerializer s = new JsonSerializer();

                using (StreamWriter sw = new StreamWriter(guildPath))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    s.Serialize(writer, whitelist);
                }
            }

            if (userID.ToString().Length == 18)
            {
                WhitelistData data = JsonConvert.DeserializeObject<WhitelistData>(File.ReadAllText(guildPath));

                data.userIDs.Add(userID);

                JsonSerializer s = new JsonSerializer();

                using (StreamWriter sw = new StreamWriter(guildPath))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    s.Serialize(writer, data);
                }
            }
        }
    }

    //White List Data Class
    public class WhitelistData
    {
        public List<ulong> userIDs;
    }
}
