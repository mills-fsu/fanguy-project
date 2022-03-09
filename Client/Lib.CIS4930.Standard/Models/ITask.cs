using Lib.CIS4930.Standard.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.CIS4930.Standard.Models
{
    [JsonConverter(typeof(TaskJsonConverter))]
    public interface ITask
    {
        string Name { get; set; }
        string Description { get; set; }
        int Priority { get; set; }
    }
}
