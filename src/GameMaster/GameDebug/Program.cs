// See https://aka.ms/new-console-template for more information
using GameMaster;
using GameMaster.Input;
using GameMaster.Output;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;



if (true)
{
    AudioPlayerSegment seg = new("C:\\Users\\felix\\Downloads\\bad guy.mp3",60);

    async void play()
    {
        await Task.Delay(1000);
        seg.PlaySound(1000);
    }

    Console.WriteLine("Init Done");

    seg.PlaySound(100);

    seg.ProcessCompleted += play;




    Console.WriteLine("Hello, World!");

    while (true) ;
}

if (false)
{
    Game game = Game.GetInstance();

    List<ILevel> pLevel = [];

    TestLevel p1 = new();
    TestLevel p2 = new();

    pLevel.Add(p1);
    pLevel.Add(p2);


    game.Levels = pLevel;
    game.LevelID = 0;

    game.Setup();


    Console.WriteLine("Hello, World!");

    var a = game.BuzzerControll.BuzzerControllerList[0];


    //a.BuzzerList[1].LEDState = true;
    //a.BuzzerList[0].LEDState = true;
    //a.LEDListe[0].SetLEDColor(50, 10, 0);
    while (true) ;
}
else if(false)
{
    Game game = Game.GetInstance();
    dot2Connector dot2 = game.dot2ConnectorList[0];

    dot2.Open();
    while (!dot2.Ready) { }

    //dot2.SendButtonPress(101);
    //dot2.SendButtonPress(103);

    //dot2.SetBlackOut(false);

    dot2.SetFaderValue(1, 75);
    dot2.SetFaderValue(2, 100);
    dot2.SetFaderValue(3, 50);

    while (true) ;
}