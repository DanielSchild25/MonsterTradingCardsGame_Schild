using MonsterTradingCardsGame.Server.HTTP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server.Handlers.PUT
{
    class Users : Handler
    {
        struct UserDataRequest
        {
            public string name;
            public string bio;
            public string image;
            
        }

        UserDataRequest requestedData;
        string RequestedUser;
        public Users(HttpResponse response, HttpRequest request) : base(response, request)
        {
            RequestedUser = request.RequestedUser;
            var reader = request.Reader;
            char[] buffer = new char[request.ContentLength];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (char)reader.Read();
            }
            string streamBuffer = string.Join("", buffer);
            string[] parts = streamBuffer.Split('\"');
            requestedData.name = parts[3];
            requestedData.bio = parts[7];
            requestedData.image = parts[11];

        }
        public async override Task Handle()
        {
            User? user = Authentication();
            if (user == null)
                return;

            if (user.username != RequestedUser)
            {
                response.status = HttpResponse.STATUS.UNAUTHORIZED;
                response.Message = new() { { "status", (int)HttpResponse.STATUS.UNAUTHORIZED }, { "error", "Not Authorized!" } };
                return;
            }
            
            bool success = await User.EditUserData(user.username, requestedData.name, requestedData.bio, requestedData.image);

            if (!success)
            {
                response.status = HttpResponse.STATUS.CONFLICT;
                response.Message = new() { { "status", (int)HttpResponse.STATUS.CONFLICT }, { "error", "Something went wrong, pls try again!" } };
                return;
            }

            response.status = HttpResponse.STATUS.OK;
            response.Message = new() { { "status", (int)HttpResponse.STATUS.OK }, { "message", "User Data successfully edited!" } };

        }
    }
    
}
