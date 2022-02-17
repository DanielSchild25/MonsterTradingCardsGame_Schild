using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server.HTTP
{
    class HttpListener
    {
        TcpListener TcpServer;
        private Func<HttpRequest, HttpResponse, Task> callback;
        StreamReader Reader;
        StreamWriter Writer;

        public HttpListener(string ip, int port, Func<HttpRequest, HttpResponse, Task> callback)
        {
            TcpServer = new TcpListener(IPAddress.Parse(ip), port);
            this.callback = callback;
        }

        public async Task Start()
        {
            TcpServer.Start();
            while(true)
            {
                Console.WriteLine("Waiting for Connection...");
                var client = TcpServer.AcceptTcpClient();
                Console.WriteLine("New Client connected");
                using var stream = client.GetStream();
                Reader = new StreamReader(stream);
                Writer = new StreamWriter(stream);
                var request = new HttpRequest(Reader);
                var response = new HttpResponse(Writer);

                /*if(request.HttpMethod != HttpRequest.METHODS.GET )
                {
                    if(request.HttpRoute == "" || request.ContentLength == -1)
                    {
                        response.Send(HttpResponse.STATUS.BAD_REQUEST, new() { { "status", (int)HttpResponse.STATUS.BAD_REQUEST }, { "error", "Bad Request" } });
                        continue;
                    }
                    if (request.ContentLength == -2)
                    {
                        response.Send(HttpResponse.STATUS.PAYLOAD_TOO_LARGE, new() { { "status", (int)HttpResponse.STATUS.PAYLOAD_TOO_LARGE }, { "error", "Your payload is too large" } });
                        continue;
                    }
                    if (request.ContentLength == -3)
                    {
                        response.Send(HttpResponse.STATUS.LENGHT_REQUIRED, new() { { "status", (int)HttpResponse.STATUS.LENGHT_REQUIRED }, { "error", "Content-Length header is missing" } });
                        continue;
                    }
                }*/
                
                await callback(request, response);
                Console.WriteLine(response.status.ToString());
                response.Send();
                stream.Close();
                Console.WriteLine();

            };
            TcpServer.Stop();
        }

    }
}
