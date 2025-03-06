using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace TaskSphere.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/TaskSphere")]
    [ApiController]
    public class TaskSpherController : ControllerBase
    {

        [HttpGet(Name = "GetData")]
        public IActionResult GetData()
        {
            return Ok( Business.msg );
        }

    }
}
