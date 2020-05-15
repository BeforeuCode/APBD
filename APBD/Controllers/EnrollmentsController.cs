using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using cw3.DAL;
using cw3.DAL.DTO;
using cw3.DAL.DTOs.Requests;
using cw3.DAL.Services;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : Controller
    {
        private readonly IStudentsDbService _dbService;

        public EnrollmentsController(IStudentsDbService dbService)
        {
            _dbService = dbService;
        }
        [HttpPost]
        public IActionResult AddEnrollment(EnrollmentDTO enrollmentDTO)
        {
            try
            {
                enrollmentDTO.GetType().GetFields().Select(field => field.GetValue(enrollmentDTO)).ToList().Find(variable => variable == null);
                Enrollment enrollment = _dbService.EnrollStudent(enrollmentDTO);

                if (enrollment != null)
                {
                    return Created("api/students/" + enrollmentDTO.IndexNumber, enrollment);
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex);
            }

        }
        [HttpPost("promotions")]
        public IActionResult Promote(PromotionDTO promotionDTO)
        {
            Enrollment enrollment = _dbService.Promote(promotionDTO);
            if (enrollment != null)
            {
                return Created("",enrollment);
            }
            else
            {
                return BadRequest();
            }
        }

    }
  
}
