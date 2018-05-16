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
    /// <summary>
    /// AUTHOR:     Bogdan Nica
    /// </summary> 
    public static class TimeStamp
    {

        #region PUBLIC STATIC     

        /// <summary>
        /// uses string as delimitor
        /// for default styles use  TimeString(format, "0") 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="delimitor"></param>
        /// <returns></returns>
        public static string TimeString(string format, string delimitor) { return Stamp(format, delimitor, false, null); }
       
        /// <summary>
        /// This function takes in the start time and end time as strings 
        /// and then returns the execution time as 
        ///options:
        ///either use Time library
        ///or 
        ///add in TimeStamp library of ASTF a function that handles this operation
        ///this function should be static and return a string that represents 
        ///the total time in seconds.miliseconds
        ///1. Convert the strings to DateTime
        ///2. Get the execution time
        ///3. Convert the DateTime value to string
        /// </summary>
        /// <param name="endtime"></param>
        /// <param name="starttime"></param>
        /// <returns></returns>
        public static double TimeDifference(string starttime, string endtime, string format)
        {
            try
            {
                DateTime conStarttime = new DateTime();
                DateTime conEndtime = new DateTime();
                TimeSpan conExectime = new TimeSpan();

                conStarttime = DateTime.Parse(starttime);
                conEndtime = DateTime.Parse(endtime);
                conExectime = conEndtime - conStarttime;

                switch (format)
                {
                    case "days":
                        return conExectime.TotalDays;
                    case "hours":
                        return conExectime.TotalHours;
                    case "minutes":
                        return conExectime.TotalMinutes;
                    case "seconds":
                        return conExectime.TotalSeconds;
                    case "milliseconds":
                        return conExectime.TotalMilliseconds;
                    case "ticks":
                        return conExectime.Ticks;
                    default:
                        "TimeDifference: The value of the format for the time execution is incorrect"
                                                    .ConsoleWriteLine(ConsoleColor.Red);
                        return 0;
                }
            }
            catch (Exception ex)
            {
                if (starttime == null) { starttime = Common.NULL; }
                if (endtime == null) { endtime = Common.NULL; }
                ("{0}: -1- One of the values: - starttime:{1}- or - endtime:{2} - {3}"
                                                    .Args(Common.ERROR,
                                                        starttime,
                                                        endtime,
                                                        ex.Message))
                                                    .ConsoleWriteLine(ConsoleColor.Red);
                return 0;
            }
        }

        public const string AddTimeUnit = "(min|hours|days|sec)";
        
        public static void DysplayEnd(string starttime, string ended, int ret, string unit = "milliseconds")
        {
            string endtime = TimeString(YMDHmsnt, "0");
            double diff = TimeDifference(starttime, endtime, unit);
            string elapsed = diff.ToString().SplitInput(".")[0];
            ("TOTAL ELAPSED TIME: {0} {1}".Args(elapsed, unit)).ConsoleWriteLine(ConsoleColor.Green);
            if (ret != 0) { ("RETURN:              {0} - {1}".Args(ret.ToString(), Common.ERROR))
                                                             .ConsoleWriteLine(ConsoleColor.Red); }
            ended.ConsoleWriteLine(ConsoleColor.Gray);
        }
        #endregion PUBLIC STATIC
        #region PRIVATE STATIC
        public static string DateSince1970UTC(DateTime? my_local_time, bool isDisplay = false)
        {
            DateTime my_time;
            if (my_local_time != null && my_local_time.HasValue) { my_time = my_local_time.Value; }
            else { my_time = DateTime.Now; }

            DateTime EpochZeroTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc); ;
            long since1970 = (my_time.ToFileTimeUtc() - EpochZeroTime.ToFileTimeUtc()) / 10000;  //take the ticks away

            if (isDisplay)
            {
                var since1970T = TimeSpan.FromMilliseconds(since1970);
                var _1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var validTill = _1970 + since1970T;
                string message = string.Format("Valid till = {0:R}", validTill);
                message.ConsoleWriteLine( ConsoleColor.Yellow);
            }
            return since1970.ToString();
        }

        /*Tools_GetRandNumber*/
        /// <summary>
        /// takes a string and returns a timeStamp string
        /// the intake string can be like: "YM" or "D" or "YMD" returning(ex:) 200709 or 17 or 20070917
        /// Main application: naming 
        /// EX:     NOW DMY ,               -   30, 11, 2015 
        ///         NOW yMD                 -   151231
        ///         NOW YMDHms 0            -   2015/11/30 15:39:24
        ///         NOW YMDHms _            -   2015_11_30_15_39_24
        ///         NOW 3DMY \, \+10days   -   Mon, 30 November 2015
        /// </summary>
        /// <param name="format"></param>
        /// <param name="delimordefault"></param>
        /// <returns></returns>
        public static string Stamp(string format, string delimordefault, bool keepInitialTimeStamp, DateTime? my_local_time)
        {
            string strMyStamp = string.Empty;
            if (format == "UTC") { return DateSince1970UTC(my_local_time); }

            int intMax = format.Length;
            bool isweek = false;
            string weekformat = "", space = " ";

            int i = 0;
            int countifweek = 0;
            int delimint = -2;
            string delim = string.Empty;
            if (string.IsNullOrEmpty(delimordefault) || !int.TryParse(delimordefault, out delimint)) { delim = delimordefault; delimint = -2; }
            DateTime my_time;
            if (keepInitialTimeStamp && my_local_time.HasValue) { my_time = my_local_time.Value; }
            else { my_time = DateTime.Now; }
            for (i = 0; i < intMax; i++)
            {
                int int_date = 0;
                string strZero = "";
                bool isnotmilisec = true;
                switch (format[i])
                {
                    case 'Y':
                        int_date = my_time.Year;
                        if (delimint >= 0)
                        {
                            if (i != intMax - 1)
                            {
                                switch (format[i + 1])
                                {
                                    case 'y':
                                    case 'M':
                                    case 'D':
                                        delim = "/";
                                        break;
                                    default:
                                        delim = " ";
                                        break;
                                }
                            }
                            else { delim = string.Empty; }
                        }
                        break;
                    case 'y':
                        int_date = my_time.Year - 2000;
                        if (delimint >= 0)
                        {
                            if (i != intMax - 1)
                            {
                                switch (format[i + 1])
                                {
                                    case 'Y':
                                    case 'M':
                                    case 'D':
                                        delim = "/";
                                        break;
                                    default:
                                        delim = " ";
                                        break;
                                }
                            }
                            else { delim = string.Empty; }
                        }
                        break;
                    case 'M':
                        if (isweek)
                        {
                            int_date = 0;
                            delim = "";
                            space = "";
                            break;
                        }
                        int_date = my_time.Month;
                        if (delimint >= 0)
                        {
                            if (i != intMax - 1)
                            {
                                switch (format[i + 1])
                                {
                                    case 'Y':
                                    case 'y':
                                    case 'D':
                                        delim = "/";
                                        break;
                                    default:
                                        delim = " ";
                                        break;
                                }
                            }
                            else { delim = string.Empty; }
                        }
                        break;
                    case 'D':
                        int_date = my_time.Day;
                        if (delimint >= 0)
                        {
                            if (i != intMax - 1)
                            {
                                switch (format[i + 1])
                                {
                                    case 'Y':
                                    case 'y':
                                    case 'M':
                                        delim = "/";
                                        break;
                                    default:
                                        delim = " ";
                                        break;
                                }
                            }
                            else { delim = string.Empty; }
                        }
                        break;
                    case 'H':
                        int_date = my_time.Hour;
                        if (delimint >= 0)
                        {
                            if (i != intMax - 1)
                            {
                                switch (format[i + 1])
                                {
                                    case 'm':
                                    case 's':
                                        delim = ":";
                                        break;
                                    case 'n':
                                        delim = ".";
                                        break;
                                    default:
                                        delim = " ";
                                        break;
                                }
                            }
                            else { delim = string.Empty; }
                        }
                        break;
                    case 'm':
                        int_date = my_time.Minute;
                        if (delimint >= 0)
                        {
                            if (i != intMax - 1)
                            {
                                switch (format[i + 1])
                                {
                                    case 'H':
                                    case 's':
                                        delim = ":";
                                        break;
                                    case 'n':
                                        delim = ".";
                                        break;
                                    default:
                                        delim = " ";
                                        break;
                                }
                            }
                            else { delim = string.Empty; }
                        }
                        break;
                    case 's':
                        int_date = my_time.Second;
                        if (delimint >= 0)
                        {
                            if (i != intMax - 1)
                            {
                                switch (format[i + 1])
                                {
                                    case 'H':
                                    case 'm':
                                        delim = ":";
                                        break;
                                    case 'n':
                                        delim = ".";
                                        break;
                                    default:
                                        delim = " ";
                                        break;
                                }
                            }
                            else { delim = string.Empty; }
                        }
                        break;
                    case 'n':
                        isnotmilisec = false;
                        int_date = my_time.Millisecond;
                        if (delimint >= 0 && i == intMax - 1) { delim = string.Empty; }
                        int len = int_date.ToString().Length;
                        if (len < 3)
                        {
                            switch (len)
                            {
                                case 2:
                                    strZero = "0";
                                    break;
                                case 1:
                                    strZero = "00";
                                    break;
                            }
                        }
                        if (i != intMax - 1)
                        {
                            switch (format[i + 1])
                            {
                                case 'H':
                                case 'm':
                                    delim = ":";
                                    break;
                                case 't':
                                    delim = "";
                                    break;
                                default:
                                    delim = " ";
                                    break;
                            }
                        }
                        else { delim = string.Empty; }
                        break;
                    case 't':
                        string all = my_time.Ticks.ToString();
                        int_date = all.Substring(all.Length - 5, 4).TryParseInt();
                        len = int_date.ToString().Length;
                        if (len < 4)
                        {
                            switch (len)
                            {
                                case 3:
                                    strZero = "0";
                                    break;
                                case 2:
                                    strZero = "00";
                                    break;
                                case 1:
                                    strZero = "000";
                                    break;
                            }
                        }
                        break;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        isweek = true;
                        string regex = "(?<={0})(.)".Args(format[i].ToString());
                        string found = format.FindString(regex);
                        int weeklen = format[i].ToString().TryParseInt();
                        switch (found)
                        {
                            case "d":
                            case "D":
                                weekformat = my_time.DayOfWeek.ToString();
                                if (weeklen == 0) break;
                                if (weekformat.Length > weeklen)
                                {
                                    weekformat = weekformat.Substring(0, weeklen);
                                }
                                delim = "";
                                break;
                            case "M":
                                weekformat = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(my_time.Month);
                                delim = delimordefault;
                                if (weeklen == 0) break;
                                if (weekformat.Length > weeklen)
                                {
                                    weekformat = weekformat.Substring(0, weeklen);
                                }
                                break;
                        }
                        break;
                }
                if (i == intMax - 1) { delim = string.Empty; }
                if (!isweek)
                {
                    if (isnotmilisec && int_date < 10) { strZero = "0"; }
                    if (strMyStamp.Length != 0)
                    { strMyStamp = strMyStamp + strZero + int_date.ToString() + delim; }
                    else { strMyStamp = int_date.ToString() + delim; }
                }
                else
                {
                    if (string.IsNullOrEmpty(weekformat) && int_date > 0)
                    { weekformat = int_date.ToString(); }
                    strMyStamp += weekformat + delim + space;
                    if (countifweek == 3)
                    {
                        isweek = false;
                        countifweek = 0;
                        delim = "";
                    }
                    int_date = 0;
                }
                ///Restore defaults:
                isnotmilisec = true;
                weekformat = "";
                strZero = string.Empty;
                countifweek++;
            }
            return strMyStamp;
        }
        #endregion PRIVATE STATIC

        #region CONSTS       
        public const string YMDHmsnt = "YMDHmsnt";    ///21 digits
        public const string YMDHmsn = "YMDHmsn";    ///17 digits
        public const string YMDHms = "YMDHms";      ///14 digits
        public const string YMD = "YMD";
        public const string yMDHmsnt = "yMDHmsnt";    ///19 digits
        public const string yMDHmsn = "yMDHmsn";    ///15 digits
        public const string yMDHms = "yMDHms";      ///12 digits
        public const string yMDHm = "yMDHm";        ///10 digits
        public const string yMDH = "yMDH";          /// 8 digits 
        public const string yMD = "yMD";            /// 6 digits
        public const string yM = "yM";              /// 4 digits

        public const string MDHmsnt = "MDHmsnt";      ///17 digits
        public const string MDHmsn = "MDHmsn";      ///13 digits
        public const string DHmsnt = "DHmsnt";        ///15 digits
        public const string DHmsn = "DHmsn";        ///11 digits
        public const string Hmsnt = "Hmsnt";          /// 13 digits
        public const string Hmsn = "Hmsn";          /// 9 digits
        public const string msnt = "msnt";            /// 11 digits
        public const string msn = "msn";            /// 7 digits
        public const string Hms = "Hms";            /// 6 digits
        #endregion CONSTS
    }
}
