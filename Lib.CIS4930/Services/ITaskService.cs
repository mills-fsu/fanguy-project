using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.CIS4930
{
    internal interface ITaskService
    {
        public static ITaskService Instance { get; }
        public IList<ITask> Tasks { get; }

        public void Save();

        public void Load();
    }
}
