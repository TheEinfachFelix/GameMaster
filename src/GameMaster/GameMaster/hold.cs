using GameMaster;
using GameMaster.Input;
using GameMaster.Output;


namespace WebGameController.Models
{
    public class MainGame
    {
        public Game game;


        public MainGame()
        {
            game = Game.GetInstance();


            List<ILevel> pLevel = [];

            FirstLevel p1 = new();
            FirstLevel p2 = new();

            pLevel.Add(p1);
            pLevel.Add(p2);

            game.Levels = pLevel;

            game.NextLevel();


            var Buzzer = new BuzzerHandler(8,3,12);

            Buzzer.Start(12, 9600);

        }
    }

    public class FirstLevel : ILevel
    {
        dot2Connector dot2 = new();

        public string Name { get; set; } = "Bzzer to .2";
        public string Beschreibung { get; set; } = "Setzt auf BuzzerPress dot2 Values";

        public void BuzzerPress(int BuzzerID)
        {
            dot2.SendButtonPress(101);

            dot2.SetFaderValue(1, 100);
            dot2.SetFaderValue(2, 100);
            dot2.SetFaderValue(3, 100);
        }

        public void BuzzerRelease(int BuzzerID)
        {
            dot2.SetFaderValue(1, 0);
            dot2.SetFaderValue(2, 0);
            dot2.SetFaderValue(3, 0);
        }

        public void Clear()
        {
            dot2.Close();
        }

        public void Setup()
        {
            dot2.Open();
            while (!dot2.Ready) { }
        }
    } 
}
