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

        [HttpPost("TaskTaken")]  // This should be HttpPost, not HttpGet!
        public IActionResult TaskTaken([FromBody] TaskDTO TDTO)
        {
            //BLL_Employee? emp = BLL_Employee.Find(TDTO.CompletedBy ?? -1);
            //if (emp != null)
            //{
            //    emp.TaskListedByThisUser.Add(TDTO);
            //    return Ok("Notification received.");
            //}
            //return BadRequest();
            
            
            return Ok("Notification of Tooken received.");
        }

        [HttpPost("TaskCompleted")]  // This should be HttpPost, not HttpGet!
        public IActionResult TaskCompleted([FromBody] TaskDTO TDTO)
        {
            //BLL_Employee? emp = BLL_Employee.Find(TDTO.CompletedBy ?? -1);
            //if (emp != null)
            //{
            //    emp.TaskListedByThisUser.Add(TDTO);
            //    return Ok("Notification received.");
            //}
            //return BadRequest();


            return Ok("Notification of Completed received.");
        }

        [HttpPost("TaskCancelled")]  // This should be HttpPost, not HttpGet!
        public IActionResult TaskCancelled([FromBody] TaskDTO TDTO)
        {
            //BLL_Employee? emp = BLL_Employee.Find(TDTO.CompletedBy ?? -1);
            //if (emp != null)
            //{
            //    emp.TaskListedByThisUser.Add(TDTO);
            //    return Ok("Notification received.");
            //}
            //return BadRequest();


            return Ok("Notification of Cancelled received.");
        }

    }
}
