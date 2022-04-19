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
        private readonly DataContext db;

        public TaskController(DataContext context)
        {
            db = context;
        }

        [HttpGet()]
        public IEnumerable<ITask> Get(string? searchString = null)
        {
            return new TaskEC(db).Query(searchString).Result;
        }

        [HttpPost()]
        public StatusCodeResult Post([FromBody] ITask task) 
        {
            new TaskEC(db).AddOrUpdate(task);
            return StatusCode(200);
        }

        [HttpPost("delete")]
        public StatusCodeResult Delete([FromBody] ITask task)
        {
            new TaskEC(db).Delete(task);
            return StatusCode(200);
        }
    }
}
