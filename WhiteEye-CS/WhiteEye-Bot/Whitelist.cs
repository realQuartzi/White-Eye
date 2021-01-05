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

        //UserLeave WhiteList Remove
        public static Task WhiteListLeave(SocketGuildUser user)
        {
            if(IsWhiteListed(user.Guild.Id, user.Id))
            {
                RemoveWhiteList(user.Guild.Id, user.Id, null);
                Console.WriteLine("UserID: " + user.Id + " removed from WhiteList through Leave Event");
            }

            return Task.CompletedTask;
        }

        //UserBan WhiteList Remove
        public static Task WhiteListBanned(SocketUser user, SocketGuild guild)
        {
            if(IsWhiteListed(guild.Id, user.Id))
            {
                RemoveWhiteList(guild.Id, user.Id, null);
                Console.WriteLine("UserID: " + user.Id +  " removed from WhiteList through Ban Event");
            }

            return Task.CompletedTask;
        }

        //Returns if the User is on the Whitelist or not
        public static bool IsWhiteListed(ulong guildID, ulong userID)
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
        public static void AddWhiteList(ulong guildID, ulong userID, SocketMessage msg)
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

                if (!data.userIDs.Contains(userID))
                {
                    data.userIDs.Add(userID);

                    JsonSerializer s = new JsonSerializer();

                    using (StreamWriter sw = new StreamWriter(guildPath))
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        s.Serialize(writer, data);
                    }

                    msg.Channel.SendMessageAsync("UserID: " + userID + " has been added to the Whitelist!");
                }
                else
                {
                    msg.Channel.SendMessageAsync("A User with that ID is already Whitelisted!");
                }

            }
            else
            {
                msg.Channel.SendMessageAsync("The given UserID is not valid!");
            }
        }

        //Remove User from WhiteList Function
        public static void RemoveWhiteList(ulong guildID, ulong userID, SocketMessage msg)
        {
            string guildPath = Bot.dataPath + "/" + guildID + ".json";

            if (userID.ToString().Length == 18)
            {
                WhitelistData data = JsonConvert.DeserializeObject<WhitelistData>(File.ReadAllText(guildPath));

                if (data.userIDs.Contains(userID))
                {
                    data.userIDs.Remove(userID);

                    JsonSerializer s = new JsonSerializer();

                    using (StreamWriter sw = new StreamWriter(guildPath))
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        s.Serialize(writer, data);
                    }

                    if(msg != null)
                    {
                        msg.Channel.SendMessageAsync("UserID: " + userID + " has been removed from the Whitelist!");
                    }

                }
                else
                {
                    if(msg != null)
                    {
                        msg.Channel.SendMessageAsync("This UserID is already not whitelisted!");
                    }

                }

            }
            else
            {
                if(msg != null)
                {
                    msg.Channel.SendMessageAsync("The given UserID is not valid!");
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
