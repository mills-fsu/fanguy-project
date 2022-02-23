using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
/*using Lib.CIS4930.Models;
using Lib.CIS4930.Services;*/

namespace UWP.CIS4930.ViewModels
{
    public class MainPageViewModel
    {
        /*private ITaskService taskService;
        private ObservableCollection<ITask> tasks;

        public ObservableCollection<ITask> Tasks
        {
            get
            {
                tasks.Clear();
                taskService.Tasks.ForEach(tasks.Add);
                return tasks;
            }
        }*/

        public MainPageViewModel()
        {
            //taskService = LocalTaskService.Instance;
        }

        public void Search()
        {

        }

        public void Add()
        {

        }

        public void Delete()
        {

        }

        public void Edit()
        {

        }
    }
}
