using Lib.CIS4930.Standard.Models;
using Lib.CIS4930.Standard.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.CIS4930.Standard.Services
{
    public class RemoteTaskService : ITaskService
    {
        private static ITaskService _instance;
        public static ITaskService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RemoteTaskService();
                }
                return _instance;
            }
        }

        public List<ITask> Tasks { get; private set; }

        private string _baseURL;
        private string _tasksEndpoint => _baseURL + "tasks";

        private string _deleteEndpoint => _tasksEndpoint + "/delete";

        private string _searchEndpoint => _tasksEndpoint + "?searchString=";

        private RemoteTaskService()
        {
            _baseURL = "http://localhost:46993/api/";
            Load();
        }

        public void Load()
        {
            var res = new WebRequestHandler().Get(_tasksEndpoint).Result;
            Tasks = JsonConvert.DeserializeObject<List<ITask>>(res) ?? new List<ITask>();
        }

        public void Save()
        {
            
        }

        public async void Save(ITask task)
        {
            await new WebRequestHandler().Post(_tasksEndpoint, task);
        }

        public async void Delete(ITask task)
        {
            await new WebRequestHandler().Post(_deleteEndpoint, task);
        }

        public async Task Search(string query)
        {
            string endpoint;

            if (query == null)
                endpoint = _tasksEndpoint;
            else
                endpoint = _searchEndpoint + query;

            var res = await new WebRequestHandler().Get(endpoint);
            Tasks = JsonConvert.DeserializeObject<List<ITask>>(res) ?? new List<ITask>();
        }
    }
}
