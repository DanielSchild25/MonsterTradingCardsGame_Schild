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
        struct RequestPackages
        {
            public string Id;
            public string Name;
            public float Damage;
            public ElementType EType;
            public CardType CType;
            public CardGroup Group;
        }

        RequestPackages[] reqPackages;

        public Packages(HttpResponse response ,HttpRequest request) : base(response, request)
        {
            var sr = request.Reader;
            char[] buffer = new char[request.ContentLength];
            for(int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (char)sr.Read();
            }
            string sbuffer = string.Join("", buffer);
            reqPackages = JsonConvert.DeserializeObject<RequestPackages[]>(sbuffer)!;
        }

        public async override Task Handle()
        {
            Player? player = Authentication();
            if (player == null) return;
            if(player.username != "admin")
            {
                response.status = HttpResponse.STATUS.FORBIDDEN;
                response.Message = new() { { "status", (int)HttpResponse.STATUS.FORBIDDEN }, { "message", "Only the admin is authorized to create packages!" } };
                return;
            }

            int index = await Cards.Card.GetPackageID();
            foreach(RequestPackages package in reqPackages)
            {
                await Cards.Card.Create(package.Id, package.Name, package.Damage, package.EType, package.CType, package.Group, index);
            }
        }
    }
}
