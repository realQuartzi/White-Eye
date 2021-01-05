# WhiteEye

WhiteEye is a Discord Whitelisting bot.  
Its functionality is to remove users from the server when they first join if they are not whitelisted.

**How does the whitelisting work?**  
How the bot does this whitelisting is through a json file which collects the Unique IDs of the user into a list which the bot will read and compare with the user that just joined. If the UserID does not match up with any ID in the json file then the user will simply be kicked from the server.

**How is the bot able to differentiate from different servers?**  
The bots json file name is named after the server's unique ID and therefore can easily be specifically targeted.

<hr>

## WhiteEye-CS

Only users with these permissions are able to use the upcoming commands:

- Kick Members
- Ban Members



**[Commands] (argument) || Context** 

<u>!wl-add (userID)</u> || This will add the given userID into the json file for that server.

<u>!wl-remove (userID)</u> || This will remove the given userID from the json file for that server.



**Registered Events**

<u>UserJoin</u> || When a User Joins the server the bot will check if the user is whitelisted and remove them accordingly.

<u>UserKick</u> || When a User is kicked they will automatically be removed from the whitelist.

<u>UserBan</u> || When a User is banned they will automatically be removed from the whitelist.



<hr>

## WhiteEye-JS

Only users with these permissions are able to use the upcoming commands:

- Kick Members
- Ban Members



**[Commands] (argument) || Context** 

<u>!wl-add (userID)</u> || This will add the given userID into the json file for that server.

<u>!wl-remove (userID)</u> || This will remove the given userID from the json file for that server.



**Registered Events**

<u>UserJoin</u> || When a User Joins the server the bot will check if the user is whitelisted and remove them accordingly.
