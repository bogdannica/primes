using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrimesClient
{
    internal static class Primes
    {
        static CancellationTokenSource STOP = new CancellationTokenSource();

        const string FORMAT = "PRIME: {0}";
        internal static string IsPrime(int res)
        {
            if (res.ToString().Length < 3)
            {
                return IsPrimeSlow(res);
            }
            else if (res.ToString().Length < 5)
            {
                return IsPrimeNumber(res);
            }
            else return IsPrimeNumberFaster(res);
        }

        private static string IsPrimeSlow(int res)
        { 
            for (int i = 2; i * i <= res; i++)
            {
                if (res % i == 0)
                {
                    return null;
                }
            }

            //if it reach below line, the number is prime
            return FORMAT.Args(res);
        }

        private static string IsPrimeNumber(int res)
        {
            if (res == 2) { return FORMAT.Args(res); }
            if (res % 2 == 0)
            {
                return null;
            }
            for (int i = 3; i * i <= res; i += 2)
            {
                if (res % i == 0)
                {
                    return null;
                }
            }

            //if it reach below line, the number is prime
            return FORMAT.Args(res);
        }

        /// <summary>
        /// this should perform much faster
        /// it returns primes greater than 2
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private static string IsPrimeNumberFaster(int num)
        {
            if (num % 2 == 0 ||
                num % 3 == 0)
                return null;
            else
            {
                int i;
                for (i = 5; i * i <= num; i += 6)
                {
                    if (num % i == 0 || num % (i + 2) == 0)
                        return null;
                }

                return FORMAT.Args(num);
            }
        }
    }
}