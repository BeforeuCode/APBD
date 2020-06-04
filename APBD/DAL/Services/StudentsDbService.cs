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
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace cw3.DAL
{
    public class StudentsDbService : IStudentsDbService
    {
        private const string CONNECTION_STRING = "Data Source=db-mssql;Initial Catalog=s17545;Integrated Security=True";
        static StudentsDbService()
        { }

        public Enrollment EnrollStudent(EnrollmentDTO enrollment)
        {
            throw new NotImplementedException();
        }

        public Student GetStudentByIndex(string index)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Enrollment> GetStudentEnrollmentByIndexNumber(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Student> GetStudents()
        {
            throw new NotImplementedException();
        }

        public Enrollment Promote(PromotionDTO promotion)
        {
            throw new NotImplementedException();
        }
    }
}
