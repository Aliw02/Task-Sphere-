using BLL;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskSphere.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/TaskSphere/Tasks")]
    [ApiController]
    public class TasksAPIController : ControllerBase
    {

        // ===================================
        // =============== GET ===============
        // ===================================
        // For Get we use Http Get
        [HttpGet("{ID}", Name = "GetTaskByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TaskDTO> GetTaskByID(int ID)
        {
            if (ID < 1)
                return BadRequest($"Not Accepted ID {ID}");
            BLL_Task? Task = BLL_Task.Find(ID);
            if (Task is null)
                return NotFound($"Task with ID {ID} not found.");
            TaskDTO TDTO = Task.TDTO;
            return Ok(TDTO);
        }


        [HttpGet( "AllTasksCompletedBy/{UserID}", Name = "GetAllCompletedTasksByEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<TaskDTO>> GetAllCompletedTasksByEmployee(int UserID)
        {
            var Tasks = BLL_Task.GetAllCompletedTasksByEmployee(UserID);
            if (Tasks is null || !Tasks.Any())
                return NotFound("No Tasks Found!");
            return Ok(Tasks); // return the list of Tasks
        }


        [HttpGet("AllTasksListedBy/{UserID}", Name = "GetAllListedTasksByEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<TaskDTO>> GetAllListedTasksByEmployee(int UserID)
        {
            var Tasks = BLL_Task.GetAllListedTasksByEmployee(UserID);
            if (Tasks is null || !Tasks.Any())
                return NotFound("No Tasks Found!");
            return Ok(Tasks); // return the list of Tasks
        }



        [HttpGet("GetAllTasksBySpecificStatus/{StatusID}", Name = "GetAllTasksBySpecificStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<TaskDTO>> GetAllTasksBySpecificStatus(int StatusID)
        {
            var Tasks = BLL_Task.GetAllTasksBySpecificStatus(StatusID);
            if (Tasks is null || !Tasks.Any())
                return NotFound("No Tasks Found!");
            return Ok(Tasks); // return the list of Tasks
        }



        [HttpGet ("GetAllTasks", Name = "GetAllTasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<TaskDTO>> GetAllTasks()
        {
            var Tasks = BLL_Task.GetAll();
            if (Tasks is null || !Tasks.Any())
                return NotFound("No Tasks Found!");


            return Ok(Tasks); // return the list of Tasks
        }



        // ====================================
        // =============== POST ===============
        // ====================================
        // For add new we use Http Post
        [HttpPost("AddNewTask", Name = "AddTask")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TaskDTO> AddTask([FromBody] TaskDTO TDTO)
        {
            if(TDTO is null || string.IsNullOrEmpty(TDTO.TaskName) || TDTO.ListedBy < 0)
                return BadRequest("Task not added");

            BLL_Task Task = new(TDTO);

            if (Task.Save())
                return CreatedAtRoute("GetTaskByID", new { ID = Task.TaskID }, Task.TDTO);
            return BadRequest("Task not added");
        }



        // ====================================
        // =============== PUT ===============
        // ====================================
        // For update we use Http Put
        [HttpPut("CompleteTask/{ID}", Name = "CompleteTask")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CompleteTask(int ID, int CompletedBy)
        {
            BLL_Task? Task = BLL_Task.Find(ID);
            if (Task is null)
                return NotFound("Task not found");

            if (Task.TaskStatus == BLL_Task.enTaskStatus.InProgress)
            {
                if(Task.Complete(CompletedBy))
                    return Ok("Task Completed Successfully.");
                else
                return BadRequest("Task allready completed or deleted!");
            }
            else
                return BadRequest("Task not Started yet!");

        }


        [HttpPut("TakeTask/{ID}", Name = "TakeTask")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult TakeTask(int ID)
        {
            BLL_Task? Task = BLL_Task.Find(ID);
            if (Task is null)
                return NotFound("Task not found");

            if (Task.TaskStatus == BLL_Task.enTaskStatus.NotStarted)
            {
                if (Task.TakeTask())
                {
                    return Ok("Task Taken Successfully.");
                }
            }
            return BadRequest("Task allready taken or completed!");
        }

        [HttpPut("CancelTask/{ID}", Name = "CancelTask")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CancelTask(int ID)
        {
            BLL_Task? Task = BLL_Task.Find(ID);

            if (Task is null)
                return NotFound("Task not found");

            if (Task.TaskStatus == BLL_Task.enTaskStatus.Completed || Task.TaskStatus == BLL_Task.enTaskStatus.Cancelled)
                return BadRequest("Task allready completed or deleted!");
 
            if ( (Task.TaskStatus == BLL_Task.enTaskStatus.InProgress || Task.TaskStatus == BLL_Task.enTaskStatus.NotStarted) && Task.CancelTask())
            {
                return Ok("Task Cancelled Successfully.");
            }
            return BadRequest("Task not Cancelled Successfully!");
        }
    }
}
