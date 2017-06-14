using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class CLIHelper
    {

        public static DateTime GetDateFromUser(string message, string errorMessage)
        {            
            bool isValidDate = false;
            DateTime userInput;
            do
            {
                Console.WriteLine(message);

                isValidDate = DateTime.TryParse(Console.ReadLine(), out userInput);

                if (!isValidDate)
                {
                    Console.WriteLine(errorMessage);
                }

            } while (!isValidDate);

            return userInput;
        }

    }
}
