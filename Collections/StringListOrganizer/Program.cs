using System;
using System.Collections;

namespace StringListOrganizer  // Exercise 1
{
    class Program
    {
        static ArrayList listOfStrings;

        static int AskForPosInt(string prompt)
        {
            int value;
            do
            {
                Console.Write(prompt + " ");
            } while (!(int.TryParse(Console.ReadLine(), out value) && value > 0));
            return value;
        }

        static ArrayList AskForStrings(int length)
        {
            ArrayList strings = new ArrayList(length);
            for (int i = 0; i < length; ++i)
            {
                Console.Write("{0}th string: ", i + 1);
                strings.Add(Console.ReadLine());
            }
            return strings;
        }

        static void SortStrings(ArrayList strings)
        {
            strings.Sort();
        }

        static void PrintStrings(ArrayList strings)
        {
            foreach (string s in strings)
                Console.WriteLine(s);
        }

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                listOfStrings = new ArrayList(args);
            }
            else
            {
                int length = AskForPosInt("How many strings would you like to add?");
                listOfStrings = AskForStrings(length);
            }
            SortStrings(listOfStrings);
            Console.WriteLine("Your strings after sorting:");
            PrintStrings(listOfStrings);
        }
    }
}
