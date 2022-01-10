using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Cards;
using Npgsql;

namespace MonsterTradingCardsGame
{
    class User
    {
        public readonly string username;
        List<Card> stack;
        public List<Card> deck;
        int coins;
        int elo;
        DateTime sessionStart;
        public readonly string token;

        static Dictionary<string, User> UserTokens = new ();

        public static User? GetUserToken(string token)
        {
            if (!UserTokens.ContainsKey(token))
                return null;
            return UserTokens[token];
        }

        public User (string username)
        {
            coins = 20;
            stack = new ();
            deck = new();
            elo = 1000;
            sessionStart = DateTime.Now;
            token = Guid.NewGuid().ToString();
            UserTokens[token] = this;

        }

        User(Dictionary<string, object> userData)
        {
            username = (string)userData["username"];
            coins = (int)userData["coins"];
            elo = (int)userData["elo"];
        }

        public async static Task<User?> Login(string username, string password)
        {
            var user = await Database.Base.Read("*", "users", new() { { "username", username } });
            if(user == null || (password != (string)user["passhash"]))
                return null;
            return new User(username);
        }

        public static async Task<User?> Register(string username, string password)
        {
            bool success = await Database.Base.Write("users", new() { { "username", username }, { "passhash", password } });
            if (!success)
                return null;
            return new User(username);
        }
    }
}
