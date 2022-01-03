using MonsterTradingCardsGame.Server.HTTP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server.Handlers.POST
{
    internal class Users : Handler
    {

        struct RequestUsers
        {
            public string Username;
            public string Password;
        }

        RequestUsers requestedUsers;

        public Users(HttpResponse response, HttpRequest request) : base(response, request)
        {
            var streamreader = request.streamReader;
            char[] buffer =  new char[request.ContentLenght];
            
            for(int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (char)streamreader.Read();
            }
            string streambuffer = string.Join("", buffer);
            requestedUsers = JsonConvert.DeserializeObject<RequestUsers>(streambuffer);
        }

        public async override Task Handle()
        {
            var player = await Player.Register(requestedUsers.Username, requestedUsers.Password);

            if(player == null)
            {
                response.status = HttpResponse.STATUS.CONFLICT;
                response.message = new() { { "status", (int)HttpResponse.STATUS.CONFLICT }, { "error", "Username already used!" } };
                return;
            }

            response.status = HttpResponse.STATUS.CREATED;
            response.message = new() { { "status", (int)HttpResponse.STATUS.CREATED }, { "message", "User successfully created!" }, { "token", player.token} };

        }
    }
}
