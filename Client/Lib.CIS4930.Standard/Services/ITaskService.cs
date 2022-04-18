using Lib.CIS4930.Standard.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lib.CIS4930.Standard.Services
{
    public interface ITaskService
    {
        //ITaskService Instance { get; }
        List<ITask> Tasks { get; }

        void Save();
        void Save(ITask task);
        void Delete(ITask task);
        Task Search(string query);

        void Load();
    }
}
