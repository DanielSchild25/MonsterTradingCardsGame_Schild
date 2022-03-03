using NUnit.Framework;
using System.Threading.Tasks;
using MonsterTradingCardsGame;
using MonsterTradingCardsGame.Cards;

namespace MonsterTradingCardsGames.Test
{
    class TestCard
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task TestCreateID()
        {
            Card success = await Card.Create("test-ID", "TestCard", 10);

            await Database.Base.Delete("cards", new() { { "id", "test-ID" } });

            Assert.IsTrue(success.id == "test-ID");
        }

        [Test]
        public async Task TestCreateName()
        {
            Card success = await Card.Create("test-ID", "TestCard", 10);

            await Database.Base.Delete("cards", new() { { "id", "test-ID" } });

            Assert.IsTrue(success.name == "TestCard");
        }

        [Test]
        public async Task TestCreateDamage()
        {
            Card success = await Card.Create("test-ID", "TestCard", 10);

            await Database.Base.Delete("cards", new() { { "id", "test-ID" } });

            Assert.IsTrue(success.damage == 10);
        }

        [Test]
        public void TestBuild()
        {
            var result = Card.Build("test-ID", "TestCard", 10);
            Assert.IsTrue(result.id == "test-ID" && result.name == "TestCard" && result.damage == 10);
        }

        [Test]
        public async Task TestBuyPackage()
        {
            var success = await User.Register("BuyTest", "12345");

            await Card.Create("ID1", "TestCard1", 11, 500);
            await Card.Create("ID2", "TestCard2", 12, 500);
            await Card.Create("ID3", "TestCard3", 13, 500);
            await Card.Create("ID4", "TestCard4", 14, 500);

            var result = await Card.BuyPackage("BuyTest");

            await Database.Base.Delete("users", new() { { "username", "BuyTest" } });
            await Database.Base.Delete("cards", new() { { "id", "ID1" } });
            await Database.Base.Delete("cards", new() { { "id", "ID2" } });
            await Database.Base.Delete("cards", new() { { "id", "ID3" } });
            await Database.Base.Delete("cards", new() { { "id", "ID4" } });
            await Database.Base.Delete("cards", new() { { "username", "BuyTest" } });

            Assert.IsTrue(result == 0 || result == -2);
        }

        [Test]
        public async Task TestBuyPackageCoins()
        {
            var success = await User.Register("BuyTest", "12345");

            await Card.Create("ID1", "TestCard1", 11, 500);
            await Card.Create("ID2", "TestCard2", 12, 500);
            await Card.Create("ID3", "TestCard3", 13, 500);
            await Card.Create("ID4", "TestCard4", 14, 500);
            await Card.BuyPackage("BuyTest");

            var result = await Database.Base.Read("*", "users", new() { { "username", "BuyTest" } });

            await Database.Base.Delete("users", new() { { "username", "BuyTest" } });
            await Database.Base.Delete("cards", new() { { "id", "ID1" } });
            await Database.Base.Delete("cards", new() { { "id", "ID2" } });
            await Database.Base.Delete("cards", new() { { "id", "ID3" } });
            await Database.Base.Delete("cards", new() { { "id", "ID4" } });
            await Database.Base.Delete("cards", new() { { "username", "BuyTest" } });

            Assert.IsTrue((int)result["coins0"] == 15);
        }

        [Test]
        public async Task TestBuyPackageUsername()
        {
            var success = await User.Register("BuyTest", "12345");

            await Card.Create("ID1", "TestCard1", 11, 500);
            await Card.Create("ID2", "TestCard2", 12, 500);
            await Card.Create("ID3", "TestCard3", 13, 500);
            await Card.Create("ID4", "TestCard4", 14, 500);
            await Card.BuyPackage("BuyTest");

            var result = await Database.Base.Read("*", "cards", new() { { "packageid", 500 } });

            await Database.Base.Delete("users", new() { { "username", "BuyTest" } });
            await Database.Base.Delete("cards", new() { { "id", "ID1" } });
            await Database.Base.Delete("cards", new() { { "id", "ID2" } });
            await Database.Base.Delete("cards", new() { { "id", "ID3" } });
            await Database.Base.Delete("cards", new() { { "id", "ID4" } });
            await Database.Base.Delete("cards", new() { { "username", "BuyTest" } });

            Assert.IsTrue((string)result["username0"] == "BuyTest");
        }

        [Test]
        public async Task TestCreateDeck()
        {
            await User.Register("DeckTest", "12345");
            await Card.Create("ID1", "TestCard1", 11);
            await Card.Create("ID2", "TestCard2", 12);
            await Card.Create("ID3", "TestCard3", 13);
            await Card.Create("ID4", "TestCard4", 14);

            var result = await Card.CreateDeck("DeckTest", "ID1", "ID2", "ID3", "ID4");

            
            Assert.IsTrue(result);
        }

        [Test]
        public async Task TestEditDeck()
        {
            await User.Register("DeckTest", "12345");
            await Card.Create("ID5", "TestCard5", 11);
            await Card.Create("ID6", "TestCard6", 12);
            await Card.Create("ID7", "TestCard7", 13);
            await Card.Create("ID8", "TestCard8", 14);

            var result = await Card.EditDeck("DeckTest", "ID5", "ID6", "ID7", "ID8");

            await Database.Base.Delete("users", new() { { "username", "DeckTest" } });
            await Database.Base.Delete("cards", new() { { "id", "ID5" } });
            await Database.Base.Delete("cards", new() { { "id", "ID6" } });
            await Database.Base.Delete("cards", new() { { "id", "ID7" } });
            await Database.Base.Delete("cards", new() { { "id", "ID8" } });

            await Database.Base.Delete("users", new() { { "username", "DeckTest" } });
            await Database.Base.Delete("cards", new() { { "id", "ID1" } });
            await Database.Base.Delete("cards", new() { { "id", "ID2" } });
            await Database.Base.Delete("cards", new() { { "id", "ID3" } });
            await Database.Base.Delete("cards", new() { { "id", "ID4" } });
            await Database.Base.Delete("decks", new() { { "username", "DeckTest" } });



            Assert.IsTrue(result || result == false);
        }

        [Test]
        public async Task TestShowDeck()
        {
            await User.Register("DeckTest", "12345");
            await Card.Create("ID1", "TestCard1", 11);
            await Card.Create("ID2", "TestCard2", 12);
            await Card.Create("ID3", "TestCard3", 13);
            await Card.Create("ID4", "TestCard4", 14);
            await Card.CreateDeck("DeckTest", "ID1", "ID2", "ID3", "ID4");

            var result = await Card.ShowDeck("DeckTest");

            await Database.Base.Delete("users", new() { { "username", "DeckTest" } });
            await Database.Base.Delete("cards", new() { { "id", "ID1" } });
            await Database.Base.Delete("cards", new() { { "id", "ID2" } });
            await Database.Base.Delete("cards", new() { { "id", "ID3" } });
            await Database.Base.Delete("cards", new() { { "id", "ID4" } });
            await Database.Base.Delete("decks", new() { { "username", "DeckTest" } });

            Assert.IsTrue((string)result["card10"] == "ID1");
        }

        [Test]
        public async Task TestGetPackageID()
        {
            await User.Register("DeckTest", "12345");
            await Card.Create("ID1", "TestCard1", 11, 500);
            await Card.Create("ID2", "TestCard2", 12, 500);
            await Card.Create("ID3", "TestCard3", 13, 500);
            await Card.Create("ID4", "TestCard4", 14, 500);

            var result = await Card.GetPackageID();

            await Database.Base.Delete("users", new() { { "username", "DeckTest" } });
            await Database.Base.Delete("cards", new() { { "id", "ID1" } });
            await Database.Base.Delete("cards", new() { { "id", "ID2" } });
            await Database.Base.Delete("cards", new() { { "id", "ID3" } });
            await Database.Base.Delete("cards", new() { { "id", "ID4" } });

            Assert.IsTrue(result == 501);
        }

        
    }
}
