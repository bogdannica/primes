using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimesClient
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string smallNumUrl = "http://localhost:8090/echoGet/min=1&max=999",
                   mediumNumUrl = "http://localhost:8090/echoGet/min=1000&max=9999",
                   largeNumUrl = "http://localhost:8090/echoGet/min=100000&max=10000000";
            do
            {
                Work(smallNumUrl);
                Work(mediumNumUrl);
                Work(largeNumUrl);
            } while ("continue? (y/n)".ConsoleIn() != "y");
            
        }

        static void Work(string url)
        {
            int count = 0;
            string starttime = TimeStamp.TimeString(TimeStamp.YMDHmsnt, "0");
            while (count++ < 200)
            {
                RequestData rd = new RequestData(url);
                rd.Header = new RequestHeader();
                rd.Header.Method = HttpKeys.GET;
                //make the call (cpu non blocking call):
                rd = HttpGeneric.RequestData(rd).Result;
                int res;
                if (int.TryParse(rd.Response, out res))
                {
                    string resp = Primes.IsPrime(res);
                    if (!string.IsNullOrEmpty(resp)) resp.ConsoleWriteLine(ConsoleColor.Green);
                    //else "{0} NOT PRIME".Args(res).ConsoleWriteLine(ConsoleColor.DarkGray);
                }
            }
            string endtime = TimeStamp.TimeString(TimeStamp.YMDHmsnt, "0");
            TimeStamp.DysplayEnd(starttime, endtime, 0);
        }
    }
}
