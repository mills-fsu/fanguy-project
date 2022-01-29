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
        /// How to display the list, either showing all items or only
        /// incomplete ones.
        /// </summary>
        public enum TodoFilterMode
        {
            All,
            NotComplete
        }

        public enum ListMode
        {
            TodosOnly,
            AppointmentsOnly,
            Both
        }

        public enum TaskType
        { 
            ToDo,
            Appointment
        }

        /// <summary>
        /// Returns the number of current tasks, both complete and incomplete.
        /// </summary>
        public int Count { get => _tasks.Count; }

        public int TodosCount { get => _tasks.Where(t => t is ToDo).Count(); }

        public int AppointmentsCount { get => _tasks.Where(t => t is Appointment).Count(); }


        private List<ITask> _tasks;
        private BinaryFormatter _serializer;

        private const string SAVEFILE = "tasks.bin";

        public TaskManager()
        {
            _serializer = new BinaryFormatter();
            Load();
        }

        public TaskType TypeOf(int index)
        {
            var task = _tasks[index];

            if (task is ToDo)
                return TaskType.ToDo;
            else
                return TaskType.Appointment;
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

        public void Create(string name, string description, DateTime start, DateTime end, IEnumerable<string> attendees)
        {
            _tasks.Add(new Appointment(name, description, start, end, attendees.ToList()));
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

        public void EditStart(int index, DateTime newStart)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _tasks.Count)
            {
                return;
            }

            var task = _tasks[index];

            if (task is Appointment appt)
                appt.Start = newStart;

            Save();
        }

        public void EditEnd(int index, DateTime newEnd)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _tasks.Count)
            {
                return;
            }

            var task = _tasks[index];

            if (task is Appointment appt)
                appt.Start = newEnd;

            Save();
        }

        public void EditAttendees(int index, IEnumerable<string> newAttendees)
        {
            // if outside the bounds, return without doing anything
            if (index < 0 || index >= _tasks.Count)
            {
                return;
            }

            var task = _tasks[index];

            if (task is Appointment appt)
                appt.Attendees = newAttendees.ToList();

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

        private delegate bool ListCondition(int index, int displayed, int startIndex, int numTasks, int taskCount);

        private bool OnlyCondition(int index, int displayed, int startIndex, int numTasks, int taskCount)
        {
            return index < taskCount && displayed < numTasks;
        }

        private bool BothCondition(int index, int displayed, int startIndex, int numTasks, int taskCount)
        {
            return index < taskCount && index < startIndex + numTasks;
        }

        /// <summary>
        /// Print the list of Tasks to the console, dictated by the given mode
        /// </summary>
        /// <param name="filter">what Tasks to show, either all of them or only incomplete ones</param>
        public void ListToDos(ListMode mode, TodoFilterMode filter, int startIndex, int numTasks)
        {
            ListCondition condition = mode switch
            {
                ListMode.TodosOnly => OnlyCondition,
                _                  => BothCondition
            };

            int displayed = 0;
            for (int i = startIndex; condition(i, displayed, startIndex, numTasks, _tasks.Count); i++ )
            {
                var task = _tasks[i];

                if (task is not ToDo)
                    continue;

                var todo = task as ToDo;

                if (todo.IsCompleted && filter == TodoFilterMode.NotComplete)
                    continue;

                Console.WriteLine($"{i}: {task}");
                displayed++;
            }

            if (displayed == 0)
            {
                Console.WriteLine("Nothing to do for now!");
            }
        }

        public void ListAppointments(ListMode mode, int startIndex, int numTasks)
        {
            ListCondition condition = mode switch
            {
                ListMode.AppointmentsOnly => OnlyCondition,
                _                         => BothCondition
            };

            int displayed = 0;
            for (int i = startIndex; condition(i, displayed, startIndex, numTasks, _tasks.Count); i++)
            {
                var task = _tasks[i];

                if (task is not Appointment)
                    continue;

                Console.WriteLine($"{i}: {task}");
                displayed++;
            }

            if (displayed == 0)
            {
                Console.WriteLine("No appointments for now!");
            }
        }

        public void Search(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            var ignoreCase = StringComparison.OrdinalIgnoreCase;

            List<ITask> results = new List<ITask>();

            foreach (var task in _tasks)
            {
                if (task.Name.Contains(searchTerm, ignoreCase) || task.Description.Contains(searchTerm, ignoreCase)) 
                { 
                    results.Add(task);
                    continue;
                }

                if (task is ToDo todo)
                {
                    if (todo.Deadline.ToLongDateString().Contains(searchTerm, ignoreCase)) 
                    { 
                        results.Add(task);
                        continue;
                    }
                }    
                else if (task is Appointment appt)
                {
                    if (appt.Start.ToLongDateString().Contains(searchTerm, ignoreCase))
                    {
                        results.Add(task);
                        continue;
                    }

                    if (appt.End.ToLongDateString().Contains(searchTerm, ignoreCase))
                    {
                        results.Add(task);
                        continue;
                    }

                    foreach (var attendee in appt.Attendees)
                    {
                        if (attendee.Contains(searchTerm, ignoreCase))
                        {
                            results.Add(task);
                            continue;
                        }
                    }
                }
            }

            for (int i = 0; i < _tasks.Count; i++)
            {
                var task = _tasks[i];

                if (results.Contains(task))
                {
                    Console.WriteLine($"{i}: {task}");
                }
            }
        }

        private void Save()
        {
            var file = File.OpenWrite(SAVEFILE);
            _serializer.Serialize(file, _tasks);
            file.Close();

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
                var file = File.OpenRead(SAVEFILE);
                _tasks = (List<ITask>)_serializer.Deserialize(file);
                file.Close();

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
