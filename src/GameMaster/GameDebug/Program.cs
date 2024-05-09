// See https://aka.ms/new-console-template for more information
using GameMaster;
using GameMaster.Input;


Game game = Game.GetInstance();

List<ILevel> pLevel = [];

TestLevel p1 = new();
TestLevel p2 = new();

pLevel.Add(p1);
pLevel.Add(p2);

game.Levels = pLevel;

game.NextLevel();


Console.WriteLine("Hello, World!");

var a = new TestBuzzer();

a.Start(12,9600);

while (true) ;