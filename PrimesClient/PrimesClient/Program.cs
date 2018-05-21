using PrimesClient.Lib;
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
            string server = "input server url (<enter> for localhost):".ConsoleIn();
            if (string.IsNullOrEmpty(server)) { server = "http://localhost:8090"; }


            string smallNumUrl = "http://localhost:8090/getInt/min=1&max=999",
                   mediumNumUrl = "http://localhost:8090/getInt/min=1000&max=9999",
                   largeNumUrl = "http://localhost:8090/getInt/min=100000&max=10000000";


            do
            {
                string min = "", max = "";
                min = "Input min:".ConsoleIn();
                max = "input max:".ConsoleIn();
                string url = "{0}/getInt/min={1}&max={2}&delay=400".Args(server, min, max);
                Work(url,true);
            } while ("prime again?(y/n)".ConsoleIn() == "y");

            if ("run auto loops? (y/n)".ConsoleIn() == "y")
            {
                do 
                {
                    Loop(smallNumUrl);
                    Loop(mediumNumUrl);
                    Loop(largeNumUrl);
                }while ("run again? (y/n)".ConsoleIn() == "y");
            }
            
        }



        /// <summary>
        /// makes a request to the server
        /// parses server response and extract the integer
        /// analysewither the integer is prime or not.
        /// </summary>
        /// <param name="url"></param>
        static void Work(string url, bool isShow = false)
        {
            //create request:
            RequestData rd = new RequestData(url);
            rd.Header = new RequestHeader();
            rd.Header.Method = HttpKeys.GET;

            //make the call (cpu non blocking call):
            rd = HttpGeneric.RequestData(rd).Result;

            //extract integer and analyse if it is prime:
            int res;
            if (int.TryParse(rd.Response, out res))
            {
                string resp = Primes.IsPrime(res);
                if (!string.IsNullOrEmpty(resp)) resp.ConsoleWriteLine(ConsoleColor.Green);
                else if (isShow) "{0} NOT PRIME".Args(res).ConsoleWriteLine(ConsoleColor.DarkGray);
            }
            else rd.Response.ConsoleWriteLine(ConsoleColor.Red);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        static void Loop(string url)
        {
            int count = 0;
            string starttime = TimeStamp.TimeString(TimeStamp.YMDHmsnt, "0");
            while (count++ < 200)
            {
                Work(url);
            }
            string endtime = TimeStamp.TimeString(TimeStamp.YMDHmsnt, "0");
            TimeStamp.DysplayEnd(starttime, endtime, 0);
        }
    }
}
