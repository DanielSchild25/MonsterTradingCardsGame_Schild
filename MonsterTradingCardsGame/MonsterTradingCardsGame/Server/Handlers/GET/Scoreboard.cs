using MonsterTradingCardsGame.Server.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server.Handlers.GET
{
    class Scoreboard : Handler
    {
        public Scoreboard(HttpResponse response, HttpRequest request) : base(response, request)
        {

        }

        public async override Task Handle()
        {
            User? user = Authentication();
            if (user == null)
                return;

            Dictionary<string, object> score = await User.GetScoreboard();

            if (score == null)
            {
                response.status = HttpResponse.STATUS.CONFLICT;
                response.Message = new() { { "status", (int)HttpResponse.STATUS.CONFLICT }, { "error", "Something went wrong, pls try again!" } };
                return;
            }

            response.status = HttpResponse.STATUS.OK;
            score = (new Dictionary<string, object> { { "status", (int)HttpResponse.STATUS.OK } }).Concat(score).ToDictionary(k => k.Key, v => v.Value);
            response.Message = score;

        }
    }
}
