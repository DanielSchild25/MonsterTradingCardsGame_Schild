using MonsterTradingCardsGame.Server.HTTP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Cards;

namespace MonsterTradingCardsGame.Server.Handlers.POST
{
    internal class Packages : Handler
    {
        struct PackageRequest
        {
            public string Id;
            public string Name;
            public float Damage;
            /*public ElementType EType;
            public CardType CType;
            public CardGroup Group;*/
        }

        PackageRequest[] requestedPackages;

        public Packages(HttpResponse response ,HttpRequest request) : base(response, request)
        {
            var reader = request.Reader;
            char[] buffer = new char[request.ContentLength];
            for(int i = 0; i < buffer.Length; i++)
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
            if(user.username != "admin")
            {
                response.status = HttpResponse.STATUS.FORBIDDEN;
                response.Message = new() { { "status", (int)HttpResponse.STATUS.FORBIDDEN }, { "message", "Only Admins can create Packages!" } };
                return;
            }

            int index = await Cards.Card.GetPackageID();
            foreach(PackageRequest package in requestedPackages)
            {
                await Cards.Card.Create(package.Id, package.Name, package.Damage, index);
            }
        }
    }
}
