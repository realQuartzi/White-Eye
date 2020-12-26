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
    public class CommandHandler
    {
        public static Task Commander(SocketMessage msg)
        {
            if (msg.Author.IsBot)
                return Task.CompletedTask;

            if (!msg.Content.StartsWith("!"))
                return Task.CompletedTask;

            var user = msg.Author as SocketGuildUser;

            string cmd = "";
            string[] args;
            int cmdLength;

            if (msg.Content.Contains(" "))
                cmdLength = msg.Content.IndexOf(" ");
            else
                cmdLength = msg.Content.Length;

            cmd = msg.Content.Substring(1, cmdLength - 1).ToLower();

            args = msg.Content.Split(' ');

            if (cmd.Equals("wl-add"))
            {
                if (user.GuildPermissions.KickMembers ||
                    user.GuildPermissions.BanMembers)
                {
                    if (!String.IsNullOrEmpty(args[1]))
                    {
                        var guild = msg.Channel as SocketGuildChannel;

                        Whitelist.AddWhiteList(guild.Guild.Id, ulong.Parse(args[1]), msg);
                    }
                }
                else
                {
                    msg.Channel.SendMessageAsync("You do not have permission to use this command");
                }


            }

            if (cmd.Equals("wl-remove"))
            {
                if (user.GuildPermissions.KickMembers ||
                    user.GuildPermissions.BanMembers)
                {
                    if (!String.IsNullOrEmpty(args[1]))
                    {
                        var guild = msg.Channel as SocketGuildChannel;

                        Whitelist.RemoveWhiteList(guild.Guild.Id, ulong.Parse(args[1]), msg);
                    }
                }
                else
                {
                    msg.Channel.SendMessageAsync("You do not have permission to use this command");
                }

            }





            return Task.CompletedTask;
        }

    }
}
