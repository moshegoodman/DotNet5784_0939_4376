using System;

namespace Targil0
{
    partial class program
    {
        static void Main(string[] args)
        {
            Welcome4376();
            Welcome0939();
            Console.ReadKey();
        }
        static partial void Welcome0939();

        private static void Welcome4376()
        {
            Console.WriteLine("Enter your nice name: ");
            string userInput = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", userInput);
        }

    }
}

