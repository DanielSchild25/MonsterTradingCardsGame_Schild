using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Cards;
using Npgsql;

namespace MonsterTradingCardsGame
{
    class Player
    {
        public readonly string username;
        string bio;
        string image;
        List<Card> stack;
        public List<Card> deck;
        uint coins;
        uint elo;
        DateTime sessionStart;
        public readonly string token;

        static Dictionary<string, Player> sessions = new ();

        public static Player? GetSession(string token)
        {
            if (!sessions.ContainsKey(token)) return null;
            return sessions[token];
        }

        public Player (string username)
        {
            coins = 20;
            stack = new ();
            deck = new();
            elo = 1000;
            sessionStart = DateTime.Now;
            token = Guid.NewGuid().ToString();
            sessions[token] = this;

            var args = Environment.GetCommandLineArgs();
            if (args.Any(arg => arg.ToLower() == "--curl-test" || (arg[0] == '-' && arg[1] != '-' && arg.Contains('C'))))
            {
                sessions[username + "-mctgToken"] = this;
            }
        }

        Player(Dictionary<string, object> data)
        {
            username = (string)data["username"];
            coins = (uint)data["coins"];
            elo = (uint)data["elo"];
        }

        public async static Task<Player?> Login(string username, string password)
        {
            var user = await Database.self.Read("*", "users", new() { { "username", username } });
            if(user == null || (password != (string)user["passhash"]))
                return null;
            return new Player(username);
        }

        public static async Task<Player?> Register(string username, string password)
        {
            bool success = await Database.self.Write("users", new() { { "username", username }, { "passhash", password } });

            if (!success) return null;

            return new Player(username);
        }
    }
}
