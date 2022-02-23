
using MonsterTradingCardsGame;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonsterTradingCardsGames.Test
{
    public class TestUser
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public  async Task TestRegister()
        {
            var success = await User.Register("Testuser", "12345");

            await Database.Base.Delete("users", new() { { "username", "Testuser" } });

            Assert.IsTrue(success != null);
        }

        [Test]
        public async Task TestRegisterCoins()
        {
            await User.Register("Testuser", "12345");

            var result = await Database.Base.Read("*", "users", new() { { "username", "Testuser" } });

            await Database.Base.Delete("users", new() { { "username", "Testuser" } });

            Assert.IsTrue((int)result["coins0"] == 20);
        }

        [Test]
        public async Task TestLogin()
        {
            await User.Register("Testuser", "12345");

            var success = await User.Login("Testuser", "12345");

            await Database.Base.Delete("users", new() { { "username", "Testuser" } });

            Assert.IsTrue(success != null);
        }

        [Test]
        public async Task TestEditUserData()
        {
            await User.Register("Testuser", "12345");

            bool success = await User.EditUserData("Testuser", "Test", "Dies ist ein Testuser", ":-)");

            await Database.Base.Delete("users", new() { { "username", "Testuser" } });

            Assert.IsTrue(success);
        }

        [Test]
        public async Task TestShowUserData()
        {
            await User.Register("Testuser", "12345");
            await User.EditUserData("Testuser", "Test", "Dies ist ein Testuser", ":-)");

            Dictionary<string, object> result = await User.ShowUserData("Testuser");

            await Database.Base.Delete("users", new() { { "username", "Testuser" } });

            Assert.AreEqual(result["bio0"], "Dies ist ein Testuser");
        }
    }
}