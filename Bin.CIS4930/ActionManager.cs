using Lib.CIS4930;
using Lib.CIS4930.Models;
using static Lib.CIS4930.TaskManager;

namespace Bin.CIS4930
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

        /// <summary>
        /// Create a new managed ToDo
        /// </summary>
        /// <param name="name">name of the ToDo</param>
        /// <param name="description">description of the ToDo</param>
        private void CreateToDo(string name, string description)
        {
            var deadline = Utils.GetValidDate("deadline: ");

            // finally, input has been sanitized, so let the manager handle creation
            _manager.Create(name, description, deadline);
        }

        /// <summary>
        /// Create a new mananged Appointment
        /// </summary>
        /// <param name="name">name of the Appointment</param>
        /// <param name="description">description of the Appointment</param>
        private void CreateAppointment(string name, string description)
        {
            // use Utils function to grab the start and end dates
            var start = Utils.GetValidDate("starting date (include time): ");
            var hours = Utils.GetValidInteger("event length (in hours): ");
            var end = start.AddHours(hours);

            // then get the attendees and use some LINQ to convert it to a
            // list of names
            Console.WriteLine("other attendees (separated by commas): ");
            var rawAttendees = Console.ReadLine() ?? "";
            var attendees = rawAttendees.Split(',').ToList();
            attendees.ForEach(a => a = a.Trim());

            // and let the _manager handle the actual creation
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

        /// <summary>
        /// Get the data field to update for a ToDo
        /// </summary>
        /// <returns>either name, description, deadline, or completed based on input</returns>
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
                    Utils.PrintRed("Please enter 'name', 'description', 'deadline' or 'completed'.");
                    continue;
                }

                break;
            }
            return editType;
        }

        /// <summary>
        /// Get the data field to update for an Appointment
        /// </summary>
        /// <returns>either name, description, start, end, or attendees</returns>
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
                    Utils.PrintRed("Please enter 'name', 'description', 'start', 'end', or 'attendees'.");
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
            // whether to display tasks only, appointments only, or both
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

            // whether to display just incomplete or all ToDos
            TodoFilterMode todoFilter = TodoFilterMode.All;

            // we only filter ToDos if we will be showing ToDos, so only prompt the user
            // if they selected ToDos only or Both
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

            Paging(mode, todoFilter);
        }

        /// <summary>
        /// Prompt the user for a search term and let the manager find the matches.
        /// </summary>
        public void Search()
        {
            string searchTerm = Utils.GetNonEmptyStringInput("search for: ");

            Console.WriteLine("\nResults");
            Console.WriteLine("---------------");

            var results = _manager.Search(searchTerm);

            Paging(ListMode.Both, TodoFilterMode.All, results);
        }

        /// <summary>
        /// Print a message saying the given command was not recognized.
        /// </summary>
        public void UnrecognizedCommand()
        {
            Utils.PrintRed("\nThat command was not recognized. Please try again.\n");
        }

        private void Paging(ListMode mode, TodoFilterMode todoFilter, List<ITask>? list = null)
        {
            // the index to start the page at
            int startIndex = 0;
            // how many items to show on the page
            int numItems = 5;
            // the count of applicable objects, depending on the list's mode
            // if we are only showing one type or the other, only get the count of those objects
            // instead of the whole _tasks list count
            int maxCount;
            if (list == null)
            {
                maxCount = mode switch
                {
                    ListMode.TodosOnly => _manager.TodosCount,
                    ListMode.AppointmentsOnly => _manager.AppointmentsCount,
                    _ => _manager.Count,
                };
            }
            else
            {
                maxCount = list.Count;
            }

            // each page has 5 items, so divide the count by 5; we must use the ceiling
            // since any decimal indicates some overflow and thus a new, partially-filled page
            int totalPages = (int)Math.Ceiling(maxCount / 5.0f);
            if (totalPages == 0)
                totalPages = 1;
            // start off at the first page
            int currentPage = 1;

            while (true)
            {
                Console.WriteLine("q to finish, arrow keys to navigate");
                Console.WriteLine($"page {currentPage}/{totalPages}");

                Console.WriteLine("\nTasks");
                Console.WriteLine("---------------");

                if (mode == ListMode.TodosOnly || mode == ListMode.Both)
                {
                    Console.WriteLine("ToDos:");
                    _manager.ListToDos(mode, todoFilter, startIndex, numItems, list);
                }

                if (mode == ListMode.AppointmentsOnly || mode == ListMode.Both)
                {
                    Console.WriteLine("\nAppointments:");
                    _manager.ListAppointments(mode, startIndex, numItems, list);
                }

                // wait until the user enters a key
                var key = Console.ReadKey(true).Key;

                // and process it
                if (key == ConsoleKey.Q)
                    break;
                else if (key == ConsoleKey.RightArrow)
                {
                    // we blindly increment these values since they will be clamped
                    currentPage++;
                    startIndex += 5;
                    Console.Clear();
                }
                else if (key == ConsoleKey.LeftArrow)
                {
                    // similarly, we blindly decrement these values
                    currentPage--;
                    startIndex -= 5;
                    Console.Clear();
                }

                // now, clamp the two pertinent values; the upper range on startIndex is a bit cryptic
                // but basically just says that we can't exceed the starting index of the final possible
                // page. we can't clamp it to the count of the entire list since that would allow a starting index
                // at the end or middle of a page, which is not desired
                startIndex = Math.Clamp(startIndex, 0, (totalPages * numItems) - numItems);
                currentPage = Math.Clamp(currentPage, 1, totalPages);
            }
        }
    }
}
