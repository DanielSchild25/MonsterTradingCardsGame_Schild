using MonsterTradingCardsGame.Server.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server.Handlers.POST
{
    class Battles : Handler
    {
        public string player1 = null;
        public string player2 = null;
        public Battles(HttpResponse response, HttpRequest request) : base(response, request)
        {

        }

        public async override Task Handle()
        {
            User? user = Authentication();
            if (user == null)
                return;
            AddPlayer(user.username);

            if (player1 != null && player2 != null)
            {
                string result = await BattleLogic.Battle(player1, player2);
            }



        }

        public void AddPlayer(string username)
        {
            if (player1 == null && player2 == null)
                player1 = username;
            else if (player2 == null)
                player2 = username;
        }
    }
}
