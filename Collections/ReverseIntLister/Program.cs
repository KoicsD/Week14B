using System;
using System.Collections;

namespace ReverseIntLister  // Exercise 2
{
    class Program
    {
        static Stack integers;

        static bool TryParseIntArgs(string[] args, out Stack ints)
        {
            ints = new Stack(args.Length);
            foreach (string arg in args)
            {
                int n;
                if (int.TryParse(arg, out n))
                    ints.Push(n);
                else
                    return false;
            }
            return true;
        }

        static int AskForPosInt(string prompt)
        {
            int value;
            do
            {
                Console.Write(prompt + " ");
            } while (!(int.TryParse(Console.ReadLine(), out value) && value > 0));
            return value;
        }

        static Stack AskForInts(int depth)
        {
            Stack ints = new Stack(depth);
            for (int i = 0; i < depth; ++i)
            {
                Console.Write("{0}th integer: ", i + 1);
                int n;
                if (int.TryParse(Console.ReadLine(), out n))
                    ints.Push(n);
                else
                    --i;
            }
            return ints;
        }

        static void ConsumeAndPrint(Stack ints)
        {
            while (ints.Count > 0)
                Console.WriteLine(ints.Pop());
        }

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (!TryParseIntArgs(args, out integers))
                {
                    Console.WriteLine("Problem when parsing integers in command-line argument.");
                    return;
                }
            }
            else
            {
                int length = AskForPosInt("How many integers would you like to add?");
                integers = AskForInts(length);
            }
            Console.WriteLine("Your integers in reverse order:");
            ConsumeAndPrint(integers);
        }
    }
}
