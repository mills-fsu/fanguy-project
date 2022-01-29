using Lib.CIS4930;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.Text.Json;

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

        private List<ITask> _tasks;
        private BinaryFormatter _serializer;

        private const string SAVEFILE = "tasks.bin";

        public TaskManager()
        {
            _serializer = new BinaryFormatter();
            Load();
        }

        /// <summary>
        /// Create a new Task that this manager will track.
        /// </summary>
        /// <param name="name">the display name of the task</param>
        /// <param name="description">a brief description of the task</param>
        /// <param name="deadline">when the task is due, in a valid DateTime</param>
        public void Create(string name, string description, DateTime deadline)
        {
            _tasks.Add(new ToDo(name, description, deadline));
            Save();
        }

        public void Create(string name, string description, DateTime start, DateTime end, List<string> attendees)
        {
            _tasks.Add(new Appointment(name, description, start, end, attendees));
            Save();
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
            Save();
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
            Save();
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
            Save();
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

            var task = _tasks[index];

            if (task is ToDo todo)
                todo.Deadline = newDeadline;

            Save();
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

            var task = _tasks[index];

            if (task is ToDo todo)
                todo.IsCompleted = newIsCompleted;

            Save();
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

            var task = _tasks[index];

            if (task is ToDo todo)
                todo.IsCompleted = true;

            Save();
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
                var task = _tasks[i];
                if (task is ToDo todo && todo.IsCompleted && mode == ListMode.NotComplete)
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

        private void Save()
        {
            _serializer.Serialize(File.OpenWrite(SAVEFILE), _tasks);

            /*
            string fileName = "TEST.json";
            string jsonString = JsonConvert.SerializeObject(_tasks); //JsonSerializer.Serialize(_tasks);
            File.WriteAllText(fileName, jsonString);
            */
        }

        private void Load()
        {
            if (File.Exists(SAVEFILE))
            {
                _tasks = (List<ITask>)_serializer.Deserialize(File.OpenRead(SAVEFILE));

                /*
                string fileName = "TEST.json";
                string jsonString = File.ReadAllText(fileName);
                var tasks = JsonConvert.DeserializeObject<List<ITask>>(jsonString, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                });

                foreach (var task in tasks)
                    Console.WriteLine(task);
                */
            }
            else
            {
                _tasks = new List<ITask>();
            }
        }
    }
}
