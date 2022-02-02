using MonsterTradingCardsGame.Server.HTTP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server.Handlers.POST
{
    class BuyPackages : Handler
    {
        struct PackageRequest
        {
            public string Id;
            public string Name;
            public float Damage;
        }

        PackageRequest[] requestedPackages;
        public BuyPackages(HttpResponse response, HttpRequest request) : base(response, request)
        {
            var reader = request.Reader;
            char[] buffer = new char[request.ContentLength];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (char)reader.Read();
            }
            string streamBuffer = string.Join("", buffer);
            requestedPackages = JsonConvert.DeserializeObject<PackageRequest[]>(streamBuffer)!;
        }


        public async override Task Handle()
        {
            User? user = Authentication();
            if (user == null)
                return;
            int result = await Cards.Card.BuyPackage(user.username);
            switch(result)
            {
                case 0:
                    response.status = HttpResponse.STATUS.OK;
                    response.Message = new() { { "status", (int)HttpResponse.STATUS.OK }, { "message", "Package successfully bought!" } };
                    break;
                case -1:
                    response.status = HttpResponse.STATUS.CONFLICT;
                    response.Message = new() { { "status", (int)HttpResponse.STATUS.CONFLICT }, { "error", "Something went wrong, pls try again!" } };
                    break;
                case -2:
                    response.status = HttpResponse.STATUS.CONFLICT;
                    response.Message = new() { { "status", (int)HttpResponse.STATUS.CONFLICT }, { "error", "User doesn't have enough coins!" } };
                    break;
            }
            

        }
    }
}
