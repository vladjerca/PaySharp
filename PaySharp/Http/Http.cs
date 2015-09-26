using PaySharp.Http.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace PaySharp.Http
{
    internal enum ContentType
    {
        FormData,
        Json,
        Default
    }

    internal static class Web
    {
        internal static int Timeout = 15000;

        internal static HttpResponse Post(string url, string postData, ContentType contentEncode = ContentType.FormData)
        {
            HttpResponse responseData = new HttpResponse();

            WebRequest httpRequest = WebRequest.Create(url);
            httpRequest.Timeout = Timeout;
            httpRequest.Method = "POST";
            httpRequest.ContentLength = postData.Length;

            switch(contentEncode)
            {
                case ContentType.FormData:
                    httpRequest.ContentType = "application/x-www-form-urlencoded";
                    break;
                case ContentType.Json:
                    httpRequest.ContentType = "application/json";
                    break;
                default:
                    break;
            }

            try
            {
                using (StreamWriter myWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    myWriter.Write(postData);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            HttpWebResponse httpRespone = (HttpWebResponse)httpRequest.GetResponse();
            
            using (StreamReader sr = new StreamReader(httpRespone.GetResponseStream()))
            {
                responseData.Response = sr.ReadToEnd();
            }

            var keys = httpRespone.Headers.AllKeys;

            foreach(var key in keys)
            {
                var values = httpRespone.Headers.GetValues(key);
                if (values.Length > 1)
                {
                    for (int i = 0; i < values.Length; i++)
                        responseData.Headers.Add(string.Format("{0}-{1}", key, i), values[i]);
                    continue;
                }
                responseData.Headers.Add(key, values.FirstOrDefault());
            }

            responseData.StatusCode = (int)httpRespone.StatusCode;
            responseData.StatusDescription = httpRespone.StatusDescription;
            responseData.Host = url;

            return responseData;
        }
    }
}
