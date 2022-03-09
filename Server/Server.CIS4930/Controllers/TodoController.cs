using Lib.CIS4930.Standard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.CIS4930.Database;

namespace Server.CIS4930.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<ToDo> Get()
        {
            return FakeDB.Todos;
        }
    }
}
