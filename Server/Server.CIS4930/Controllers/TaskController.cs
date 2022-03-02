using Lib.CIS4930.Standard.Models;
using Lib.CIS4930.Standard.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Server.CIS4930.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskService taskService;

        public TaskController()
        {
            taskService = LocalTaskService.Instance;
        }

        [HttpGet()]
        public IEnumerable<ITask> Get()
        {
            return taskService.Tasks;
        }
    }
}
