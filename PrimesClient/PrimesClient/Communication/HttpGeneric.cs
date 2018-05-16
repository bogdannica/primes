using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PrimesClient
{
    public class HttpGeneric
    {
        public static async Task<RequestData> RequestData(RequestData rd)
        {
            try
            {
                string rooturl = rd.URLRoot;
                var baseAddress = new Uri(rooturl);
                using (HttpClientHandler handler = new HttpClientHandler
                {
                    UseCookies = false,
                    UseDefaultCredentials = true,
                    //AllowAutoRedirect = rd.IsRedirectAuto,
                    //MaxAutomaticRedirections = count,
                })
                using (HttpClient client = new HttpClient(handler)
                {
                    BaseAddress = baseAddress,
                })
                {
                    client.DefaultRequestHeaders.ExpectContinue = false;

                    HttpRequestMessage message = AddContent(rd);

                    foreach (KeyValuePair<string, string> header in rd.Header.Headers)
                    { if (header.Key != "Authorization") message.Headers.Add(header.Key, header.Value); }

                    HttpResponseMessage response = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead);

                    rd.Response = await ReadStreamDecodeIfNeeded(response);
                    return  rd;
                }
            }
            catch (HttpRequestException hre)
            {
                rd.Response = "ERROR: {0}".Args(hre.ToString());
                return rd;
            }
            catch (TaskCanceledException tce)
            {
                rd.Response = "ERROR: {0}".Args(tce.ToString());
                return rd;
            }
            catch (Exception ex)
            {
                rd.Response = "ERROR: {0}".Args(ex.ToString());
                return rd;
            }
        }

        static HttpRequestMessage AddContent(RequestData rd)
        {
            string contenttype = rd.Header.ContentType;

            switch (rd.Header.Method)
            {
                case HttpKeys.DELETE:
                    var message = new HttpRequestMessage(HttpMethod.Delete, rd.URL);
                    return message;
                case HttpKeys.GET:
                    message = new HttpRequestMessage(HttpMethod.Get, rd.URL);
                    return message;
                case HttpKeys.POST:
                    message = new HttpRequestMessage(HttpMethod.Post, rd.URL);
                    message.Content = rd.Content;
                    return message;
                case HttpKeys.PUT:
                    message = new HttpRequestMessage(HttpMethod.Put, rd.URL);
                    message.Content = rd.Content;
                    return message;
                default:
                    throw new NotImplementedException("BOGDAN SAIS: IMPLEMENT ME FOR: -{0}-".Args(rd.Header.Method));
            }
        }

        private static async Task<string> ReadStreamDecodeIfNeeded(HttpResponseMessage response)
        {
            string res = "";
            res = response.ToString();
            using (var contentstream = new MemoryStream())
            {
                var stream = await response.Content.ReadAsStreamAsync();
                stream.CopyTo(contentstream);

                contentstream.Seek(0, SeekOrigin.Begin);
                res = new StringBuilder(await new StreamReader(contentstream, Encoding.UTF8).ReadToEndAsync()).ToString();
            }
            return res;
        }

    }
}