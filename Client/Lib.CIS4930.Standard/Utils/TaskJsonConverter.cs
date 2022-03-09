using Lib.CIS4930.Standard.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.CIS4930.Standard.Utils
{
    public class TaskJsonConverter : JsonCreationConverter<ITask>
    {
        protected override ITask Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            if (jObject["isCompleted"] != null || jObject["IsCompleted"] != null)
            {
                return new ToDo();
            }
            else
            {
                return new Appointment();
            }
        }
    }
}
