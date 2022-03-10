using Lib.CIS4930.Standard.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
        private string _savePath;
        private string _saveFileName;

        private string SaveFile { get => Path.Combine(_savePath, _saveFileName); }

        // the settings to use in the JSON conversion
        private JsonSerializerSettings _serializerSettings;

        private LocalTaskService()
        {
            _saveFileName = "tasks.json";

            // create the save directory if it does not exist yet
            _savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CIS4930");
            if (!File.Exists(_savePath))
                Directory.CreateDirectory(_savePath);

            // we know the saveDir exists now, so append the filename to it in order to get the save file
            // _savePath = Path.Combine(_savePath, "tasks.json");

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
            File.WriteAllText(SaveFile, jsonString);
        }

        /// <summary>
        /// Load the JSON file and initialize the Tasks list with its data if possible; otherwise
        /// set Tasks to an empty list.
        /// </summary>
        public void Load()
        {
            if (File.Exists(SaveFile))
            {
                var jsonString = File.ReadAllText(SaveFile);
                Tasks = JsonConvert.DeserializeObject<List<ITask>>(jsonString, _serializerSettings) ?? new List<ITask>();
            }
            else
                Tasks = new List<ITask>();
        }

        public void Load(string filename)
        {
            _saveFileName = filename + ".json";
            Load();
        }
    }
}
