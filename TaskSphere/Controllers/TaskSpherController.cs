using BLL;
using DAL;
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

        // ===================================
        // =============== GET ===============
        // ===================================
        // For Get we use Http Get
        [HttpGet("{ID}", Name = "GetPersonByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PersonDTO> GetPersonByID(int ID)
        {
            if (ID < 1)
                return BadRequest($"Not Accepted ID {ID}");

            BLL_Person? Person = BLL_Person.Find(ID);

            if (Person is null)
                return NotFound($"Person with ID {ID} not found.");

            PersonDTO PDTO = Person.PDTO;
            return Ok(PDTO);
        }


        [HttpGet("AllPeople", Name = "GetAllPeople")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<PersonDTO>> GetAllPeople()
        {

            var People = BLL_Person.GetAll();

            if (People is null || People.Count == 0)
                return NotFound("No People Found!");

            return Ok(People); // return the list of People
        }


        // ====================================
        // =============== POST ===============
        // ====================================
        // For add new we use Http Post
        [HttpPost(Name = "AddPerson")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PersonDTO> AddPerson(PersonDTO NewPersonDTO)
        {
            if(NewPersonDTO is null || string.IsNullOrEmpty(NewPersonDTO.FullName) || string.IsNullOrEmpty(NewPersonDTO.Address)|| string.IsNullOrEmpty(NewPersonDTO.Email))
            {
                return BadRequest("Invalid Person Data.");
            }

            BLL_Person Person = new(NewPersonDTO);
            if (!Person.Save())
                return Unauthorized("The Person data not Saved.");
                
            NewPersonDTO.ID = Person.ID;

            return CreatedAtRoute("GetPersonByID", new {id = NewPersonDTO.ID}, NewPersonDTO);
        }


        // ===================================
        // =============== PUT ===============
        // ===================================
        // Here we use http put method for update
        [HttpPut("{id}", Name = "UpdatePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonDTO> UpdatePerson(int id, PersonDTO UpdatedPerson)
        {
            if (id < 1 || UpdatedPerson is null || string.IsNullOrEmpty(UpdatedPerson.FullName) || string.IsNullOrEmpty(UpdatedPerson.Address) || string.IsNullOrEmpty(UpdatedPerson.Email))
            {
                return BadRequest("Invalid Person Data.");
            }

            BLL_Person? person = BLL_Person.Find(id);

            if (person is null)
            {
                return NotFound($"Person with ID {id} not found.");
            }

            person.FullName = UpdatedPerson.FullName;
            person.BirthDate = UpdatedPerson.BirthDate;
            person.Address = UpdatedPerson.Address;
            person.Email = UpdatedPerson.Email;
            person.Phone = UpdatedPerson.Phone;

            if (person.Save())
                // We return the DTO not the full person object.
                return Ok(person.PDTO);
            else
                return StatusCode(500, new { message = "Error Updating Person" });
        }



        // ======================================
        // =============== DELETE ===============
        // ======================================
        // Here we use HttpDelete method
        [HttpDelete("{ID}", Name = "DeletePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeletePerson(int ID)
        {
            if (ID < 1)
                return BadRequest($"Not accepted ID {ID}");

            if (BLL_Person.DeletePerson(ID))
                return Ok($"Person With ID {ID} has been deleted successfully.");

            return NotFound($"Person With ID {ID} not found. no rows deleted!");
        }

    }
}
