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
        public enum TYPE { JSON }
        public STATUS status;
        public readonly Dictionary<TYPE, string> TypeValues = new()
        {
            { TYPE.JSON, "application/json" },
        };

        public readonly StreamWriter Writer;

        public readonly Dictionary<string, string> headers = new();

        public TYPE Type
        {
            set
            {
                headers["Content-Type"] = TypeValues[value];
            }
        }

        public int Length
        {
            set
            {
                headers["Content-Length"] = value.ToString();
            }
        }

        public string Body { get; private set; } = "";

        public Dictionary<string, object> Message
        {
            set
            {
                Body = JsonConvert.SerializeObject(value);
            }

            get
            {
                return Message;
            }
        }

        public HttpResponse(StreamWriter Writer)
        {
            this.Writer = Writer;
            Type = TYPE.JSON;
        }

        public void Send(STATUS status, Dictionary<string, object> mes)
        {
            this.status = status;
            this.Message = mes;
            Send();
        }

        public void Send()
        {
            byte[] buffer = Encoding.UTF8.GetBytes(Body);
            Length = buffer.Length;

            string[] terms = status.ToString().ToLower().Split("_");

            for(int i = 0; i < terms.Length; i++)
                terms[i] = terms[i][0].ToString().ToUpper() + terms[i].Substring(1);
            if(status == STATUS.OK)
                terms[0] = "OK";
            Writer.WriteLine($"HTTP/1.1 {(int)status} {string.Join(" ", terms)}");

            foreach (string key in headers.Keys)
                Writer.WriteLine($"{key}: {headers[key]}");

            Writer.WriteLine();
            Writer.WriteLine(Body);
            Writer.Close();
        }

    }
}