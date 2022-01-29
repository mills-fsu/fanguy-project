using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.Assignment1.TaskManager;

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
                    PrintRed(errorMessage);
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
                int index = GetValidInteger(prompt);

                if (index < 0 || index > maxIndex)
                {
                    // print an error message and restart the loop if valid integer
                    // was not in correct bounds
                    PrintRed($"Please enter an integer between 0 and {maxIndex}");
                    continue;
                }

                // if we reach here, the given index was valid, so return it 
                return index;
            }
        }

        public static int GetValidInteger(string prompt)
        {
            while (true)
            {
                string input = GetNonEmptyStringInput(prompt);
                int val;

                try
                {
                    val = int.Parse(input);
                }
                catch (FormatException)
                {
                    // print an error message and restart the loop if string was not a valid integer
                    PrintRed("Please enter a valid integer.");
                    continue;
                }

                return val;
            }
        }

        public static DateTime GetValidDate(string prompt)
        {
            DateTime date;

            while (true)
            {
                // first, wait until a non-empty string is given
                string dateIn = Utils.GetNonEmptyStringInput(prompt);

                // then, make sure the given string is a valid date. if not, repeat
                // the whole process
                try
                {
                    date = DateTime.Parse(dateIn);
                    break;
                }
                catch (FormatException)
                {
                    PrintRed("Invalid date format entered. Please try again.");
                }
            }

            return date;
        }

        public static TaskType GetTaskType()
        {
            while (true)
            {
                string input = GetNonEmptyStringInput("task type ('todo' or 'appt'): ");

                if (input == "todo")
                    return TaskType.ToDo;
                else if (input == "appt")
                    return TaskType.Appointment;
                else
                    PrintRed("Please enter either 'todo' or 'appt'.");
            }
        }

        public static void PrintBlue(object val)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(val);
            Console.ResetColor();
        }

        public static void PrintGreen(object val)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(val);
            Console.ResetColor();
        }

        public static void PrintRed(object val)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(val);
            Console.ResetColor();
        }
    }
}
