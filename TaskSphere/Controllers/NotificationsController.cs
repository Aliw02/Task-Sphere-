using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL;
using BLL;

namespace TaskSphere.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/TaskSphere/Notifications")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        [HttpPost("TaskListting")]  // This should be HttpPost, not HttpGet!
        public IActionResult TaskListting([FromBody] TaskDTO TDTO)
        {
            BLL_Employee? emp = BLL_Employee.Find(TDTO.ListedBy);
            if (emp != null)
            {
                emp.TaskListedByThisUser.Add(TDTO);
                return Ok("Notification received.");
            }
            return BadRequest();
        }

        // Other methods...
    }
}
