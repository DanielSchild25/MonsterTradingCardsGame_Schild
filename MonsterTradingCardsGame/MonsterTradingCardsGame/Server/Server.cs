using MonsterTradingCardsGame.Server.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server
{
    internal class Server
    {
        Dictionary<HttpRequest.METHODS, Dictionary<string, Func<HttpRequest, HttpResponse, Handler>>> handler = new();
        public async Task Start()
        {
            RegisterHandlers();
            HttpListener http = new("127.0.0.1", 10001, Handle);
            await http.Start();
        }

        async Task Handle(HttpRequest request, HttpResponse response)
        {
            response.Type = HttpResponse.TYPE.JSON;
            if(request.HttpRoute == "" || !handler.ContainsKey(request.HttpMethod) || !handler[request.HttpMethod].ContainsKey(request.HttpRoute))
            {
                response.status = HttpResponse.STATUS.NOT_FOUND;
                response.Message = new() { { "status", 404 }, { "error", "Route not found" } };
                return;
            }

            response.status = HttpResponse.STATUS.OK;
            await handler[request.HttpMethod][request.HttpRoute](request, response).Handle();
        }

        public void RegisterHandler<T>(HttpRequest.METHODS method, string route) where T : Handler
        {
            if(!handler.ContainsKey(method))
            {
                handler[method] = new Dictionary<string, Func<HttpRequest, HttpResponse, Handler>>();
            }

            handler[method][$"/{route.ToLower()}/"] = (request, response) =>
            {
                Handler handle = (T?)Activator.CreateInstance(typeof(T), new object[] { response, request });
                if (handle == null)
                {
                    throw new NullReferenceException("Something went wrong, unable to create handler");
                }
                return handle;
            };
        }

        void RegisterHandlers()
        {
            RegisterHandler<Handlers.POST.Register>(HttpRequest.METHODS.POST, "users");
            RegisterHandler<Handlers.POST.Login>(HttpRequest.METHODS.POST, "sessions");
            RegisterHandler<Handlers.POST.Packages>(HttpRequest.METHODS.POST, "packages");
            RegisterHandler<Handlers.POST.BuyPackages>(HttpRequest.METHODS.POST, "transactions/packages");
            RegisterHandler<Handlers.GET.GetCards>(HttpRequest.METHODS.GET, "cards");
            RegisterHandler<Handlers.GET.ShowDeck>(HttpRequest.METHODS.GET, "deck");
            RegisterHandler<Handlers.PUT.EditDeck>(HttpRequest.METHODS.PUT, "deck");
            RegisterHandler<Handlers.GET.Users>(HttpRequest.METHODS.GET, "users");
            RegisterHandler<Handlers.PUT.Users>(HttpRequest.METHODS.PUT, "users");
            RegisterHandler<Handlers.POST.Battles>(HttpRequest.METHODS.POST, "battles");
            RegisterHandler<Handlers.GET.Stats>(HttpRequest.METHODS.GET, "stats");
            RegisterHandler<Handlers.GET.Scoreboard>(HttpRequest.METHODS.GET, "score");
        }
                
    }
}
