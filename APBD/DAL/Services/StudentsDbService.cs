using cw3.DAL.DTO;
using cw3.DAL.DTOs.Requests;
using cw3.DAL.Services;
using cw3.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace cw3.DAL
{
    public class StudentsDbService : IStudentsDbService
    {
        s17545Context db = new s17545Context();
        static StudentsDbService()
        { }

        public Enrollment EnrollStudent(EnrollmentDTO enrollment)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Student> GetStudents()
        {
            var res = db.Student.ToList();
            return res;
        }

        public Enrollment Promote(PromotionDTO promotion)
        {
            throw new NotImplementedException();
        }

        public Student RemoveStudent(string indexNumber)
        {
            try
            {
                var studentToRemove = db.Student.Where(s => s.IndexNumber.Equals(indexNumber)).FirstOrDefault();
                db.Student.Remove(studentToRemove);
                return studentToRemove;
            }
            catch (ArgumentNullException e)
            {
                return null;
            }
        }

        public Student UpdateStudent(StudentDTO student)
        {

            try
            {
                var resStudent = db.Student.Where(s => s.IndexNumber.Equals(student.IndexNumber)).FirstOrDefault();
                resStudent.FirstName = student.FirstName;
                resStudent.LastName = student.LastName;
                resStudent.BirthDate = Convert.ToDateTime(student.BirthDate);
                db.SaveChanges();

                var newStudent = new Student
                {
                    BirthDate = Convert.ToDateTime(student.BirthDate),
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    IdEnrollment = resStudent.IdEnrollment,
                    IndexNumber = student.IndexNumber
                };

                return newStudent;
            }
            catch (ArgumentNullException e)
            {
                return null;
            }
        }
    }
}
