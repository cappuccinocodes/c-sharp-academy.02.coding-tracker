using System;

namespace CodeTracker1
{
    internal class CodingController
    {
        internal static void GetUserCommand()
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("Type 0 to Close Application.");
            Console.WriteLine("Type 1 to keep going.");
            int command = Convert.ToInt32(Console.ReadLine());

            switch (command)
            {
                case 0:
                    break;
            }

        }
    }
}