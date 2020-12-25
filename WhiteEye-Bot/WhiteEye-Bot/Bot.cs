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
    class Bot
    {
        static void Main(string[] args)
        => new Bot().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient client;
        CommandService commands;

        string appPath;
        string dataPath;

        public async Task MainAsync()
        {
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });

            commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug
            });

            client.MessageReceived += CommandHandler;


            client.Ready += ClientReady;
            client.Log += Log;

            client.UserJoined += CheckWhiteList;
            

            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string updatedPath = Path.GetFullPath(Path.Combine(path, @"../"));

            appPath = updatedPath;

            CreateDirectory();

            var token = GetToken();

            if (String.IsNullOrEmpty(token))
            {
                Console.WriteLine("No Token Found!");
                Environment.Exit(-1);
            }
            else
            {
                Console.WriteLine("Token Found!");
            }

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private Task ClientReady()
        {
            Console.WriteLine("Client is Ready!");
            return Task.CompletedTask;
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task CommandHandler(SocketMessage msg)
        {
            if (msg.Author.IsBot)
                return Task.CompletedTask;

            if (!msg.Content.StartsWith("!"))
                return Task.CompletedTask;

            string cmd = "";
            string[] args;
            int cmdLength;

            if(msg.Content.Contains(" "))
                cmdLength = msg.Content.IndexOf(" ");
            else
                cmdLength = msg.Content.Length;

            cmd = msg.Content.Substring(1, cmdLength - 1).ToLower();

            args = msg.Content.Split(' ');

            if (cmd.Equals("whitelist")) 
            {
                if (!String.IsNullOrEmpty(args[1]))
               {
                    var guild = msg.Channel as SocketGuildChannel;

                    AddWhiteList(guild.Guild.Id, ulong.Parse(args[1]));
                }
            }

            return Task.CompletedTask;
        }

        private Task CheckWhiteList(SocketGuildUser user)
        {
            if (!IsWhiteListed(user.Guild.Id, user.Id))
            {
                Console.WriteLine(user.Nickname + " was Kicked from the server as he was not whitelisted!");

                 user.KickAsync();
            }

            return Task.CompletedTask;
        }

        public bool IsWhiteListed(ulong guildID, ulong userID)
        {
            Console.WriteLine("Checking Whitelist...");

            string guildPath = dataPath + "/" + guildID + ".json";

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

        public void AddWhiteList(ulong guildID, ulong userID)
        {
            string guildPath = dataPath + "/" + guildID + ".json";

            if(!File.Exists(guildPath))
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

            if(userID.ToString().Length == 18)
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

        public string GetToken()
        {
            Console.WriteLine(appPath);

            if (!File.Exists(appPath + "token.txt"))
            {
                File.Create(appPath + "token.txt").Dispose();

                Console.WriteLine("1. Not Token File Found! Creating new Token File!");
                Console.WriteLine("2. Please enter the Bot Token in the token.txt File!");
                Console.WriteLine("3. Restart Application when the Token Key was Added");
                Console.ReadLine();
            }

            var token = File.ReadAllText(appPath + "token.txt");

            return token;
        }

        public void CreateDirectory()
        {
            if(!Directory.Exists(appPath + "Data"))
            {
                Console.WriteLine("Created Data Path");
                Directory.CreateDirectory(appPath + "Data");
            }

            dataPath = appPath + "Data";
        }
    }

    public class WhitelistData
    {
        public List<ulong> userIDs;
    }


}
