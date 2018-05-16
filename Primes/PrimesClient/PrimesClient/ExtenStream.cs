using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PrimesClient
{
    public static class ExtenString
    {
        public static string Args(this string format, params object[] args)
        {
            if (string.IsNullOrEmpty(format))
                ThrowException();

            if (args == null)
                return format;

            return args.Length > 0 ? new StringBuilder(string.Format(format, args)).ToString() : format;
        }

        private static void ThrowException()
        {
            var stackTrace = new StackTrace();
            var frame = stackTrace.GetFrame(2);
            var method = frame.GetMethod();
            var className = method.DeclaringType == null ? string.Empty : method.DeclaringType.Name;
            throw new ApplicationException("Error formatting string. Input string is null at {0}.{1}(...)".Args(className, method.Name));
        }

        public static MemoryStream ToStream(this string item)
        {
            var outBytes = Encoding.ASCII.GetBytes(item);
            var stream = new MemoryStream();
            stream.Write(outBytes, 0, outBytes.Length);
            stream.Position = 0;

            return stream;
        }

        public static string[] SplitInput(this string dataVal,
                                          string splt,
                                          bool isRemoveEmpty = true)
        {
            string[] datasplit = null;
            if (!string.IsNullOrEmpty(dataVal) && dataVal.Contains(splt)) { datasplit = dataVal.Split(splt, isRemoveEmpty); }
            else if (!string.IsNullOrEmpty(dataVal)) { datasplit = new string[1] { dataVal }; }  
            return datasplit;
        }
        static string[] Split(this string text, string splitter, bool isRemoveEmpty)
        {
            try
            {
                string[] mySplitter = new string[] { splitter };
                if (isRemoveEmpty) { return text.Split(mySplitter, StringSplitOptions.RemoveEmptyEntries); }
                else { return text.Split(mySplitter, StringSplitOptions.None); }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SPLIT ERROR", ex.Message, ex.StackTrace );
                return new string[0];
            }
        }
        public static string FindString(this string text, string regex, bool ignorecase = false)
        {
            try
            {
                RegexOptions ignore = RegexOptions.None;
                if (ignorecase) { ignore = RegexOptions.IgnoreCase; }

                MatchCollection mtchs = Regex.Matches(text, regex, RegexOptions.Compiled | ignore);
                if (mtchs == null || mtchs.Count == 0) return "";

                return mtchs[0].Groups[1].ToString();
            }
            catch { return ""; }
        }

        public static void ConsoleWriteLine(this string message, ConsoleColor cc)
        {
            ConsoleColor current = Console.ForegroundColor;
            Console.ForegroundColor = cc;
            Console.WriteLine(message);
            Console.ForegroundColor = current;
        }

        public static string ConsoleIn(this string command)
        {
            Console.Write(command);
            System.IO.TextReader reader = reader = Console.In;
            return reader.ReadLine();
        }

        public static int TryParseInt(this string num)
        {
            int index;
            if (num == null) { return -1; }
            if (num.Contains(",")) { num = num.Replace(",", ""); }
            if (int.TryParse(num, out index)) { return index; }
            else { return -1; }
        }

    }
}
