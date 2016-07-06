#[M]edieval [I]mmersive [M]ulti User Dungeon    
A text based MMORPG inspired by the [MUDs of the 90s](https://en.wikipedia.org/wiki/DikuMUD) built in C# using ASP.net MVC, SignalR and MongoDB. Unlike a traditional MUD MIM runs in the browser instead of Telnet. Telnet support could be added so make a pull request :wink: 

##Prerequisities
1. The project is using C# 6 so you will need Visual Studio 2015 or add the Roslyn compiler to [Visual Studio 2013](http://stackoverflow.com/a/33848006)

2. Create a Mongo database called: MIMDB

3. Add these collections to MIMDB:

- Item
- Mob
- Player
- Room

You need atleast one room for the site to run correctly. Copy the contents from room.json found in the Database folder and put it in the Room collections. Using a Mongo Client such as [RoboMongo](https://robomongo.org) makes this easier.

##Run the site
With the above steps completed, hitting the start button in Visual Studio should successfully run the application. Your URL should show /Home and you should see the Create Character Wizard.
Once you have created a character you will be in the game. You can log in again using the same character name.

##In game commands
Here are the commands currently supported, all commands are checked using starts with which allows you to not type the whole word.

- Movement using N, E, S, W, U and D
- Get, get all
- Drop, drop all
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


##Creating Rooms, Items and [mobs](https://en.wikipedia.org/wiki/Mob_(video_gaming))

Currently the only way to add rooms to the game is to edit the Room collection and manually add them.  Work is in progress to make building rooms easier by doing it online. You can find it here /Room/Create. 

That should be all, all contributions and feedback welcomed :smile:

