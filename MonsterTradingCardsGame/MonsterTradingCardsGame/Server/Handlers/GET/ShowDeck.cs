using MonsterTradingCardsGame.Server.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server.Handlers.GET
{
    class ShowDeck : Handler
    {

        public ShowDeck(HttpResponse response, HttpRequest request) : base(response, request)
        {

        }
        public async override Task Handle()
        {
            User? user = Authentication();
            if (user == null)
                return;

            Dictionary<string, object> result = await Cards.Card.ShowDeck(user.username);

            if(result == null)
            {
                Dictionary<string, object> cards = await Cards.Card.GetCardsId(user.username);
                var success = await Cards.Card.CreateDeck(user.username, (string)cards["id0"], (string)cards["id1"], (string)cards["id2"], (string)cards["id3"]);

                if(!success)
                {
                    response.status = HttpResponse.STATUS.CONFLICT;
                    response.Message = new() { { "status", (int)HttpResponse.STATUS.CONFLICT }, { "error", "Something went wrong, pls try again!" } };
                    return;
                }
                else
                {
                    result = await Cards.Card.ShowDeck(user.username);
                    
                }
                
            }
            response.status = HttpResponse.STATUS.OK;
            //response.Message = new() { { "status", (int)HttpResponse.STATUS.OK },  };
            result = (new Dictionary<string, object> { { "status", (int)HttpResponse.STATUS.OK } }).Concat(result).ToDictionary(k => k.Key, v => v.Value);
            response.Message = result;

        }
    }
}
