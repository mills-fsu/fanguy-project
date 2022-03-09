using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.CIS4930.Standard.Models
{
    [Serializable]
    public class ToDo : ITask
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
        /// The importance of the task between 1 and 5, where 5 is most important.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// When the Task should be completed by
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Whether or not the Task has been marked complete
        /// </summary>
        public bool IsCompleted { get; set; }

        public ToDo()
        {
            Name = "";
            Description = "";
            Deadline = DateTime.Now;
            IsCompleted = false;
        }

        public ToDo(string name, string description, DateTime deadline, bool isCompleted = false)
        {
            Name = name;
            Description = description;
            Deadline = deadline;
            IsCompleted = isCompleted;
        }

        public override string ToString()
        {
            // if completed, show a filled checkbox, empty otherwise
            var checkbox = IsCompleted ? "✔️" : "🔘";

            // convert the deadline to the format m/d/yyyy
            var dueDate = Deadline.ToShortDateString();

            StringBuilder stars = new StringBuilder();
            for (int i = 0; i < Priority && i < 5; i++)
            {
                stars.Append("⭐");
            }

            // finally, return these values formatted nicely
            // note 7 spaces before notes, this is 3 for the index + :, plus 4 for the '[ ] '
            return $"{checkbox} {Name} (due {dueDate})\n       priority: {stars}\n       notes: {Description}";
        }
    }
}
