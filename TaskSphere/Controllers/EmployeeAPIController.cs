using BLL;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskSphere.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/TaskSphere/Employee")]
    [ApiController]
    public class EmployeeAPIController : ControllerBase
    {

        // ===================================
        // =============== GET ===============
        // ===================================
        // For Get we use Http Get
        [HttpGet("{ID}", Name = "GetEmployeeByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<EmployeeDTO> GetEmployeeByID(int ID)
        {
            if (ID < 1)
                return BadRequest($"Not Accepted ID {ID}");

            BLL_Employee? Employee = BLL_Employee.Find(ID);

            if (Employee is null)
                return NotFound($"Employee with ID {ID} not found.");

            EmployeeDTO EDTO = Employee.EDTO;
            return Ok(EDTO);
        }


        [HttpGet("AllEmployees", Name = "GetAllEmployees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<EmployeeDTO>> GetAllEmployees()
        {
            var Employees = BLL_Employee.GetAll();
            if (Employees is null || Employees.Count == 0)
                return NotFound("No Employees Found!");

            return Ok(Employees); // return the list of Employees
        }


        // ====================================
        // =============== POST ===============
        // ====================================
        // For add new we use Http Post
        [HttpPost(Name = "AddEmployee")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<EmployeeDTO> AddEmployee([FromBody] EmployeeDTO EDTO)
        {
            if (EDTO is null)
                return BadRequest("Employee Data is missing!");
            
            BLL_Employee Employee = new(EDTO);
            
            if (Employee.Save())
                return CreatedAtRoute("GetEmployeeByID", new { ID = Employee.EmployeeID }, Employee.EDTO);
  
            else
                return BadRequest("Employee not added!");
        }

    
        // ====================================
        // =============== PUT ===============
        // ====================================
        // For update we use Http Put
        [HttpPut("{ID}", Name = "UpdateEmployee")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<EmployeeDTO> UpdateEmployee(int ID, [FromBody] EmployeeDTO UpdatedEmployee)
        {
            if (ID < 1 || UpdatedEmployee is null)
            {
                return BadRequest("Invalid Employee Data.");
            }

            BLL_Employee? Employee = BLL_Employee.Find(ID);

            if (Employee is null)
            {
                return NotFound($"Employee with ID {ID} not found.");
            }

            Employee.Salary = UpdatedEmployee.salary;
            Employee.TitleID = UpdatedEmployee.TitleID;
            Employee.DepartmentID = UpdatedEmployee.departmentID;
            Employee.StartDate = UpdatedEmployee.StartDate;
            Employee.EndDate = UpdatedEmployee.EndDate;

            if (Employee.Save())
                // We return the DTO not the full person object.
                return Ok(Employee.EDTO);
            else
                return StatusCode(500, new { message = "Error Updating Employee" });
        }



        // =======================================
        // =============== DELETE ===============
        // =======================================
        // For delete we use Http Delete
        [HttpDelete("{ID}", Name = "DeleteEmployee")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteEmployee(int ID)
        {
            if (ID < 1)
                return BadRequest($"Not Accepted ID {ID}");
            if (BLL_Employee.Delete(ID))
                return Ok("Employee Deleted!");
            else
                return BadRequest("Employee not deleted!");
        }



    }
}
