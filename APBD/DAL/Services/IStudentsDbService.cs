using cw3.DAL.DTO;
using cw3.DAL.DTOs.Requests;
using cw3.Models;
using System.Collections.Generic;

namespace cw3.DAL.Services
{
    public interface IStudentsDbService
    {
        public IEnumerable<Student> GetStudents();
        public Enrollment EnrollStudent(EnrollmentDTO enrollment);
        public Enrollment Promote(PromotionDTO promotion);
        public Student UpdateStudent(StudentDTO student);
        public Student RemoveStudent(string indexNumber);

    }
}
