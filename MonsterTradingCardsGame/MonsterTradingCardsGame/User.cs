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
            this.username = username;
            coins = 20;
            stack = new ();
            deck = new();
            elo = 1000;
            sessionStart = DateTime.Now;
            token = Guid.NewGuid().ToString();
            UserTokens[token] = this;
            UserTokens[username + "-mtcgToken"] = this;

            var args = Environment.GetCommandLineArgs();
            if (args.Any(arg => arg.ToLower() == "--curl-test" || (arg[0] == '-' && arg[1] != '-' && arg.Contains('C'))))
            {
                UserTokens[username + "-mtcgToken"] = this;
            }

        }

        User(Dictionary<string, object> userData)
        {
            username = (string)userData["username"];
            coins = (int)userData["coins"];
            elo = (int)userData["elo"];
            //bio = (string)data["bio"];
            //image = (string)data["image"];
        }

        public async static Task<User?> Login(string username, string password)
        {
            var user = await Database.Base.Read("*", "users", new() { { "username", username } });
            if(user == null || (password != (string)user["password"]))
                return null;
            return new User(username);
        }

        public static async Task<User?> Register(string username, string password)
        {
            bool success = await Database.Base.Write("users", new() { { "username", username }, { "password", password } ,{ "coins", 20 } });
            if (!success)
                return null;
            return new User(username);
        }
    }
}
