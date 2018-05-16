using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace PrimesClient
{
    public class RequestHeader
    {
        private string _accept = string.Empty;
        public string Accept
        {
            set { _accept = value; }
            get { return _accept; }
        }
        private string _acceptencoding = string.Empty;
        public string AcceptEncoding
        {
            set { _acceptencoding = value; }
            get { return _acceptencoding; }
        }
        private string _acceptlanguage = string.Empty;
        public string AcceptLanguage
        {
            set { _acceptlanguage = value; }
            get { return _acceptlanguage; }
        }
        private string _apikey = string.Empty;
        public string ApiKey
        {
            set { _apikey = value; }
            get { return _apikey; }
        }
        private string _connection = string.Empty;
        public string Connection
        {
            set { _connection = value; }
            get { return _connection; }
        }
        private string _contenttype = "text/html; charset=utf-8";
        public string ContentType
        {
            set { _contenttype = value; }
            get { return _contenttype; }
        }
        ConcurrentDictionary<string, string> _cookies = new ConcurrentDictionary<string, string>();
        public ConcurrentDictionary<string, string> Cookies
        {
            set { _cookies = new ConcurrentDictionary<string, string>(value); }
            get { return _cookies; }
        }
        string _cookieCollection = "";
        public string CookieCollection
        {
            set { if (_cookieCollection != value) { _cookieCollection = new StringBuilder(value).ToString(); } }
            get { return _cookieCollection; }
        }
        HashSet<KeyValuePair<string, string>> _headers = new HashSet<KeyValuePair<string, string>>();
        public HashSet<KeyValuePair<string, string>> Headers
        {
            get { return _headers; }
            set { _headers = value; }
        }
        private HttpKeys _method;
        public HttpKeys Method
        {
            set {  _method = value; } 
            get { return _method; }
        }
        private string _useragent = string.Empty;
        public string UserAgent
        {
            set { _useragent = value; }
            get { return _useragent; }
        }
    }
}