using MonsterTradingCardsGame.Server.HTTP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server.Handlers.PUT
{
    class EditDeck : Handler
    {
        struct DeckRequest
        {
            public string Id1;
            public string Id2;
            public string Id3;
            public string Id4;
        }

        DeckRequest requestedCards;
        public EditDeck(HttpResponse response, HttpRequest request) : base(response, request)
        {
            var reader = request.Reader;
            char[] buffer = new char[request.ContentLength];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (char)reader.Read();
            }
            string streamBuffer = string.Join("", buffer);
            string[] parts = streamBuffer.Split('\"');

            if(parts.Length <= 7)
            {
                requestedCards.Id1 = null;
            }
            else
            {
                requestedCards.Id1 = parts[1];
                requestedCards.Id2 = parts[3];
                requestedCards.Id3 = parts[5];
                requestedCards.Id4 = parts[7];
            }
            
        }
        public async override Task Handle()
        {
            User? user = Authentication();
            if (user == null)
                return;
            if(requestedCards.Id1 != null)
            {
                var success = await Cards.Card.EditDeck(user.username, requestedCards.Id1, requestedCards.Id2, requestedCards.Id3, requestedCards.Id4);

                if (!success)
                {
                    response.status = HttpResponse.STATUS.CONFLICT;
                    response.Message = new() { { "status", (int)HttpResponse.STATUS.CONFLICT }, { "error", "Something went wrong, pls try again!" } };
                    return;
                }

                response.status = HttpResponse.STATUS.OK;
                response.Message = new() { { "status", (int)HttpResponse.STATUS.OK }, { "message", "Deck successfully configured!" } };
            }
            else
            {
                response.status = HttpResponse.STATUS.CONFLICT;
                response.Message = new() { { "status", (int)HttpResponse.STATUS.CONFLICT }, { "error", "Not enough cards!" } };
            }
            

            

        }
    }
}
