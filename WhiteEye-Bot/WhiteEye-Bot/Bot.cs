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

        public static DiscordSocketClient client;
        CommandService commands;

        public static string appPath;
        public static string dataPath;

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

            client.MessageReceived += CommandHandler.Commander;


            client.Ready += ClientReady;
            client.Log += Log;

            client.UserJoined += Whitelist.CheckWhiteList;
            

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




}
