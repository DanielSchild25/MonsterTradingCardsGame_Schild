using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Server.HTTP;
using MonsterTradingCardsGame.Server.Handlers;

namespace MonsterTradingCardsGame.Server
{
    internal abstract class Handler
    {
        protected HttpRequest request;
        protected HttpResponse response;
        string token = "";

        public Handler(HttpResponse response, HttpRequest request)
        {
            this.request = request;
            this.response = response;
            
            if(request.headers.ContainsKey("authorization"))
            {
                string[] split = request.headers["authorization"].Split("");
                if(split.Length == 2)
                {
                    token = split[1];
                }
            }
        }

        public abstract Task Handle();

        public Player? Authentication()
        {
            Player? player = Player.GetSession(token);
            if (player != null) return player;
            response.status = HttpResponse.STATUS.UNAUTHORIZED;
            response.Message = new() { { "status", (int)HttpResponse.STATUS.UNAUTHORIZED }, { "error", "Not Authorized!" } };
            return null;

        }


    }
}
