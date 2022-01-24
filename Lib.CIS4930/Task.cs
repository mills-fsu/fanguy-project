using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assignment1
{
    public class Task
    {
        /// <summary>
        /// The display name of the to-do Task
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A short description of the Task, such as steps to take or other notes
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// When the Task should be completed by
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Whether or not the Task has been marked complete
        /// </summary>
        public bool IsCompleted { get; set; }

        public Task(string name, string description, DateTime deadline, bool isCompleted = false)
        {
            Name = name;
            Description = description;
            Deadline = deadline;
            IsCompleted = isCompleted;
        }

        public override string ToString()
        {
            // if completed, show a filled checkbox, empty otherwise
            var checkbox = IsCompleted ? "[X]" : "[ ]";

            // convert the deadline to the format m/d/yyyy
            var dueDate = Deadline.ToShortDateString();

            // finally, return these values formatted nicely
            // note 7 spaces before notes, this is 3 for the index + :, plus 4 for the '[ ] '
            return $"{checkbox} {Name} (due {dueDate})\n       notes: {Description}";
        }
    }
}
