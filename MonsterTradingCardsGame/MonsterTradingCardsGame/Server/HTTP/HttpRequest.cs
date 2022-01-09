using System;
using System.Collections.Generic;
using System.IO;

namespace MonsterTradingCardsGame.Server.HTTP
{
    internal class HttpRequest
    {
        public enum METHODS { GET, POST, DELETE, HEAD, OPTIONS, CONNECT, PUT };
        public readonly METHODS HttpMethod;
        public readonly string HttpVersion = "";
        public readonly string HttpRoute = "";
        public readonly StreamReader Reader;
        public readonly Dictionary<string, string> headers = new();

        public int ContentLength
        {
            get
            {
                if (!int.TryParse(this["content-length"], out int lenght)) return -1;
                if (lenght < 0) return -1;
                if (lenght > 1536) return -2;
                if (!headers.ContainsKey("content-length")) return -3;
                return lenght;
            }
        }
        public string this[string keys]
        {
            get
            {
                return this.headers[keys.ToLower()];
            }
            set
            {
                this.headers[keys.ToLower()] = value;
            }
        }
       
        public HttpRequest(StreamReader sReader)
        {
            Reader = sReader;
            if (sReader.EndOfStream) 
                return;
            string row = sReader.ReadLine();
            var part = row.Split(" ");
            if (part.Length != 3) 
                return;
            if (!Enum.TryParse(typeof(METHODS), part[0], out object? method)) 
                return;
            this.HttpMethod = (METHODS)method;
            HttpRoute = part[1];
            if (!HttpRoute.EndsWith("/")) 
                HttpRoute += "/";
            HttpVersion = part[2];
            while (!sReader.EndOfStream && (row = sReader.ReadLine()!) != "")
            {
                int pos = row.IndexOf(":");
                string key = row.Substring(0, pos);
                string value = row.Substring(pos + 1, row.Length - pos - 1).TrimStart();
                Console.WriteLine($"{key}: {value}");
                this[key] = value;
            }
        }
    }   
}