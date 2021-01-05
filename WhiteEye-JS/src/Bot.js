require('dotenv').config();

const fs = require('fs');

const { Client } = require('discord.js');
const client = new Client();

const PREFIX = "!";

const dataDir = './data/';

client.on('ready', () => 
{
    console.log(`${client.user.username} has succesfully logged in!`);

    CreateDirectory();
});

client.on('guildMemberAdd', (user) => 
{
    if(CheckWhitelist(user.id, user.guild.id) == false)
    {
        user.kick("The User was not Whitelisted on this Server!");
    }
});

client.on('message', (message) => 
{
    //Check if Message Author is a Bot
    if(message.author.bot) return;

    if(message.channel.type == "dm") return;

    if(message.content.startsWith(PREFIX))
    {
        const [cmd, ...args] = message.content.trim().substring(PREFIX.length).split(/\s+/);

        if(cmd == "wl-add")
        {
            if(message.member.permissions.has("KICK_MEMBERS") || message.member.permissions.has("BAN_MEMBERS"))
            {
                AddWhitelist(args[0], message.channel.guild.id, message.channel);
            }
            else
            {
                message.channel.send("You do not have the required permissions to use this command!")
            }

        }
        else if(cmd == "wl-remove")
        {
            if(message.member.permissions.has("KICK_MEMBERS") || message.member.permissions.has("BAN_MEMBERS"))
            {
                RemoveWhitelist(args[0], message.channel.guild.id, message.channel);
            }
            else
            {
                message.channel.send("You do not have the required permissions to use this command!")
            }
        }
    }

    console.log(message.content);
});

client.login(process.env.WHITEEYE_BOT_TOKEN);


function AddWhitelist(userID, guildID, channel)
{
    CreateWhitelist(guildID);

    if(fs.existsSync(dataDir + guildID + ".json"))
    {
        let rawData = fs.readFileSync(dataDir + guildID + ".json");
        var data = JSON.parse(rawData);

        if(!data.userIDs.includes(userID))
        {
            data.userIDs.push(userID)

            let json = JSON.stringify(data, null, 2);

            fs.writeFileSync(dataDir + guildID + ".json", json);

            channel.send("The UserID " + userID + " has been added into the Whitelist!");
        }
        else channel.send("A User by that ID is already Whitelisted!");
    }
}

function RemoveWhitelist(userID, guildID, channel)
{
    CreateWhitelist(guildID);

    if(fs.existsSync(dataDir + guildID + ".json"))
    {
        let rawData = fs.readFileSync(dataDir + guildID + ".json");
        var data = JSON.parse(rawData);

        if(data.userIDs.includes(userID))
        {
            const index = data.userIDs.indexOf(userID);
            if(index > -1) 
            {
                data.userIDs.splice(index,1);

                let json = JSON.stringify(data, null, 2);

                fs.writeFileSync(dataDir + guildID + ".json", json);

                channel.send("The UserID " + userID + " has been removed from the Whitelist!");
            }
        }
        else channel.send("The User by that ID is already **not** Whitelisted!");
    }
}

function CheckWhitelist(userID, guildID)
{
    if(fs.existsSync(dataDir + guildID + ".json"))
    {
        let rawData = fs.readFileSync(dataDir + guildID + ".json");
        var data = JSON.parse(rawData);

        if(data.userIDs.includes(userID)) return true;

        console.log(userID + " could not be found in the Whitelist!");
    } 
    else CreateWhitelist(guildID);

    return false;
}

function CreateDirectory()
{
    var dir = './data';

    if(!fs.existsSync(dir))
    {
        console.log('Data Directory not found! Creating new Directory!');
        fs.mkdirSync(dir);
    }
}

function CreateWhitelist(guildID)
{
    if(!fs.existsSync(dataDir + guildID + ".json"))
    {
        console.log('File for ' + guildID + ' was not found! Creating new File!');

        let data = JSON.stringify(whitelistData, null, 2);

        fs.writeFileSync(dataDir + guildID + ".json", data);
    }

}

const whitelistData = {
    userIDs: []
};