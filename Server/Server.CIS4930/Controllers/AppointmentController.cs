using Lib.CIS4930.Standard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.CIS4930.Database;

namespace Server.CIS4930.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Appointment> Get()
        {
            return FakeDB.Appointments;
        }
    }
}
