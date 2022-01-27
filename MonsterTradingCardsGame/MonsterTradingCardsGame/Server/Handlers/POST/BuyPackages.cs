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

        }
    }
}
