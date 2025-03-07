using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace TaskSphere.Controllers
{


    public class EmployeeTitleDto
    {
        public int ID { get; set; }
        public string? Title { get; set; }
    }


    //[Route("api/[controller]")]
    [Route("api/TaskSphere")]
    [ApiController]
    public class TaskSpherController : ControllerBase
    {

        [HttpGet(Name = "GetData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<EmployeeTitleDto>> GetData()
        {
            List<EmployeeTitleDto> lst = [.. BLL_Task.GetEmployeeTitles().Select(t => new EmployeeTitleDto { ID = t.ID, Title = t.Title })];

            if (lst.Count == 0)
            {
                return NotFound("No Students Found!");
            }

            return Ok(lst);
        }

    }
}
