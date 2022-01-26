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
                string[] part = request.headers["authorization"].Split(" ");
                if(part.Length == 2)
                {
                    token = part[1];
                }
            }
        }

        public abstract Task Handle();

        public User Authentication()
        {
            User user = User.GetUserToken(token);
            if (user != null) return user;
            response.status = HttpResponse.STATUS.UNAUTHORIZED;
            response.Message = new() { { "status", (int)HttpResponse.STATUS.UNAUTHORIZED }, { "error", "Not Authorized!" } };
            return null;

        }


    }
}
