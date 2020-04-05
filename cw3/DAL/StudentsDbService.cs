using cw3.DAL;
using cw3.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace cw3.DAL
{
    public class StudentsDbService : IDbService
    {
        static StudentsDbService()
        {
        }
        public IEnumerable<Student> GetStudents()
        {
            string connectionString = "Data Source=db-mssql;Initial Catalog=s17545;Integrated Security=True";
            List<Student> students = new List<Student> { };
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT s.IndexNumber, s.FirstName, s.LastName, s.BirthDate, e.Semester, e.StartDate, st.Name " +
                                      "FROM Student s " +
                                      "INNER JOIN Enrollment e ON s.IdEnrollment = e.IdEnrollment " +
                                      "INNER JOIN Studies st ON e.IdStudy = st.IdStudy ";

                connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    Student student = Student.newStudent(
                        dataReader["IndexNumber"].ToString(),
                        dataReader["FirstName"].ToString(),
                        dataReader["LastName"].ToString(),
                        Convert.ToDateTime(dataReader["BirthDate"]),
                        Enrollment.newEnrollment(
                            Convert.ToInt32(dataReader["Semester"]),
                            Study.newStudy(dataReader["Name"].ToString()),
                            Convert.ToDateTime(dataReader["StartDate"]))
                        );

                    students.Add(student);
                }

            }
            return students;
        }

        public IEnumerable<Enrollment> GetStudentEnrollmentByIndexNumber(string indexNumber)
        {
            string connectionString = "Data Source=db-mssql;Initial Catalog=s17545;Integrated Security=True";
            List<Enrollment> enrollments = new List<Enrollment> { };
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT s.Name, e.Semester, e.StartDate FROM Enrollment e " +
                                      "INNER JOIN Studies s ON e.IdStudy = s.IdStudy " +
                                      "WHERE(SELECT s.IdEnrollment FROM Student s WHERE s.IndexNumber = @ID) = e.IdEnrollment ";
                command.Parameters.AddWithValue("@ID", indexNumber);
            
                connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {

                    enrollments.Add(
                        Enrollment.newEnrollment(
                            Convert.ToInt32(dataReader["Semester"]),
                            Study.newStudy(dataReader["Name"].ToString()),
                            Convert.ToDateTime(dataReader["StartDate"])
                            )
                    );
                }

            }
            return enrollments;
        }

    }
}
