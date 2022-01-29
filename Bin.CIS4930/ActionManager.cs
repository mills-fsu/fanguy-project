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
        /// Prompt user for information and then create a new managed Task.
        /// </summary>
        public void Create()
        {
            var type = Utils.GetTaskType();

            // name is required, so wait until it can be gotten
            string name = Utils.GetNonEmptyStringInput("name: ");

            // deadline may be empty, so only prompt once
            Console.Write("description (optional): ");
            string? input = Console.ReadLine();
            // prefer the empty string to null
            string description = input ?? "";

            if (type == TaskType.ToDo)
                CreateToDo(name, description);
            else if (type == TaskType.Appointment)
                CreateAppointment(name, description);

            Utils.PrintGreen("\nTask added succesfully!");
        }

        private void CreateToDo(string name, string description)
        {
            var deadline = Utils.GetValidDate("deadline: ");

            // finally, input has been sanitized, so let the manager handle creation
            _manager.Create(name, description, deadline);
        }

        private void CreateAppointment(string name, string description)
        {
            var start = Utils.GetValidDate("starting date (include time): ");
            var hours = Utils.GetValidInteger("event length (in hours): ");
            var end = start.AddHours(hours);

            Console.WriteLine("other attendees (separated by commas): ");
            var rawAttendees = Console.ReadLine() ?? "";
            var attendees = rawAttendees.Split(',').ToList();
            attendees.ForEach(a => a = a.Trim());

            _manager.Create(name, description, start, end, attendees);
        }

        /// <summary>
        /// Prompt user for input and then delete selected Task.
        /// </summary>
        public void Delete()
        {
            // get a valid integer in the correct range, then let the manager delete at that point
            int toDelete = Utils.GetIndexInput("index of the task to delete: ", _manager.Count - 1);
            _manager.Delete(toDelete);

            Utils.PrintGreen("\nTask deleted successfully!");
        }

        /// <summary>
        /// Prompt user for input and then edit selected Task.
        /// </summary>
        public void Edit()
        {
            // get a valid integer in the correct range
            int toEdit = Utils.GetIndexInput("index of the task to edit: ", _manager.Count - 1);

            var type = _manager.TypeOf(toEdit);

            EditType editType = type switch
            {
                TaskType.ToDo => GetTodoEditType(),
                TaskType.Appointment => GetAppointmentEditType()
            };

            string newValue;

            // now we must get a valid new value for the specified field
            while (true)
            {
                // get a non-empty string to start
                string input = Utils.GetNonEmptyStringInput("new value for the field: ");

                // if we are updating the deadline, try to parse the string
                // if we cannot, reprompt the user until a valid date is given.
                if (editType == EditType.Deadline || editType == EditType.Start || editType == EditType.End)
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
                case EditType.Start: _manager.EditStart(toEdit, DateTime.Parse(newValue));
                    break;
                case EditType.End: _manager.EditEnd(toEdit, DateTime.Parse(newValue));
                    break;
                case EditType.Attendees: _manager.EditAttendees(toEdit, newValue.Split(","));
                    break;
            }

            Utils.PrintGreen("\nTask updated succesfully!");
        }

        private EditType GetTodoEditType()
        {
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
            return editType;
        }

        private EditType GetAppointmentEditType()
        {
            EditType editType;
            // then, get a valid editType
            while (true)
            {
                // get a non-empty string
                string fieldName = Utils.GetNonEmptyStringInput("what would you like to edit? (name, description, start, end, attendees): ");

                // make sure it is either name, description, or deadline. repeat this loop if not
                if (fieldName == "name")
                {
                    editType = EditType.Name;
                }
                else if (fieldName == "description")
                {
                    editType = EditType.Description;
                }
                else if (fieldName == "start")
                {
                    editType = EditType.Start;
                }
                else if (fieldName == "end")
                {
                    editType = EditType.End;
                }
                else if (fieldName == "attendees")
                {
                    editType = EditType.Attendees;
                }
                else
                {
                    Console.WriteLine("Please enter 'name', 'description', 'start', 'end', or 'attendees'.");
                    continue;
                }

                break;
            }
            return editType;
        }

        /// <summary>
        /// Parse user for input and then mark selected Task as complete.
        /// </summary>
        public void Complete()
        {
            // get a valid integer in the correct range, then let the manager handle marking it complete
            int toComplete = Utils.GetIndexInput("index of the task to complete: ", _manager.Count - 1);
            _manager.Complete(toComplete);

            Utils.PrintGreen("\nTo Do marked as complete succesfully!");
        }

        /// <summary>
        /// Prompt user for input and then list the managed Tasks in requested mode.
        /// </summary>
        public void List()
        {
            ListMode mode;

            while (true)
            {
                // get a non-empty string
                string input = Utils.GetNonEmptyStringInput("show todos only ('todo'), appointments only ('appt'), or 'both': ");

                // we must make sure the input was valid. if not, reprompt until a correct
                // answer is given
                if (input == "todo")
                    mode = ListMode.TodosOnly;
                else if (input == "appt")
                    mode = ListMode.AppointmentsOnly;
                else if (input == "both")
                    mode = ListMode.Both;
                else
                {
                    Utils.PrintRed("Please enter either 'todo', 'appt', or 'both'.");
                    continue;
                }

                break;
            }

            TodoFilterMode todoFilter = TodoFilterMode.All;

            if (mode == ListMode.TodosOnly || mode == ListMode.Both)
            {
                while (true)
                {
                    // get a non-empty string
                    string input = Utils.GetNonEmptyStringInput("show 'incomplete' todos OR 'all': ");

                    // we must make sure the input was valid. if not, reprompt until a correct
                    // answer is given
                    if (input == "incomplete")
                    {
                        todoFilter = TodoFilterMode.NotComplete;
                    }
                    else if (input == "all")
                    {
                        todoFilter = TodoFilterMode.All;
                    }
                    else
                    {
                        Console.WriteLine("Please enter either 'incomplete' or 'all'.");
                        continue;
                    }

                    break;
                }
            }

            int startIndex = 0;
            int numItems = 5;
            var maxCount = mode switch
            {
                ListMode.TodosOnly        => _manager.TodosCount,
                ListMode.AppointmentsOnly => _manager.AppointmentsCount,
                _                         => _manager.Count,
            };
            int totalPages = (int) Math.Ceiling(maxCount / 5.0f);
            int currentPage = 1;

            while (true)
            {
                Console.WriteLine("q to finish, arrow keys to navigate");
                Console.WriteLine($"page {currentPage}/{totalPages}");

                Console.WriteLine("\nYour Tasks");
                Console.WriteLine("---------------");

                if (mode == ListMode.TodosOnly || mode == ListMode.Both)
                {
                    Console.WriteLine("ToDos:");
                    _manager.ListToDos(mode, todoFilter, startIndex, numItems);
                }

                if (mode == ListMode.AppointmentsOnly || mode == ListMode.Both)
                {
                    Console.WriteLine("\nAppointments:");
                    _manager.ListAppointments(mode, startIndex, numItems);
                }

                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Q)
                    break;
                else if (key == ConsoleKey.RightArrow)
                {
                    currentPage++;
                    startIndex += 5;
                    Console.Clear();
                }
                else if (key == ConsoleKey.LeftArrow)
                {
                    currentPage--;
                    startIndex -= 5;
                    Console.Clear();
                }

                startIndex = Math.Clamp(startIndex, 0, (totalPages * 5) - 5);
                currentPage = Math.Clamp(currentPage, 1, totalPages);
            }
        }

        public void Search()
        {
            string searchTerm = Utils.GetNonEmptyStringInput("search for: ");

            Console.WriteLine("\nResults");
            Console.WriteLine("---------------");

            _manager.Search(searchTerm);
        }

        /// <summary>
        /// Print a message saying the given command was not recognized.
        /// </summary>
        public void UnrecognizedCommand()
        {
            Utils.PrintRed("\nThat command was not recognized. Please try again.\n");
        }
    }
}
