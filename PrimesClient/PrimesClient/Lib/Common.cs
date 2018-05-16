using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimesClient
{

    public enum HttpKeys
    {
        DELETE,
        GET,
        POST,
        PUT
    }
    class Common
    {
        public const string EMPTY = "EMPTY",
                            ERROR = "ERROR",
                            NULL = "NULL",
                            CTRL    = "\r\n",
                            CR      = "\r",
                            LF      = "\n",
                            TAB     = "\t";

        public const string HTTP = "http://",
                            HTTPS = "https://";


        public static string RemoveHTTPs(string url, out string protocol)
        {
            protocol = "";
            if (url.Contains(HTTPS))
            {
                protocol = HTTPS;
                url = url.Replace(HTTPS, "");
            }
            else if (url.Contains(HTTP))
            { protocol = HTTP; url = url.Replace(HTTP, ""); }
            return url;
        }
    }
}
