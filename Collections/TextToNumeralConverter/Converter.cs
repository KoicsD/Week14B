using System;
using System.Collections.Generic;

namespace TextToNumeralConverter  // Exercise 3
{
    public class Converter
    {
        // TODO processing string word-by-word
        // TODO how about processing simple char or array of chars?

        static Dictionary<char, string> numberLookup;

        static Converter()
        {
            numberLookup = new Dictionary<char, string>(10);
            numberLookup['0'] = "zero";
            numberLookup['1'] = "one";
            numberLookup['2'] = "two";
            numberLookup['3'] = "three";
            numberLookup['4'] = "four";
            numberLookup['5'] = "five";
            numberLookup['6'] = "six";
            numberLookup['7'] = "seven";
            numberLookup['8'] = "eight";
            numberLookup['9'] = "nine";
        }

        public static string Convert(string s)
        {
            string converted = "";
            foreach (char c in s)
            {
                if (numberLookup.ContainsKey(c))
                    converted += numberLookup[c];
                else
                    converted += c;
            }
            return converted;
        }
    }
}
