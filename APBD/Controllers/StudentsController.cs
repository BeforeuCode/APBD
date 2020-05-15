using System;
using System.Linq;
using System.Text.Json;
using cw3.DAL;
using cw3.DAL.Services;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentsDbService _dbService;

        public StudentsController(IStudentsDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            return Ok(_dbService.GetStudents());
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentEnrollmentById(string id)
        {
            return Ok(_dbService.GetStudentEnrollmentByIndexNumber(id));
        }

        //https://localhost:44386/api/students/query?orderBy=lastname
        [HttpGet("query")]
        public string GetStudentByQuery(String orderBy)
        {
            return $"Kowalski, Malewski, Andrzejewski sortowanie = {orderBy}";
        }

        //https://localhost:44386/api/students
        [HttpPost("add")]
        public IActionResult CreateStudent(Student student)
        {
            //.. add to db
            //.. generate index
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateStudent(int id)
        {
            return Ok("Aktualizacja dokończona");
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteStudent(int id)
        {
            return Ok("Usuwanie ukończkone");
        }
    }
}