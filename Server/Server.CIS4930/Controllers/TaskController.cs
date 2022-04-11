using Lib.CIS4930.Standard.Models;
using Lib.CIS4930.Standard.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.CIS4930.Database;

namespace Server.CIS4930.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        [HttpGet()]
        public IEnumerable<ITask> Get()
        {
            return FakeDB.Tasks;
        }

        [HttpPost()]
        public StatusCodeResult Post([FromBody] ITask task) 
        {
            new TaskEC().AddOrUpdate(task);
            return StatusCode(200);
        }

        [HttpDelete()]
        public StatusCodeResult Delete([FromBody] ITask task)
        {
            new TaskEC().Delete(task);
            return StatusCode(200);
        }
    }
}
