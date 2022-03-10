using Lib.CIS4930.Standard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP.CIS4930.ViewModels
{
    internal class OpenDialogViewModel
    {
        private string fileName;
        public string FileName
        {
            get => fileName;
            set
            {
                fileName = value;

            }
        }

        private ITaskService taskService;

        public OpenDialogViewModel(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        public void Confirm()
        {
            taskService.Load(FileName);
        }
    }
}
