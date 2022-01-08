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
        Dictionary<HttpRequest.METHOD, Dictionary<string, Func<HttpRequest, HttpResponse, Handler>>> handler = new();

        public async Task Start()
        {
            RegisterHandlers();
            HttpListener http = new("127.0.0.1", 10001, Handle);
            await http.Start();
        }

        async Task Handle(HttpRequest request, HttpResponse response)
        {
            response.ContentType = HttpResponse.CONTENT_TYPE.JSON;
            if(request.route == "" || !handler.ContainsKey(request.method) || !handler[request.method].ContainsKey(request.route))
            {
                response.status = HttpResponse.STATUS.NOT_FOUND;
                response.message = new() { { "status", 404 }, { "error", "Route not found" } };
                return;
            }

            response.status = HttpResponse.STATUS.OK;
            await handler[request.method][request.route](request, response).Handle();
        }

        public void RegisterHandler<T>(HttpRequest.METHOD method, string route) where T : Handler
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
            RegisterHandler<Handlers.POST.Users>(HttpRequest.METHOD.POST, "users");
            RegisterHandler<Handlers.POST.Sessions>(HttpRequest.METHOD.POST, "sessions");
            RegisterHandler<Handlers.POST.Packages>(HttpRequest.METHOD.POST, "packages");
        }
    }
}
