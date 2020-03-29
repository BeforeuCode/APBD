using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Models
{
    public class Student
    {
        public int IdStudent { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber { get; set; }

        public Student(int idStudent, string firstName, string lastName)
        {
            IdStudent = idStudent;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
