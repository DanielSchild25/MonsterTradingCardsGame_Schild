using MonsterTradingCardsGame.Server.HTTP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server.Handlers.GET
{
    class GetCards :  Handler
    {
        struct CardsRequest
        {
            public string Id;
        }

        CardsRequest[] requestedCards;
        public GetCards(HttpResponse response, HttpRequest request) : base(response, request)
        {
            
        }

        public async override Task Handle()
        {
            User? user = Authentication();
            if (user == null)
                return;

            Dictionary<string, object> result = await Cards.Card.GetCardsId(user.username);

            if(result == null)
            {
                response.status = HttpResponse.STATUS.CONFLICT;
                response.Message = new() { { "status", (int)HttpResponse.STATUS.CONFLICT }, { "error", "Something went wrong, pls try again!" } };
            }

            response.status = HttpResponse.STATUS.OK;
            //response.Message = new() { { "status", (int)HttpResponse.STATUS.OK },  };
            result = (new Dictionary<string, object> { { "status", (int)HttpResponse.STATUS.OK } }).Concat(result).ToDictionary(k => k.Key, v => v.Value);
            response.Message = result;

        }
    }
}
