using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assignment1
{
    public class TaskManager
    {
        /// <summary>
        /// The type of edit action we are taking, either updating the
        /// name, description, or deadline of one of the tasks.
        /// </summary>
        public enum EditType
        {
            Name,
            Description,
            Deadline,
            IsCompleted
        }

        /// <summary>
        /// How to display the list, either showing all items or only
        /// incomplete ones.
        /// </summary>
        public enum ListMode
        {
            All,
            NotComplete
        }

        /// <summary>
        /// Returns the number of current tasks, both complete and incomplete.
        /// </summary>
        public int Count { get => _tasks.Count; }

        private List<Task> _tasks;

        public TaskManager()
        {
            _tasks = new List<Task>();
        }

        /// <summary>
        /// Create a new Task that this manager will track.
        /// </summary>
        /// <param name="name">the display name of the task</param>
        /// <param name="description">a brief description of the task</param>
        /// <param name="deadline">when the task is due, in a valid DateTime</param>
        public void Create(string name, string description, DateTime deadline)
        {
            _tasks.Add(new Task(name, description, deadline));
        }

        /// <summary>
        /// Remove the Task at the given index from the manager.
        /// </summary>
        /// <param name="index">the zero-based index to delete at</param>
        public void Delete(int index)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _tasks.Count)
            {
                return;
            }

            _tasks.RemoveAt(index);
        }

        /// <summary>
        /// Edit the name, description, or deadline of the Task at the given index.
        /// </summary>
        /// <param name="index">zero-based index of the task to edit</param>
        /// <param name="type">whether to update the name, description, or deadline</param>
        /// <param name="newValue">a string represented the new value. must be a valid date string if type is for Deadline</param>
        [Obsolete("Edit is obsolete. Use individual EditName, EditDescription, etc. instead.")]
        public void Edit(int index, EditType type, string newValue)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _tasks.Count)
            {
                return;
            }

            switch (type)
            {
                case EditType.Name:
                {
                    _tasks[index].Name = newValue;
                    break;
                }
                case EditType.Description:
                {
                    _tasks[index].Description = newValue;
                    break;
                }
                case EditType.Deadline:
                {
                    // it's not great, but the DateTime is passed in as a string so that
                    // there can be a single Edit method instead of 2 or 3 separate ones.
                    // not sure if this is actually better, but the public interface is somewhat
                    // nicer maybe? regardless, assume the date is well-formatted and parse it
                    DateTime deadline = DateTime.Parse(newValue);
                    _tasks[index].Deadline = deadline;
                    break;
                }
            }
        }

        /// <summary>
        /// Update the name of the Task at the given index.
        /// </summary>
        /// <param name="index">the index of the Task to update</param>
        /// <param name="newName">the new name for this Task</param>
        public void EditName(int index, string newName)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _tasks.Count)
            {
                return;
            }

            _tasks[index].Name = newName;
        }

        /// <summary>
        /// Update the description of the Task at the given index.
        /// </summary>
        /// <param name="index">the index of the Task to update</param>
        /// <param name="newDescription">the new description for this Task</param>
        public void EditDescription(int index, string newDescription)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _tasks.Count)
            {
                return;
            }

            _tasks[index].Description = newDescription;
        }

        /// <summary>
        /// Update the deadline of the Task at the given index.
        /// </summary>
        /// <param name="index">the index of the Task to update</param>
        /// <param name="newDeadline">the new deadline for this Task</param>
        public void EditDeadline(int index, DateTime newDeadline)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _tasks.Count)
            {
                return;
            }

            _tasks[index].Deadline = newDeadline;
        }

        /// <summary>
        /// Update the completion status of the Task at the given index.
        /// </summary>
        /// <param name="index">the index of the Task to update</param>
        /// <param name="newIsCompleted">the new completed value for this Task</param>
        public void EditIsCompleted(int index, bool newIsCompleted)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _tasks.Count)
            {
                return;
            }

            _tasks[index].IsCompleted = newIsCompleted;
        }

        /// <summary>
        /// Mark a Task as complete.
        /// </summary>
        /// <param name="index">zero-based index of the Task to mark complete</param>
        public void Complete(int index)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _tasks.Count)
            {
                return;
            }

            _tasks[index].IsCompleted = true;
        }

        /// <summary>
        /// Print the list of Tasks to the console, dictated by the given mode
        /// </summary>
        /// <param name="mode">what Tasks to show, either all of them or only incomplete ones</param>
        public void List(ListMode mode)
        {
            int displayed = 0;
            for (int i = 0; i < _tasks.Count; i++)
            {
                Task task = _tasks[i];
                if (task.IsCompleted && mode == ListMode.NotComplete)
                {
                    continue;
                }

                Console.WriteLine($"{i}: {task}");
                displayed++;
            }

            if (displayed == 0)
            {
                Console.WriteLine("Nothing to do for now!");
            }
        }
    }
}
