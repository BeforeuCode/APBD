using cw3.DAL.DTO;
using cw3.DAL.DTOs.Requests;
using cw3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.DAL.Services
{
    public interface IStudentsDbService
    {
        public IEnumerable<Student> GetStudents();
        public IEnumerable<Enrollment> GetStudentEnrollmentByIndexNumber(string id);
        public Enrollment EnrollStudent(EnrollmentDTO enrollment);
        public Enrollment Promote(PromotionDTO promotion);
        public Student  GetStudentByIndex(string index);
    }
}
