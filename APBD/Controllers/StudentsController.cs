using cw3.DAL.DTOs.Requests;
using cw3.DAL.Services;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentsDbService _dbService;
        public IConfiguration Configuration { get; set; }

        public StudentsController(IStudentsDbService dbService, IConfiguration configuration)
        {
            _dbService = dbService;
            Configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok(_dbService.GetStudents());
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateStudent(StudentDTO studentDTO)
        {

            if (!ModelState.IsValid)
            {
                var state = ModelState;
                return BadRequest();
            }
            Student student = _dbService.UpdateStudent(studentDTO);

            if (student != null)
            {
                return Created("/", student);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteStudent(string indexNumber)
        {
            if (!ModelState.IsValid)
            {
                var state = ModelState;
                return BadRequest();
            }
            Student student = _dbService.RemoveStudent(indexNumber);

            if (student != null)
            {
                return Ok(student);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}