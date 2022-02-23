using Lib.CIS4930.Standard.Models;
using Lib.CIS4930.Standard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib.CIS4930.Standard
{
    public class TaskManager
    {
        /// <summary>
        /// The type of edit action we are taking, either updating the
        /// name, description, or deadline of one of the tasks.
        /// </summary>
        public enum EditType
        {
            // ITask
            Name,
            Description,
            // ToDo
            Deadline,
            IsCompleted,
            // Appointment
            Start,
            End,
            Attendees
        }

        /// <summary>
        /// How to display the list of ToDos, either showing all items or only
        /// incomplete ones.
        /// </summary>
        public enum TodoFilterMode
        {
            All,
            NotComplete
        }

        /// <summary>
        /// How to display the list, either only ToDos, only appointments, or both.
        /// </summary>
        public enum ListMode
        {
            TodosOnly,
            AppointmentsOnly,
            Both
        }

        /// <summary>
        /// Represents the possible Task types, either ToDo or Appointment
        /// </summary>
        public enum TaskType
        {
            ToDo,
            Appointment
        }

        /// <summary>
        /// Returns the number of current tasks, both complete and incomplete.
        /// </summary>
        public int Count { get => _taskService.Tasks.Count; }

        /// <summary>
        /// Returns the number of Tasks of type ToDo
        /// </summary>
        public int TodosCount { get => _taskService.Tasks.Where(t => t is ToDo).Count(); }

        /// <summary>
        /// Returns the number of Tasks of type Appointment
        /// </summary>
        public int AppointmentsCount { get => _taskService.Tasks.Where(t => t is Appointment).Count(); }

        // the internally managed list of tasks
        private ITaskService _taskService;

        // defines the type of a function to call within the for loop of the List function
        private delegate bool ListCondition(int index, int displayed, int startIndex, int numTasks, int taskCount);

        public TaskManager()
        {
            _taskService = LocalTaskService.Instance;
        }

        /// <summary>
        /// Get the type of the Task at the given index
        /// </summary>
        /// <param name="index">The index of the Task to check</param>
        /// <returns>the type of the Task at that index, either ToDo or Appointment</returns>
        public TaskType TypeOf(int index)
        {
            var task = _taskService.Tasks[index];

            if (task is ToDo)
                return TaskType.ToDo;
            else
                return TaskType.Appointment;
        }

        /// <summary>
        /// Create a new ToDo that this manager will track.
        /// </summary>
        /// <param name="name">the display name of the ToDo</param>
        /// <param name="description">a brief description of the ToDo</param>
        /// <param name="deadline">when the ToDo is due</param>
        public void Create(string name, string description, DateTime deadline)
        {
            _taskService.Tasks.Add(new ToDo(name, description, deadline));
            _taskService.Save();
        }

        /// <summary>
        /// Create a new Appointment that this manager will track.
        /// </summary>
        /// <param name="name">the display name of the Appointment</param>
        /// <param name="description">a brief description of the Appointment</param>
        /// <param name="start">when the Appointment starts</param>
        /// <param name="end">when the Appointment ends</param>
        /// <param name="attendees">a list of the other attendees</param>
        public void Create(string name, string description, DateTime start, DateTime end, IEnumerable<string> attendees)
        {
            _taskService.Tasks.Add(new Appointment(name, description, start, end, attendees.ToList()));
            _taskService.Save();
        }

        /// <summary>
        /// Remove the Task at the given index from the manager.
        /// </summary>
        /// <param name="index">the zero-based index to delete at</param>
        public void Delete(int index)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _taskService.Tasks.Count)
            {
                return;
            }

            _taskService.Tasks.RemoveAt(index);
            _taskService.Save();
        }

        /// <summary>
        /// Update the name of the Task at the given index.
        /// </summary>
        /// <param name="index">the index of the Task to update</param>
        /// <param name="newName">the new name for this Task</param>
        public void EditName(int index, string newName)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _taskService.Tasks.Count)
            {
                return;
            }

            _taskService.Tasks[index].Name = newName;
            _taskService.Save();
        }

        /// <summary>
        /// Update the description of the Task at the given index.
        /// </summary>
        /// <param name="index">the index of the Task to update</param>
        /// <param name="newDescription">the new description for this Task</param>
        public void EditDescription(int index, string newDescription)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _taskService.Tasks.Count)
            {
                return;
            }

            _taskService.Tasks[index].Description = newDescription;
            _taskService.Save();
        }

        /// <summary>
        /// Update the deadline of the Task at the given index.
        /// </summary>
        /// <param name="index">the index of the Task to update</param>
        /// <param name="newDeadline">the new deadline for this Task</param>
        public void EditDeadline(int index, DateTime newDeadline)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _taskService.Tasks.Count)
            {
                return;
            }

            var task = _taskService.Tasks[index];

            if (task is ToDo todo)
                todo.Deadline = newDeadline;

            _taskService.Save();
        }

        /// <summary>
        /// Update the completion status of the Task at the given index.
        /// </summary>
        /// <param name="index">the index of the Task to update</param>
        /// <param name="newIsCompleted">the new completed value for this Task</param>
        public void EditIsCompleted(int index, bool newIsCompleted)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _taskService.Tasks.Count)
            {
                return;
            }

            var task = _taskService.Tasks[index];

            if (task is ToDo todo)
                todo.IsCompleted = newIsCompleted;

            _taskService.Save();
        }

        /// <summary>
        /// Update the start date of the Task at the given index.
        /// </summary>
        /// <param name="index">the index of the Task to update</param>
        /// <param name="newStart">the new start value for this Task</param>
        public void EditStart(int index, DateTime newStart)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _taskService.Tasks.Count)
            {
                return;
            }

            var task = _taskService.Tasks[index];

            if (task is Appointment appt)
                appt.Start = newStart;

            _taskService.Save();
        }

        /// <summary>
        /// Update the end date of the Task at the given index.
        /// </summary>
        /// <param name="index">the index of the Task to update</param>
        /// <param name="newEnd">the new end value for this Task</param>
        public void EditEnd(int index, DateTime newEnd)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _taskService.Tasks.Count)
            {
                return;
            }

            var task = _taskService.Tasks[index];

            if (task is Appointment appt)
                appt.Start = newEnd;

            _taskService.Save();
        }

        /// <summary>
        /// Update the attendees of the Task at the given index.
        /// </summary>
        /// <param name="index">the index of the Task to update</param>
        /// <param name="newAttendees">the new attendees value for this Task</param>
        public void EditAttendees(int index, IEnumerable<string> newAttendees)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _taskService.Tasks.Count)
            {
                return;
            }

            var task = _taskService.Tasks[index];

            if (task is Appointment appt)
                appt.Attendees = newAttendees.ToList();

            _taskService.Save();
        }

        /// <summary>
        /// Mark a Task as complete.
        /// </summary>
        /// <param name="index">zero-based index of the Task to mark complete</param>
        public void Complete(int index)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _taskService.Tasks.Count)
            {
                return;
            }

            var task = _taskService.Tasks[index];

            if (task is ToDo todo)
                todo.IsCompleted = true;

            _taskService.Save();
        }

        /// <summary>
        /// Print the page of ToDos to the console, dictated by the given mode
        /// </summary>
        /// <param name="mode">whether we are in ToDos only or Both mode</param>
        /// <param name="filter">whether to show incomplete or all ToDos</param>
        /// <param name="startIndex">the starting index for the page</param>
        /// <param name="numTasks">the number of tasks to show per page</param>
        public void ListToDos(ListMode mode, TodoFilterMode filter, int startIndex, int numTasks)
        {
            // choose which for loop condition we will use based on the list mode
            ListCondition condition = BothCondition;

            if (mode == ListMode.TodosOnly)
                condition = OnlyCondition;

            int displayed = 0;
            for (int i = startIndex; condition(i, displayed, startIndex, numTasks, _taskService.Tasks.Count); i++)
            {
                // get the task and coerce it into a ToDo if possible
                var task = _taskService.Tasks[i];
                if (!(task is ToDo))
                    continue;

                var todo = task as ToDo;

                if (todo.IsCompleted && filter == TodoFilterMode.NotComplete)
                    continue;

                Console.WriteLine($"{i}: {task}");
                displayed++;
            }

            if (displayed == 0)
                Console.WriteLine("Nothing to do for now!");
        }

        /// <summary>
        /// Print the page of Appointments to the console, dictated by the given mode
        /// </summary>
        /// <param name="mode">whether we are in Appointments only or Both mode</param>
        /// <param name="startIndex">the starting index for the page</param>
        /// <param name="numTasks">the number of tasks to show per page</param>
        public void ListAppointments(ListMode mode, int startIndex, int numTasks)
        {
            // choose which for loop condition we will use based on the list mode
            ListCondition condition = BothCondition;

            if (mode == ListMode.TodosOnly)
                condition = OnlyCondition;

            int displayed = 0;
            for (int i = startIndex; condition(i, displayed, startIndex, numTasks, _taskService.Tasks.Count); i++)
            {
                // only print this task if it is an appointment
                var task = _taskService.Tasks[i];
                if (!(task is Appointment))
                    continue;

                Console.WriteLine($"{i}: {task}");
                displayed++;
            }

            if (displayed == 0)
                Console.WriteLine("No appointments for now!");
        }

        /// <summary>
        /// Search all possible data fields of every Task for the search term and print the results
        /// </summary>
        /// <param name="searchTerm">the substring to check each field for</param>
        public void Search(string searchTerm)
        {
            // we will do all comparisons ignoring case, so this just reduces having to retype the whole
            // thing on every comparison
            var ignoreCase = StringComparison.OrdinalIgnoreCase;

            var results = new List<ITask>();
            foreach (var task in _taskService.Tasks)
            {
                // check both the name and description first
                if (task.Name.Contains(searchTerm) || task.Description.Contains(searchTerm))
                {
                    results.Add(task);
                    continue;
                }

                // check the special data fields of a ToDo
                if (task is ToDo todo)
                {
                    if (todo.Deadline.ToLongDateString().Contains(searchTerm))//, ignoreCase))
                    {
                        results.Add(task);
                        continue;
                    }
                }
                // check the special data fields of an Appointment
                else if (task is Appointment appt)
                {
                    if (appt.Start.ToLongDateString().Contains(searchTerm))//, ignoreCase))
                    {
                        results.Add(task);
                        continue;
                    }

                    if (appt.End.ToLongDateString().Contains(searchTerm))//, ignoreCase))
                    {
                        results.Add(task);
                        continue;
                    }

                    foreach (var attendee in appt.Attendees)
                    {
                        if (attendee.Contains(searchTerm))//, ignoreCase))
                        {
                            results.Add(task);
                            continue;
                        }
                    }
                }
            }

            // we do a separate for loop over the entire list of tasks so that we can preserve the correct
            // index for each task in the output; slightly inefficient, but the best way I could think
            // of to easily preserve this information
            for (int i = 0; i < _taskService.Tasks.Count; i++)
            {
                var task = _taskService.Tasks[i];

                if (results.Contains(task))
                {
                    Console.WriteLine($"{i}: {task}");
                }
            }
        }

        /// <summary>
        /// The for loop condition when the list is in ToDo or Appointment only mode
        /// </summary>
        /// <param name="index">current index of the for loop</param>
        /// <param name="displayed">the number of tasks that have been displayed</param>
        /// <param name="startIndex">the starting index of the current page</param>
        /// <param name="numTasks">the desired number of tasks to display on this page</param>
        /// <param name="taskCount">the overall size of the _taskService.Tasks array</param>
        /// <returns>true if the foor loop should continue, false otherwise</returns>
        private bool OnlyCondition(int index, int displayed, int startIndex, int numTasks, int taskCount)
        {
            // continue if we have not reached the end of the list and we have not already displayed
            // the maximum number of tasks per page
            return index < taskCount && displayed < numTasks;
        }

        /// <summary>
        /// The for loop condition when the list is in Both mode
        /// </summary>
        /// <param name="index">current index of the for loop</param>
        /// <param name="displayed">the number of tasks that have been displayed</param>
        /// <param name="startIndex">the starting index of the current page</param>
        /// <param name="numTasks">the desired number of tasks to display on this page</param>
        /// <param name="taskCount">the overall size of the _taskService.Tasks array</param>
        /// <returns>true if the foor loop should continue, false otherwise</returns>
        private bool BothCondition(int index, int displayed, int startIndex, int numTasks, int taskCount)
        {
            // continue if we have not reached the end of the list and the current index is within the list of
            // indices for this page
            return index < taskCount && index < startIndex + numTasks;
        }
    }
}
