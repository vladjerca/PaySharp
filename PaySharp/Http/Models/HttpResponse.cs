using System.Collections.Generic;
using System.Text;

namespace PaySharp.Http.Models
{
    internal class HttpResponse
    {
        internal string Response { get; set; }
        internal int StatusCode { get; set; }
        internal string StatusDescription { get; set; }
        internal string Host { get; set; }
        internal Dictionary<string, string> Headers { get; set; }

        public HttpResponse()
        {
            Headers = new Dictionary<string, string>();
        }

        public override string ToString()
        {
            StringBuilder appender = new StringBuilder();

            appender.AppendLine(string.Format("Status Code: {0}", StatusCode));
            appender.AppendLine(string.Format("Status Description: {0}", StatusDescription));
            appender.AppendLine(string.Format("Response: {0}", Response));
            appender.AppendLine(string.Format("Header Count: {0}", Headers.Count));

            foreach (var header in Headers)
                appender.AppendLine(string.Format("{0}: {1}", header.Key, header.Value));

            return appender.ToString();
        }
    }
}
