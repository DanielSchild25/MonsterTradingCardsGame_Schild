using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Server.HTTP
{
    internal class HttpRequest
    {
        public enum METHOD { GET, POST, DELETE, HEAD, OPTIONS, CONNECT, PUT, PATCH, TRACE };
        public readonly METHOD method;
        public readonly string version = "";
        public readonly string route = "";
        public readonly StreamReader streamReader;
        public readonly Dictionary<string, string> headers = new();


        public string this[string key]
        {
            get
            {
                return headers[key.ToLower()];
            }
            set
            {
                headers[key.ToLower()] = value;
            }
        }
       

        public int ContentLenght
        {
            get
            {
                if (!headers.ContainsKey("content-lenght")) return -1;
                if (!int.TryParse(this["content-length"], out int lenght)) return -2;
                if (lenght < 0) return -2;
                if (lenght > 1024) return -3;
                return lenght;
            }
        }

        public HttpRequest(StreamReader streamR)
        {
            streamReader = streamR;
            if (streamR.EndOfStream) return;
            string line = streamR.ReadLine();
            var parts = line.Split(" ");
            if (parts.Length != 3) return;
            if (!Enum.TryParse(typeof(METHOD), parts[0], out object? method)) return;
            this.method = (METHOD)method;
            route = parts[1];
            if (!route.EndsWith("/")) route += "/";
            version = parts[2];
            while (!streamR.EndOfStream && (line = streamR.ReadLine()!) != "")
            {
                int positon = line.IndexOf(":");
                string key = line.Substring(0, positon);
                string value = line.Substring(positon + 1, line.Length - positon - 1).TrimStart();
                Console.WriteLine($"{key}: {value}");
                this[key] = value;
            }
        }
    }   
}