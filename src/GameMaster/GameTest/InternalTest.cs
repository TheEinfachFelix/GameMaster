using GameMaster;

namespace GameTest
{
    public class InternalTests
    {
        public Game game = Game.GetInstance();

        [SetUp]
        public void Setup()
        {
            game.ResetAll();
        }

        [Test]
        public void TestCreatingPlayer()
        {
            Player p1 = new("Peter");

            Assert.IsNotNull(p1);
            Assert.IsNotNull(p1.Name);
            Assert.IsNotNull(p1.Points);

            Assert.That(p1.Name, Is.EqualTo("Peter"));

            Assert.That(p1.Points, Is.EqualTo(0));
            p1.Points = 10;
            Assert.That(p1.Points, Is.EqualTo(10));
        }
    }
}