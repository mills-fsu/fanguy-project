using Lib.CIS4930.Standard.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lib.CIS4930.Standard.Services
{
    public class LocalTaskService : ITaskService
    {
        private static LocalTaskService _instance;

        /// <summary>
        /// The singleton instance of this class.
        /// </summary>
        public static ITaskService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LocalTaskService();

                return _instance;
            }
        }

        /// <summary>
        /// The list of ITasks to be managed by the application.
        /// </summary>
        public List<ITask> Tasks { get; private set; }

        // the file name for the save data
        private string _saveFile;

        // the settings to use in the JSON conversion
        private JsonSerializerSettings _serializerSettings;

        private LocalTaskService()
        {
            // create the save directory if it does not exist yet
            var saveDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CIS4930");
            if (!File.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            // we know the saveDir exists now, so append the filename to it in order to get the save file
            _saveFile = Path.Combine(saveDir, "tasks.json");

            // this is necessary in order to serialize a list of interfaces
            _serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
            };

            // finally, load the data from the file if possible
            Load();
        }

        /// <summary>
        /// Save the Tasks list to a JSON file.
        /// </summary>
        public void Save()
        {
            var jsonString = JsonConvert.SerializeObject(Tasks, _serializerSettings);
            File.WriteAllText(_saveFile, jsonString);
        }

        public void Save(ITask task)
        {
            Save();
        }

        public void Delete(ITask task)
        {
            Save();
        }

        /// <summary>
        /// Load the JSON file and initialize the Tasks list with its data if possible; otherwise
        /// set Tasks to an empty list.
        /// </summary>
        public void Load()
        {
            if (File.Exists(_saveFile))
            {
                var jsonString = File.ReadAllText(_saveFile);
                Tasks = JsonConvert.DeserializeObject<List<ITask>>(jsonString, _serializerSettings) ?? new List<ITask>();
            }
            else
                Tasks = new List<ITask>();
        }

        public async Task Search(string query)
        {
            // pass
        }
    }
}
