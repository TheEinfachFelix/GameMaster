using GameMaster;

namespace GameTest
{
    internal class GameTests
    {
        public Game game = Game.GetInstance();

        [SetUp]
        public void Setup()
        {
            game.ResetAll();
        }

        [Test]
        public void TestAddingPlayerToGame()
        {
            List<IPlayer> pPlayer = [];

            Player p1 = new("Peter");
            Player p2 = new("simone");

            pPlayer.Add(p1);
            pPlayer.Add(p2);

            game.Players = pPlayer;

            Assert.IsTrue(game.Players.Count == 2);
            Assert.IsTrue(game.Players[0].Name == "Peter");
        }
        [Test]
        public void TestRemovePlayerFromGame()
        {
            TestAddingPlayerToGame();

            Assert.NotNull(game.Players);

            game.Players.RemoveAt(0);

            Assert.IsTrue(game.Players.Count == 1);
            Assert.IsTrue(game.Players[0].Name == "simone");
        }
        [Test]
        public void TestAddingLevelToGame()
        {
            List<ILevel> pLevel = [];

            TestLevel p1 = new();
            TestLevel p2 = new();

            pLevel.Add(p1);
            pLevel.Add(p2);

            game.Levels = pLevel;

            Assert.IsTrue(game.Levels.Count == 2);
        }
        [Test]
        public void TestRemoveLevelFromGame()
        {
            TestAddingLevelToGame();

            Assert.NotNull(game.Levels);

            game.Levels.RemoveAt(0);

            Assert.IsTrue(game.Levels.Count == 1);
        }
        [Test]
        public void TestGameGetInstance()
        {
            var a = Game.GetInstance();
            var b = Game.GetInstance();

            Assert.IsNotNull(a);
            Assert.IsNotNull(b);

            Assert.That(b, Is.EqualTo(a));
        }
        [Test]
        public void TestLevelID() 
        {
            TestAddingLevelToGame();

            Assert.That(game.Levels, Is.Not.Null);

            Assert.IsNotNull(game.LevelID);
            Assert.That(game.LevelID == 0, Is.True);
            game.LevelID = 1;

            Assert.Multiple(() =>
            {
                Assert.That(game.CLevel, Is.Not.Null);

                Assert.That(game.LevelID, Is.EqualTo(1));

                Assert.That(game.CLevel, Is.EqualTo(game.Levels[1]));
            });

            try
            {
                game.LevelID = 10;
            }
            catch { }
            Assert.That(game.LevelID == 0, Is.True);

        }
        [Test]
        public void TestCLevel()
        {
            Assert.IsNull(game.CLevel);
        }
        [Test]
        public void TestGameNextLevel() 
        {
            // setup
            TestAddingLevelToGame();

            Assert.That(game.Levels, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(game.CLevel, Is.Null);

                // check if nextlevel worx
                Assert.That(game.NextLevel(), Is.True);

                Assert.That(game.CLevel, Is.Not.Null);

                Assert.That(game.LevelID, Is.EqualTo(0));

                Assert.That(game.CLevel, Is.EqualTo(game.Levels[0]));

                // check argain
                Assert.That(game.NextLevel(), Is.True);

                Assert.That(game.CLevel, Is.Not.Null);

                Assert.That(game.LevelID, Is.EqualTo(1));

                Assert.That(game.CLevel, Is.EqualTo(game.Levels[1]));

                // Check End of level behavior
                Assert.That(game.NextLevel(), Is.False);
            });

        }
    }
}


// RestetAll
