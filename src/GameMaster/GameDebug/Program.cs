// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


List<GameMaster.IPlayer> pPlayer = new List<GameMaster.IPlayer>();

GameMaster.Player p1 = new("Peter");
GameMaster.Player p2 = new("simone");


pPlayer.Add(p1);
pPlayer.Add(p2);

//a.Players =  pPlayer;

