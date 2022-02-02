using Lib.CIS4930.Models;

namespace Lib.CIS4930
{
    internal interface ITaskService
    {
        public static ITaskService Instance { get; }
        public List<ITask> Tasks { get; }

        public void Save();

        public void Load();
    }
}
