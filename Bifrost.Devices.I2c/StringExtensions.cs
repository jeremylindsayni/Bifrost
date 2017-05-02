using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bifrost.Devices.I2c
{
    public static class StringExtensions
    {
        public static string[] SplitOnSpaceOrTabs(this string stringToSplit)
        {
            string[] results = stringToSplit.Split(' ', Convert.ToChar(9))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                //.Where(x => x != Convert.ToChar(9).ToString()) // tabs
                .Select(s => s.Trim()).ToArray();

            //Console.WriteLine(results.Length);
            //for (int i = 0; i < results.Length; i++)
            //{
            //    Console.WriteLine("i = " + i + " result = " + results[i]);
            //}

            return results;

        }
    }
}
