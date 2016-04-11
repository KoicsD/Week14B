using System;

namespace TextToNumeralConverter  // Exercise 3
{
    class Program
    {
        // TODO processing text from command-line argument
        // TODO how about processing files given as command-line arguments

        static int AskForPosInt(string prompt)
        {
            int value;
            do
            {
                Console.Write(prompt + " ");
            } while (!(int.TryParse(Console.ReadLine(), out value) && value > 0));
            return value;
        }

        static string AskForText(int length)
        {
            string text = "";
            for (int i = 0; i < length; ++i)
            {
                Console.Write("Line {0}: ", i + 1);
                text += Console.ReadLine() + "\n";
            }
            return text;
        }

        static void Main(string[] args)
        {
            int numberOfLines = AskForPosInt("How many lines would you like to write?");
            string textToConvert = AskForText(numberOfLines);
            string textConverted = Converter.Convert(textToConvert);
            Console.WriteLine("Your text after digit-to numeral conversion:");
            Console.Write(textConverted);
        }
    }
}
