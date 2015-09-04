using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker
{
    public static class StringExtension
    {
        public static string EscapeChars(this string sequence)
        {
            return sequence.Replace("\'", "\'\'");
        }
    }
}