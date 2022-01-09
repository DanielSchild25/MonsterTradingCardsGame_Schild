using MonsterTradingCardsGame.Server.HTTP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server.Handlers.POST
{
    class Sessions : Handler
    {
        struct RequestUsers
        {
            public string Username;
            public string Password;
        }

        RequestUsers reqUsers;

        public Sessions(HttpResponse response, HttpRequest request) : base(response, request)
        {
            var sr = request.Reader;
            char[] buffer = new char[request.ContentLength];

            for(int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (char)sr.Read();
            }

            string sbuffer = string.Join("", buffer);
            reqUsers = JsonConvert.DeserializeObject<RequestUsers>(sbuffer);
        }

        public async override Task Handle()
        {
            var player = await Player.Login(reqUsers.Username, reqUsers.Password);

            if(player == null)
            {
                response.status = HttpResponse.STATUS.UNAUTHORIZED;
                response.Message = new() { { "status", (int)HttpResponse.STATUS.UNAUTHORIZED }, { "error", "No player is matching username and password!" } };
                return;
            }

            response.status = HttpResponse.STATUS.OK;
            response.Message = new() { { "status", (int)HttpResponse.STATUS.OK }, { "message", "User logged in successfully!" }, { "token", player.token } };
        }
    }
}
