// See https://aka.ms/new-console-template for more information
using GameMaster;
using GameMaster.Input;
using GameMaster.Output;

if (false)
{
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

    a.Start(12, 9600);

    while (true) ;
}
else
{
    dot2Connector dot2 = new();

    dot2.Open();
    while (!dot2.Ready) { }
    
    dot2.SendButtonPress(101);
    dot2.SendButtonPress(103);

    while (true) ;
}