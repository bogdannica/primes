using System.Net.Http;
using System.Text;

namespace PrimesClient
{
    public class RequestData
    {
        public RequestData(string url)
        {
            _url = url;
            string tmp = Common.RemoveHTTPs(_url, out _protocol).SplitInput("/")[0];
            _urlroot = "{0}{1}".Args(_protocol, tmp);
        }

        string _protocol;
        string _url = "";
        public string URL
        {
            set { _url = value; }
            get { return _url; }
        }

        string _urlroot = "";
        public string URLRoot
        {
            set { _urlroot = value; }
            get { return _urlroot; }
        }

        public RequestHeader Header { get; internal set; }

        public HttpContent Content { set; get; }

        string _response = string.Empty;
        public string Response
        {
            set
            {
                if (_response != value)
                {
                    _response = new StringBuilder(value).ToString();
                }
            }
            get { return _response; }
        }
    }
}