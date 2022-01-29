using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.CIS4930
{
    public interface ITask
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
