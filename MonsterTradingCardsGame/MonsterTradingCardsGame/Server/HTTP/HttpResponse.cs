using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MonsterTradingCardsGame.Server.HTTP
{
    internal class HttpResponse
    {
        public enum STATUS { OK = 200, CREATED = 201, NO_CONTENT = 204, BAD_REQUEST = 400, UNAUTHORIZED = 401, FORBIDDEN = 403, NOT_FOUND = 404, CONFLICT = 409, LENGHT_REQUIRED = 411, PAYLOAD_TOO_LARGE = 413 }
        public enum CONTENT_TYPE { JSON }
        public readonly Dictionary<CONTENT_TYPE, string> CONTENT_TYPE_VALUES = new()
        {
            { CONTENT_TYPE.JSON, "application/json" },
        };

        public readonly StreamWriter streamWriter;

        public readonly Dictionary<string, string> headers = new();

        public CONTENT_TYPE ContentType
        {
            set
            {
                headers["Content-Type"] = CONTENT_TYPE_VALUES[value];
            }
        }

        public int ContentLength
        {
            set
            {
                headers["Content-Length"] = value.ToString();
            }
        }

        public string Body { get; private set; } = "";

        public Dictionary<string, object> message
        {
            set
            {
                Body = JsonConvert.SerializeObject(value);
            }
        }

        public STATUS status;

        public HttpResponse(StreamWriter streamW)
        {
            streamWriter = streamW;
            ContentType = CONTENT_TYPE.JSON;
        }

        public void Send(STATUS status, Dictionary<string, object> message)
        {
            this.status = status;
            this.message = message;
            Send();
        }

        public void Send()
        {
            byte[] buffer = Encoding.UTF8.GetBytes(Body);
            ContentLength = buffer.Length;

            string[] words = status.ToString().ToLower().Split("_");

            for(int i = 0; i < words.Length; i++)
            {
                words[i] = words[i][0].ToString().ToUpper() + words[i].Substring(1);
            }

            if(status == STATUS.OK)
            {
                words[0] = "OK";
            }

            streamWriter.WriteLine($"HTTP/1.1 {(int)status} {string.Join(" ", words)}");

            foreach (string key in headers.Keys)
            {
                streamWriter.WriteLine($"{key}: {headers[key]}");
            }

            streamWriter.WriteLine();
            streamWriter.WriteLine(Body);
            streamWriter.Close();
        }

    }
}