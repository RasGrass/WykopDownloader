using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace wypokDownloader.wykop
{
    class WykopApi
    {
        private readonly SecureString _secret;

        public WykopApi(string secret)
        {
            _secret = secret.ToSecureString();
        }

        private HttpWebRequest PrepareRequest(string requestString, Dictionary<string, string> postData)
        {
            var url = "http://a.wykop.pl/" + requestString;
            var request = WebRequest.CreateHttp(url);
            request.Method = WebRequestMethods.Http.Get;
            SignRequest(postData, url, request);
            AddPostData(postData, request);
            return request;
        }

        private static void AddPostData(Dictionary<string, string> postData, HttpWebRequest request)
        {
            if (postData != null)
            {
                var data =
                    Encoding.ASCII.GetBytes(string.Join("&",
                        postData.Select(pair => WebUtility.UrlEncode(pair.Key) + "=" + WebUtility.UrlEncode(pair.Value))));
                request.ContentType = "application/x-www-form-urlencoded";
                Debug.Assert(data != null, "postString != null");
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
        }

        private void SignRequest(Dictionary<string, string> postData, string url, WebRequest request)
        {
            var inputString = new NetworkCredential(string.Empty, _secret).Password + url;
            if (postData != null)
                inputString += string.Join(",", postData.Select(pair => pair.Value));

            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(inputString));
            var textmd5 = BitConverter.ToString(hash).Replace("-", string.Empty);

            request.Headers.Add("apisign", textmd5);
        }

        public string DoRequest(string requestString, Dictionary<string, string> postData)
        {
            var request = PrepareRequest(requestString, postData);
            string text;
            WebRequest.DefaultWebProxy = null;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.UseNagleAlgorithm = true;
            request.ServicePoint.Expect100Continue = false;
            request.Proxy = null;
            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var sr = new StreamReader(stream))
            {
                text = sr.ReadToEnd();
            }
            return text;
        }
    }

    public static class Helpers
    {
        public static SecureString ToSecureString(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;
            var result = new SecureString();
            foreach (var c in source.ToCharArray())
                result.AppendChar(c);
            return result;
        }
    }
}
