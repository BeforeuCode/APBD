using cw3.Models;
using System.Collections.Generic;


namespace cw3.DAL
{
    public class MockDbService : IDbService
    {
        private static IEnumerable<Student> _students;

        static MockDbService()
        {
            _students = new List<Student>
            {
            };
        }

        public IEnumerable<Enrollment> GetStudentEnrollmentByIndexNumber(string id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }

}
