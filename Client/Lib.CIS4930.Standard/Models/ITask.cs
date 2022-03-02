using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.CIS4930.Standard.Models
{
    public interface ITask
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}
