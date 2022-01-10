using MonsterTradingCardsGame.Server.HTTP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server.Handlers.POST
{
    internal class Register : Handler
    {

        struct UserRequest
        {
            public string Username;
            public string Password;
        }

        UserRequest requestedUsers;

        public Register(HttpResponse response, HttpRequest request) : base(response, request)
        {
            var reader = request.Reader;
            char[] buffer =  new char[request.ContentLength];
            
            for(int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (char)reader.Read();
            }
            string streamBuffer = string.Join("", buffer);
            requestedUsers = JsonConvert.DeserializeObject<UserRequest>(streamBuffer);
        }

        public async override Task Handle()
        {
            var user = await User.Register(requestedUsers.Username, requestedUsers.Password);

            if(user == null)
            {
                response.status = HttpResponse.STATUS.CONFLICT;
                response.Message = new() { { "status", (int)HttpResponse.STATUS.CONFLICT }, { "error", "Username is already taken! Choose another Username!" } };
                return;
            }

            response.status = HttpResponse.STATUS.CREATED;
            response.Message = new() { { "status", (int)HttpResponse.STATUS.CREATED }, { "message", "User successfully registered!" }, { "token", user.token} };

        }
    }
}
