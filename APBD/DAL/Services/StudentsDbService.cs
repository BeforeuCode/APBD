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

            DateTime parsedDate = Convert.ToDateTime(enrollment.BirthDate);

            try
            {
                var studies = db.Studies.Where(study => study.Name.Equals(enrollment.Studies)).First();
                int idStudies = studies.IdStudy;

                var resEnrollment = db.Enrollment.Where(enrollment => enrollment.IdStudy.Equals(idStudies)).FirstOrDefault();

                if (resEnrollment != null)
                {
                    int idEnrollment = resEnrollment.IdEnrollment;
                    var studentToEnroll = new Student
                    {
                        IndexNumber = enrollment.IndexNumber,
                        FirstName = enrollment.FirstName,
                        LastName = enrollment.LastName,
                        BirthDate = parsedDate,
                        IdEnrollment = idEnrollment
                    };
                    db.Student.Add(studentToEnroll);
                    db.SaveChanges();

                    return resEnrollment;

                }
                else
                {
                    var newEnrollment = new Enrollment
                    {
                        Semester = 1,
                        StartDate = new DateTime(),
                        IdStudy = idStudies,
                    };
                    db.Enrollment.Add(newEnrollment);

                    var studentToEnroll = new Student
                    {
                        IndexNumber = enrollment.IndexNumber,
                        FirstName = enrollment.FirstName,
                        LastName = enrollment.LastName,
                        BirthDate = parsedDate,
                        IdEnrollment = newEnrollment.IdEnrollment
                    };

                    db.Student.Add(studentToEnroll);
                    db.SaveChanges();

                    return newEnrollment;
                }
            }
            catch (ArgumentNullException e)
            {
                return null;
            }
        }

        public IEnumerable<Student> GetStudents()
        {
            var res = db.Student.ToList();
            return res;
        }

        public Enrollment Promote(PromotionDTO promotion)
        {
            try
            {
                var resEnrollment = db.Enrollment.Join(db.Studies, e => e.IdStudy, s => s.IdStudy, (e, s) => new { e, s })
                .Where(entry => entry.s.Name.Equals(promotion.Studies))
                .Where(entry => entry.e.Semester.Equals(promotion.Semester)).First();

                var enrollmentForUpdate = db.Enrollment.Join(db.Studies, e => e.IdStudy, s => s.IdStudy, (e, s) => new { e, s })
                .Where(entry => entry.s.Name.Equals(promotion.Studies))
                .Where(entry => entry.e.Semester.Equals(promotion.Semester + 1)).FirstOrDefault();

                var studentsToPromote = db.Student.Where(s => s.IdEnrollment.Equals(resEnrollment.e.IdEnrollment)).ToList();

                if (enrollmentForUpdate != null)
                {
                   
                    studentsToPromote.ForEach(s => s.IdEnrollment = enrollmentForUpdate.e.IdEnrollment);
                    db.SaveChanges();

                    return enrollmentForUpdate.e;

                }
                else
                {

                    var studies = db.Studies.Where(s => s.Name.Equals(promotion.Studies)).First();
                    var newEnrollment = new Enrollment
                    {
                        IdStudy = studies.IdStudy,
                        Semester = promotion.Semester + 1,
                        StartDate = new DateTime(),
                    };

                    studentsToPromote.ForEach(s => s.IdEnrollment = newEnrollment.IdEnrollment);
                    db.SaveChanges();
                   
                    return newEnrollment;
                }
            }
            catch (ArgumentNullException e)
            {
                return null;
            }
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
