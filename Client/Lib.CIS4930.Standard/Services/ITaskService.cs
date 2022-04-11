using Lib.CIS4930.Standard.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.CIS4930.Standard.Services
{
    public interface ITaskService
    {
        //ITaskService Instance { get; }
        List<ITask> Tasks { get; }

        void Save();
        void Save(ITask task);
        void Delete(ITask task);

        void Load();
    }
}
