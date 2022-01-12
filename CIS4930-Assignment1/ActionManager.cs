using Library.Assignment1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.Assignment1.TaskManager;

namespace CIS4930_Assignment1
{
    class ActionManager
    {
        private TaskManager _manager;

        public ActionManager()
        {
            _manager = new TaskManager();
        }

        /// <summary>
        /// Print a basic help message about available commands.
        /// </summary>
        public void PrintHelp()
        {
            Console.WriteLine("\nThis is the CIS4930 To Do Manager");
            Console.WriteLine("Available commands are: quit|exit, help, create, delete, edit, complete, and list\n");
        }

        /// <summary>
        /// Prompt user for information and then create a new managed Task.
        /// </summary>
        public void Create()
        {
            Console.WriteLine();

            // name is required, so wait until it can be gotten
            string name = Utils.GetNonEmptyStringInput("name: ");

            // deadline may be empty, so only prompt once
            Console.Write("description (optional): ");
            string? input = Console.ReadLine();
            // prefer the empty string to null
            string description = input ?? "";

            DateTime deadline;

            while (true)
            {
                // first, wait until a non-empty string is given
                string deadlineIn = Utils.GetNonEmptyStringInput("deadline: ");

                // then, make sure the given string is a valid date. if not, repeat
                // the whole process
                try
                {
                    deadline = DateTime.Parse(deadlineIn);
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid date format entered. Please try again.");
                }
            }

            // finally, input has been sanitized, so let the manager handle creation
            _manager.Create(name, description, deadline);

            Console.WriteLine("\nTo Do added succesfully!\n");
        }

        /// <summary>
        /// Prompt user for input and then delete selected Task.
        /// </summary>
        public void Delete()
        {
            Console.WriteLine();

            // get a valid integer in the correct range, then let the manager delete at that point
            int toDelete = Utils.GetIndexInput("index of the task to delete: ", _manager.Count - 1);
            _manager.Delete(toDelete);

            Console.WriteLine("\nTo Do deleted successfully!\n");
        }

        /// <summary>
        /// Prompt user for input and then edit selected Task.
        /// </summary>
        public void Edit()
        {
            Console.WriteLine();

            // get a valid integer in the correct range
            int toEdit = Utils.GetIndexInput("index of the task to edit: ", _manager.Count - 1);

            EditType editType;

            // then, get a valid editType
            while (true)
            {
                // get a non-empty string
                string fieldName = Utils.GetNonEmptyStringInput("what would you like to edit? (name, description, deadline, completed): ");

                // make sure it is either name, description, or deadline. repeat this loop if not
                if (fieldName == "name")
                {
                    editType = EditType.Name;
                }
                else if (fieldName == "description")
                {
                    editType = EditType.Description;
                }
                else if (fieldName == "deadline")
                {
                    editType = EditType.Deadline;
                }
                else if (fieldName == "completed")
                {
                    editType = EditType.IsCompleted;
                }
                else
                {
                    Console.WriteLine("Please enter 'name', 'description', 'deadline' or 'completed'.");
                    continue;
                }

                break;
            }

            string newValue;

            // now we must get a valid new value for the specified field
            while (true)
            {
                // get a non-empty string to start
                string input = Utils.GetNonEmptyStringInput("new value for the field: ");

                // if we are updating the deadline, try to parse the string
                // if we cannot, reprompt the user until a valid date is given.
                if (editType == EditType.Deadline)
                {
                    try
                    {
                        DateTime.Parse(input);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid date.");
                        continue;
                    }
                }
                else if (editType == EditType.IsCompleted)
                {
                    try
                    {
                        bool.Parse(input);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter either 'true' or 'false'.");
                        continue;
                    }
                }

                // at this point, the input is safe to be used as the new value
                newValue = input;
                break;
            }

            // let the manager handle editing the underlying Task
            // _manager.Edit(toEdit, editType, newValue);

            switch (editType)
            {
                case EditType.Name: _manager.EditName(toEdit, newValue);
                    break;
                case EditType.Description: _manager.EditDescription(toEdit, newValue);
                    break;
                case EditType.Deadline: _manager.EditDeadline(toEdit, DateTime.Parse(newValue));
                    break;
                case EditType.IsCompleted: _manager.EditIsCompleted(toEdit, bool.Parse(newValue));
                    break;
            }

            Console.WriteLine("\nTo Do updated succesfully!\n");
        }

        /// <summary>
        /// Parse user for input and then mark selected Task as complete.
        /// </summary>
        public void Complete()
        {
            Console.WriteLine();

            // get a valid integer in the correct range, then let the manager handle marking it complete
            int toComplete = Utils.GetIndexInput("index of the task to complete: ", _manager.Count - 1);
            _manager.Complete(toComplete);

            Console.WriteLine("\nTo Do marked as complete succesfully!\n");
        }

        /// <summary>
        /// Prompt user for input and then list the managed Tasks in requested mode.
        /// </summary>
        public void List()
        {
            Console.WriteLine();

            ListMode mode;

            while (true)
            {
                // get a non-empty string
                string input = Utils.GetNonEmptyStringInput("incomplete OR all: ");

                // we must make sure the input was valid. if not, reprompt until a correct
                // answer is given
                if (input == "incomplete")
                {
                    mode = ListMode.NotComplete;
                }
                else if (input == "all")
                {
                    mode = ListMode.All;
                }
                else
                {
                    Console.WriteLine("Please enter either 'incomplete' or 'all'.");
                    continue;
                }

                break;
            }

            // finally, input is safe, so print the list
            Console.WriteLine("\nYour To-Do List");
            Console.WriteLine("---------------");
            _manager.List(mode);

            Console.WriteLine();
        }

        /// <summary>
        /// Print a message saying the given command was not recognized.
        /// </summary>
        public void UnrecognizedCommand()
        {
            Console.WriteLine("\nThat command was not recognized. Please try again.\n");
        }
    }
}
