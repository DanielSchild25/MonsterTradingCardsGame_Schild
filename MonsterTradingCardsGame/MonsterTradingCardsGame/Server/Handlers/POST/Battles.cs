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
        public Battles(HttpResponse response, HttpRequest request) : base(response, request)
        {

        }

        public async override Task Handle()
        {
            User? user = Authentication();
            if (user == null)
                return;

            var result = await Database.Base.Read("*", "battle");

            if(result == null)
            {
                bool success = await Database.Base.Write("battle", new() { { "player", user.username } });
                response.status = HttpResponse.STATUS.OK;
                response.Message = new() { { "status", (int)HttpResponse.STATUS.OK }, { "message", $"{user.username} is waiting for apponent!" } };
            }
            else
            {
                string BattleResult = await BattleLogic.Battle((string)result["player0"], user.username);
                bool success = await Database.Base.Delete("battle");
                response.status = HttpResponse.STATUS.OK;
                response.Message = new() { { "status", (int)HttpResponse.STATUS.OK }, { "message", BattleResult } };
            }

        }

    }
}
