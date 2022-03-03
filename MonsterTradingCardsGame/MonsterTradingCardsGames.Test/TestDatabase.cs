using NUnit.Framework;
using System.Threading.Tasks;
using MonsterTradingCardsGame;
using MonsterTradingCardsGame.Cards;
using System.Collections.Generic;

namespace MonsterTradingCardsGames.Test
{
    class TestDatabase
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task TestRead()
        {
            await Card.Create("test-ID", "TestCard", 10);

            var result = await Database.Base.Read("*", "cards", new() { { "name", "TestCard" } });

            await Database.Base.Delete("cards", new() { { "id", "test-ID" } });

            Assert.IsTrue((string)result["id0"] == "test-ID");
        }

        [Test]
        public async Task TestReadOrder()
        {
            await Card.Create("test-ID1", "TestCard", 10);
            await Card.Create("test-ID2", "TestCard", 20);

            var result = await Database.Base.Read("*", "cards", new() { { "name", "TestCard" } }, false, "damage");

            await Database.Base.Delete("cards", new() { { "id", "test-ID1" } });
            await Database.Base.Delete("cards", new() { { "id", "test-ID2" } });

            Assert.IsTrue((string)result["id0"] == "test-ID2");
        }


        [Test]
        public async Task TestUpdate()
        {
            await Card.Create("test-ID", "TestCard", 10);

            await Database.Base.Update("cards", new() { { "name", "NewName" } }, new() { { "id", "test-ID" } });
            var result = await Database.Base.Read("*", "cards", new() { { "id", "test-ID" } });

            await Database.Base.Delete("cards", new() { { "id", "test-ID" } });

            Assert.IsTrue((string)result["name0"] == "NewName");
        }

        [Test]
        public async Task TestWrite()
        {
            Dictionary<string, object> data = new() { { "id", "TestID" }, { "name", "TestName" }, { "damage", 10 } };

            await Database.Base.Write("cards", data);
            var result = await Database.Base.Read("*", "cards", new() { { "id", "TestID" } });

            await Database.Base.Delete("cards", new() { { "id", "TestID" } });

            Assert.IsTrue((string)result["name0"] == "TestName");
        }

        [Test]
        public async Task TestDelete()
        {
            Dictionary<string, object> data = new() { { "id", "TestID" }, { "name", "TestName" }, { "damage", 10 } };
            await Database.Base.Write("cards", data);

            await Database.Base.Delete("cards", new() { { "id", "TestID" } });
            var result = await Database.Base.Read("*", "cards", new() { { "id", "TestID" } });

            Assert.IsTrue(result == null);
        }
    }
}
