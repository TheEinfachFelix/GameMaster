using GameMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTest
{
    internal class GameTests
    {
        public Game game = Game.getInstance();

        [SetUp]
        public void Setup()
        {
            game.Players = new();
            game.Levels = new();
            game.LevelID = new();
            game.CLevel = null;

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
            var a = Game.getInstance();
            var b = Game.getInstance();

            Assert.IsNotNull(a);
            Assert.IsNotNull(b);

            Assert.That(b, Is.EqualTo(a));
        }
        [Test]
        public void TestLevelID() 
        {
            Assert.IsNotNull(game.LevelID);
            Assert.IsTrue(game.LevelID == 0);
            game.LevelID = 1;
            Assert.IsTrue(game.LevelID == 1);
        }
        [Test]
        public void TestCLevel()
        {
            Assert.IsNull(game.CLevel);
        }
    }
}
