#[M]edieval [I]mmersive [M]ulti User Dungeon    
 
![MIM](http://i.imgur.com/K4YDQKG.png)

A text based MMORPG inspired by the [MUDs of the 90s](https://en.wikipedia.org/wiki/DikuMUD) built in C# using ASP.net MVC, SignalR and MongoDB. Unlike a traditional MUD MIM runs in the browser instead of Telnet. Telnet support could be added so make a pull request :wink:

##Prerequisities
1. The project is using C# 6 so you will need Visual Studio 2015 or add the Roslyn compiler to [Visual Studio 2013](http://stackoverflow.com/a/33848006)

2. Internet connection to connect to mongoDB

##Run the site
With the above steps completed, hitting the start button in Visual Studio should successfully run the application. Your URL should show /Home and you should see the Create Character Wizard.
Once you have created a character you will be in the game. You can log in again using the same character name.

##In game commands
Here are the commands currently supported, all commands are checked using starts with which allows you to not type the whole word.

- Movement using N, E, S, W, U and D
- Get, get all, get <item> <container>
- Drop, drop all, drop <item> <container>
- Look, look at/in, 
- Examine
- Smell
- Touch
- Taste
- Inventory
- Score (not fully implemented)
- Say, ', Say to, >
- Save
- Quit
- Equipment
- Wear
- Wield
- Remove
- Kill
- Flee
- lock, unlock
- open, close


##Creating Rooms, Items and [mobs](https://en.wikipedia.org/wiki/Mob_(video_gaming))

To create a room, items or mobs check the example [room](https://github.com/LiamKenneth/MIM/blob/master/MIMWebClient/Core/World/Anker/Anker.cs)

That should be all, all contributions and feedback welcomed :smile:


