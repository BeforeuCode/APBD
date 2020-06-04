using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using cw3.DAL;
using cw3.DAL.Services;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public IActionResult GetStudents(string orderBy)
        {
            //return Ok(_dbService.GetStudents());
            return BadRequest();
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentEnrollmentById(string id)
        {
            //return Ok(_dbService.GetStudentEnrollmentByIndexNumber(id));
            return BadRequest();
        }

        //https://localhost:44386/api/students/query?orderBy=lastname
        [HttpGet("query")]
        public string GetStudentByQuery(String orderBy)
        {
            // return $"Kowalski, Malewski, Andrzejewski sortowanie = {orderBy}";
            return "";
        }

        //https://localhost:44386/api/students
        [HttpPost("add")]
        public IActionResult CreateStudent(Student student)
        {
            //.. add to db
            //.. generate index
            // student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            //return Ok(student);
            return BadRequest();
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateStudent(int id)
        {
            //return Ok("Aktualizacja dokończona");
            return BadRequest();
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteStudent(int id)
        {
            //return Ok("Usuwanie ukończkone");
            return BadRequest();
        }
    }
}