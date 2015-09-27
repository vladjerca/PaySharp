using PaySharp.Http.Models;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

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

        internal static HttpResponse Post(string url, NameValueCollection requestData, ContentType contentEncode = ContentType.FormData, NameValueCollection headers = null)
        {
            HttpResponse responseData = new HttpResponse();

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Timeout = Timeout;
            httpRequest.Method = "POST";
            httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";

            string postData = string.Empty;

            if (headers != null)
                httpRequest.Headers.Add(headers);

            switch(contentEncode)
            {
                case ContentType.Json:
                    httpRequest.ContentType = "application/json";
                    var dictionary = requestData
                                    .AllKeys
                                    .ToDictionary(k => k, k => requestData[k]);
                    postData = new JavaScriptSerializer().Serialize(dictionary);
                    break;
                case ContentType.FormData:
                    httpRequest.ContentType = "application/x-www-form-urlencoded";
                    postData = requestData.ToString();
                    break;
                default:
                    postData = requestData.ToString();
                    break;
            }

            httpRequest.ContentLength = postData.Length;

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

            HttpWebResponse httpResponse;

            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }

            using (StreamReader sr = new StreamReader(httpResponse.GetResponseStream()))
            {
                responseData.Response = sr.ReadToEnd();
            }

            var keys = httpResponse.Headers.AllKeys;

            foreach(var key in keys)
            {
                var values = httpResponse.Headers.GetValues(key);
                if (values.Length > 1)
                {
                    for (int i = 0; i < values.Length; i++)
                        responseData.Headers.Add(string.Format("{0}-{1}", key, i), values[i]);
                    continue;
                }
                responseData.Headers.Add(key, values.FirstOrDefault());
            }

            responseData.StatusCode = (int)httpResponse.StatusCode;
            responseData.StatusDescription = httpResponse.StatusDescription;
            responseData.Host = url;

            return responseData;
        }
    }
}
