using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Models
{
    public class Student
    {
        //I had to use public bacuause of JSON serialization 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public Enrollment Enrollment { get; set; }

        public Student(){
        }

        public static Student newStudent(string indexNumber, string firstName, string lastName, DateTime birthDate, Enrollment enrollment)
        {
            Student student = new Student();

            student.IndexNumber = indexNumber;
            student.FirstName = firstName;
            student.LastName = lastName;
            student.BirthDate = birthDate;
            student.Enrollment = enrollment;

            return student;
        }
      
    }
}
