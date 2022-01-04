using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MonsterTradingCardsGame
{
    class Player
    {
        public readonly string username;
        string bio;
        string image;
        List<Card> cards;
        List<Card> deck;
        uint coins;
        uint elo;
        DateTime sessionStart;
        public readonly string token;

        static Dictionary<string, Player> sessions = new();

        public static Player? GetSession(string token)
        {
            if (!sessions.ContainsKey(token)) return null;
            return sessions[token];
        }

        Player (string username)
        {
            coins = 20;
            cards = new();
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
            coins = (uint)data["coins"];
            elo = (uint)data["elo"];
            bio = (string)data["bio"];
            image = (string)data["image"];
        }

        public async static Task<Player?> Login(string username, string password)
        {
            var user = await Database.self.Read("*", "player", new() { { "username", username } });
            if(user == null || !Password.Check(password, (string)user["password"]))
            {
                return null;
            }
            return new Player(username);
        }

        public static async Task<Player?> Register(string username, string password)
        {
            bool success = await Database.self.Write("player", new() { { "username", username }, { "password", password.GetHashCode(password) } });

            if (!success) return null;

            return new Player(username);
        }
    }
}
