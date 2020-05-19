using cw3.DAL;
using cw3.DAL.DTO;
using cw3.DAL.DTOs.Requests;
using cw3.DAL.Services;
using cw3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace cw3.DAL
{
    public class StudentsDbService : IStudentsDbService
    {
        private const string CONNECTION_STRING = "Data Source=db-mssql;Initial Catalog=s17545;Integrated Security=True";
        static StudentsDbService()
        {
        }
        public IEnumerable<Student> GetStudents()
        {
            List<Student> students = new List<Student> { };
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
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
            List<Enrollment> enrollments = new List<Enrollment> { };
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
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


        public Enrollment EnrollStudent(EnrollmentDTO enrollment)
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand command = new SqlCommand())
            {
                DateTime parsedDate = Convert.ToDateTime(enrollment.BirthDate);
                command.Connection = connection;
                connection.Open();


                SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                //GET IDSTUDY if NOT present throw 404
                command.CommandText = "SELECT IdStudy FROM Studies WHERE Name = @Name";
                command.Parameters.AddWithValue("Name", enrollment.Studies);
                SqlDataReader dataReader = command.ExecuteReader();
                //1.WORKFLOW 1 - IdStudy exists
                if (dataReader.Read())
                {

                    int idStudies = (int)dataReader["IdStudy"];
                    dataReader.Close();

                    //Czy istenieje IdEnrollment z idStudy i Semester 1 jesli nie to stworz enrollment
                    command.CommandText = "SELECT IdEnrollment FROM Enrollment WHERE IdStudy = @IdStudy AND Semester = 1";
                    command.Parameters.AddWithValue("IdStudy", idStudies);

                    dataReader = command.ExecuteReader();

                    //1.WORKFLOW 1.1 - IdEnrollment with IdStudy and Semester = 1 exists
                    if (dataReader.Read())
                    {
                        var idEnrollment = dataReader["IdEnrollment"];
                        dataReader.Close();
                        try
                        {
                            command.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) " +
                                "VALUES(@IndexNumber, @FirstName, @LastName, @BirthDate, @IdEnrollment )";
                            command.Parameters.AddWithValue("IndexNumber", enrollment.IndexNumber);
                            command.Parameters.AddWithValue("FirstName", enrollment.FirstName);
                            command.Parameters.AddWithValue("LastName", enrollment.LastName);
                            command.Parameters.AddWithValue("BirthDate", parsedDate);
                            command.Parameters.AddWithValue("IdEnrollment", (int)idEnrollment);
                            command.ExecuteNonQuery();

                            transaction.Commit();

                            command.CommandText = "SELECT e.Semester, s.Name, e.StartDate " +
                                "FROM Enrollment e " +
                                "INNER JOIN Studies s ON e.IdStudy = s.IdStudy " +
                                "WHERE IdEnrollment = @IdEnrollemnt";
                            command.Parameters.AddWithValue("IdEnrollemnt", idEnrollment);
                            dataReader = command.ExecuteReader();

                            if (dataReader.Read())
                            {
                                int semester = Convert.ToInt32(dataReader["Semester"]);
                                Study study = Study.newStudy(dataReader["Name"].ToString());
                                DateTime dateTime = Convert.ToDateTime(dataReader["StartDate"]);
                                dataReader.Close();
                                return (Enrollment.newEnrollment(semester, study, dateTime));
                            }

                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine("Duplicated Student ID" + ex);
                            return null;
                        }
                    }
                    //1.WORKFLOW 1.2 - IdEnrollment with IdStudy and Semester = 1 does NOT exist
                    else
                    {
                        //Workaround for unique id of enrollment 
                        command.CommandText = "SELECT MAX(IdEnrollment) as idEnrollment FROM Enrollment ";

                        dataReader.Close();
                        dataReader = command.ExecuteReader();
                        if (dataReader.Read())
                        {
                            int idNewEnrollment = (int)dataReader["idEnrollment"] + 1;
                            dataReader.Close();

                            try
                            {
                                command.CommandText = "INSERT INTO Enrollment(IdEnrollment, Semester, IdStudy, StartDate) " +
                                "VALUES(@IdEnrollment, 1, @IdStudy, GETDATE())";
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("IdStudy", idStudies);
                                command.Parameters.AddWithValue("IdEnrollment", idNewEnrollment);


                                command.ExecuteNonQuery();

                                transaction.Commit(); // Czy to nie jest zle jesli mam 2 x commit zamiast jednego przy calej operacji 
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error when creating Enrollment:" + ex);
                                transaction.Rollback();
                                return null;
                            }


                            try
                            {
                                SqlTransaction transactionStudent = connection.BeginTransaction();
                                command.Transaction = transactionStudent;
                                command.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) " +
                                   "VALUES(@IndexNumber, @FirstName, @LastName, @BirthDate, @IdEnrollment )";
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("IndexNumber", enrollment.IndexNumber);
                                command.Parameters.AddWithValue("FirstName", enrollment.FirstName);
                                command.Parameters.AddWithValue("LastName", enrollment.LastName);
                                command.Parameters.AddWithValue("BirthDate", parsedDate);
                                command.Parameters.AddWithValue("IdEnrollment", idNewEnrollment);


                                command.ExecuteNonQuery();

                                transactionStudent.Commit();

                                command.CommandText = "SELECT e.Semester, s.Name, e.StartDate " +
                                                      "FROM Enrollment e " +
                                                      "INNER JOIN Studies s ON e.IdStudy = s.IdStudy " +
                                                      "WHERE IdEnrollment = @IdEnrollemnt";

                                command.Parameters.AddWithValue("IdEnrollemnt", idNewEnrollment);
                                dataReader = command.ExecuteReader();

                                if (dataReader.Read())
                                {
                                    int semester = Convert.ToInt32(dataReader["Semester"]);
                                    Study study = Study.newStudy(dataReader["Name"].ToString());
                                    DateTime dateTime = Convert.ToDateTime(dataReader["StartDate"]);
                                    dataReader.Close();
                                    return (Enrollment.newEnrollment(semester, study, dateTime));

                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error when creating Student:" + ex);
                                transaction.Rollback();
                                return null;
                            }

                        }
                    }
                    return null;
                }
                //2.WORKFLOW 2 - IdStudy does not exist
                else
                {
                    Console.WriteLine("Nie znaleziono Studies o takim name" + enrollment.Studies);
                    return null;

                }
            }
        }

        public Enrollment Promote(PromotionDTO promotion)
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand command = new SqlCommand())
            {
           
                try { 
                    command.Connection = connection;
                    connection.Open();
                    command.CommandText = "PromoteStudent";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("Semester", promotion.Semester);
                    command.Parameters.AddWithValue("Study", promotion.Studies);

                    SqlDataReader dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        int semester = dataReader.GetInt32(0);
                        Study study = Study.newStudy(dataReader.GetString(1));
                        DateTime dateTime = Convert.ToDateTime(dataReader.GetDateTime(2));

                        return (Enrollment.newEnrollment(semester, study, dateTime));
                    }
                    return null;
                } 
                catch(Exception ex){
                    Console.WriteLine(ex);
                    return null;
                }
            }
        }
    }
}
