using MonsterTradingCardsGame.Server.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server.Handlers.GET
{
    class Stats : Handler
    {
        public Stats(HttpResponse response, HttpRequest request) : base(response, request)
        {

        }

        public async override Task Handle()
        {
            User? user = Authentication();
            if (user == null)
                return;

            Dictionary<string, object> stats = await User.GetStats(user.username);

            if(stats == null)
            {
                response.status = HttpResponse.STATUS.CONFLICT;
                response.Message = new() { { "status", (int)HttpResponse.STATUS.CONFLICT }, { "error", "Something went wrong, pls try again!" } };
                return;
            }

            response.status = HttpResponse.STATUS.OK;
            response.Message = new() { { "status", (int)HttpResponse.STATUS.OK }, { "Username", stats["username0"] }, { "Elo", stats["elo0"] }, { "Wins", stats["wins0"] }, { "Loses", stats["loses0"] }, { "Draws", stats["draws0"] } };

        }
    }
}
