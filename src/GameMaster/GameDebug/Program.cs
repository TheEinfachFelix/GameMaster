﻿// See https://aka.ms/new-console-template for more information
using GameMaster;
using GameMaster.Input;
using GameMaster.Output;
using static System.Net.Mime.MediaTypeNames;

if (true)
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

    var a = game.buzzerHandlerList[0];


    a.BuzzerList[1].LEDState = true;
    a.BuzzerList[0].LEDState = true;
    a.LEDListe[0].SetLEDColor(50, 10, 0);
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