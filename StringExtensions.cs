using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public static class StringExtensions
    {
        // http://stackoverflow.com/questions/541954/how-would-you-count-occurences-of-a-string-within-a-string-c
        // -- http://stackoverflow.com/a/4410627/1613961
        // This is faster than ContainsWithOverlap
        // Test by Immediate Window:
        // "thethethe answer is thethe answer".ContainsWithoutOverlap("thethe");
        // 2
        public static int ContainsWithoutOverlap(this string source, string trackedString)
        {
            return new Regex(trackedString).Matches(source).Count;
        }

        // http://stackoverflow.com/questions/541954/how-would-you-count-occurences-of-a-string-within-a-string-c
        // -- http://stackoverflow.com/a/6004505/1613961
        // Test by Immediate Window:
        // "thethethe answer is thethe answer".ContainsWithOverlap("thethe");
        // 3
        public static int ContainsWithOverlap(this string source, string trackedString)
        {
            int count = 0;
            int n = 0;

            while ((n = source.IndexOf(trackedString, n)) != -1)
            {
                n++;
                count++;
            }

            return count;
        }
    }
}
