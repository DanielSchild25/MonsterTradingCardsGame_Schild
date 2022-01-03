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
        TcpListener server;
        private Func<HttpRequest, HttpResponse, Task> callback;
        StreamReader streamReader;
        StreamWriter streamWriter;

        public HttpListener(string ip, int port, Func<HttpRequest, HttpResponse, Task> callback)
        {
            server = new TcpListener(IPAddress.Parse(ip), port);
            this.callback = callback;
        }

        public async Task Start()
        {
            server.Start();
            while(true)
            {
                Console.WriteLine("Waiting for Connection...");
                var client = server.AcceptTcpClient();
                Console.WriteLine("New Client connected");
                using var stream = client.GetStream();
                streamReader = new StreamReader(stream);
                streamWriter = new StreamWriter(stream);
                var request = new HttpRequest(streamReader);
                var response = new HttpResponse(streamWriter);

                if(request.route == "" || request.ContentLenght == -2)
                {
                    response.Send(HttpResponse.STATUS.BAD_REQUEST, new() { { "status", (int)HttpResponse.STATUS.BAD_REQUEST }, { "error", "Bad Request" } });
                    continue;
                }
                if (request.ContentLenght == -3)
                {
                    response.Send(HttpResponse.STATUS.PAYLOAD_TOO_LARGE, new() { { "status", (int)HttpResponse.STATUS.PAYLOAD_TOO_LARGE }, { "error", "Your payload is too large" } });
                    continue;
                }
                if (request.ContentLenght == -1)
                {
                    response.Send(HttpResponse.STATUS.LENGHT_REQUIRED, new() { { "status", (int)HttpResponse.STATUS.LENGHT_REQUIRED }, { "error", "Content-Length header is missing" } });
                    continue;
                }
                await callback(request, response);
                Console.WriteLine(response.status.ToString());
                response.Send();
                stream.Close();
                Console.WriteLine();

            };
            server.Stop();
        }

    }
}
