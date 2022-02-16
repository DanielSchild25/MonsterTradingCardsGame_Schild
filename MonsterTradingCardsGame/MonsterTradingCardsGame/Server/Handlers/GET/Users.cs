using MonsterTradingCardsGame.Server.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server.Handlers.GET
{
    class Users : Handler
    {
        string RequestedUser;
        public Users(HttpResponse response, HttpRequest request) : base(response, request)
        {
            RequestedUser = request.RequestedUser;
        }

        public async override Task Handle()
        {
            User? user = Authentication();
            if (user == null)
                return;


            if(user.username != RequestedUser)
            {
                response.status = HttpResponse.STATUS.UNAUTHORIZED;
                response.Message = new() { { "status", (int)HttpResponse.STATUS.UNAUTHORIZED }, { "error", "Not Authorized!" } };
                return;
            }

            Dictionary<string, object> result = await User.ShowUserData(user.username);

            response.status = HttpResponse.STATUS.OK;
            response.Message = new() { { "status", (int)HttpResponse.STATUS.OK }, { "Name", result["name0"] }, { "Bio", result["bio0"] }, { "Image", result["image0"] } };



        }
    }
}
