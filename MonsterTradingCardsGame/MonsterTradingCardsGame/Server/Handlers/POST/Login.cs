using MonsterTradingCardsGame.Server.HTTP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server.Handlers.POST
{
    class Login : Handler
    {
        struct UserRequest
        {
            public string Username;
            public string Password;
        }

        UserRequest requestedUsers;

        public Login(HttpResponse response, HttpRequest request) : base(response, request)
        {
            var reader = request.Reader;
            char[] buffer = new char[request.ContentLength];

            for(int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (char)reader.Read();
            }

            string stringBuffer = string.Join("", buffer);
            requestedUsers = JsonConvert.DeserializeObject<UserRequest>(stringBuffer);
        }

        public async override Task Handle()
        {
            var user = await User.Login(requestedUsers.Username, requestedUsers.Password);

            if(user == null)
            {
                response.status = HttpResponse.STATUS.UNAUTHORIZED;
                response.Message = new() { { "status", (int)HttpResponse.STATUS.UNAUTHORIZED }, { "error", "Username and/or Password is not correct!" } };
                return;
            }

            response.status = HttpResponse.STATUS.OK;
            response.Message = new() { { "status", (int)HttpResponse.STATUS.OK }, { "message", "User successfully logged in!" }, { "token", user.token } };
        }
    }
}
