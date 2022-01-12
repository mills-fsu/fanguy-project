using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS4930_Assignment1
{
    public static class Utils
    {
        /// <summary>
        /// Repeatedly prompt the user until a non-empty string is given.
        /// </summary>
        /// <param name="prompt">The message to prompt the user with.</param>
        /// <param name="errorMessage">The message to display when the user enters an empty string.</param>
        /// <returns>the non-empty string that was read in</returns>
        public static string GetNonEmptyStringInput(string prompt, string errorMessage = "Please enter a non-empty string.")
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine(errorMessage);
                    continue;
                }

                return input;
            }
        }

        /// <summary>
        /// Gets a valid integer from standard input that is greater than 0 and less than or
        /// equal to the given maxIndex.
        /// </summary>
        /// <param name="prompt">The message to prompt the user with.</param>
        /// <param name="maxIndex">The highest index to be allowed. This is an inclusive upper bound.</param>
        /// <returns>the integer that was read in</returns>
        public static int GetIndexInput(string prompt, int maxIndex)
        {
            while (true)
            {
                string input = GetNonEmptyStringInput(prompt);
                int index;

                try
                {
                    index = int.Parse(input);
                }
                catch (FormatException)
                {
                    // print an error message and restart the loop if string was not a valid integer
                    Console.WriteLine("Please enter a valid integer.");
                    continue;
                }

                if (index < 0 || index > maxIndex)
                {
                    // print an error message and restart the loop if valid integer
                    // was not in correct bounds
                    Console.WriteLine($"Please enter an integer between 0 and {maxIndex}");
                    continue;
                }

                // if we reach here, the given index was valid, so return it 
                return index;
            }
        }
    }
}
