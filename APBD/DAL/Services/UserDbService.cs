using cw3.DAL.DTO;
using cw3.DAL.Services;
using cw3.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace cw3.DAL
{
    public class UserDbService : IUserDbService
    {
        private const string CONNECTION_STRING = "Data Source=db-mssql;Initial Catalog=s17545;Integrated Security=True";
        public void AddUser(LoginDto registerData)
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand command = new SqlCommand())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                try
                {

                    var salt = CreateSalt();
                    var password = CreatePassword(registerData.Password, salt);

                    command.CommandText = "INSERT INTO User(Username, Password, Salt) " +
                                "VALUES(@Username, @Password, @Salt)";
                    command.Parameters.AddWithValue("Username", registerData.Username);
                    command.Parameters.AddWithValue("Password", password);
                    command.Parameters.AddWithValue("Salt", salt);
                    command.Connection = connection;

                   

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("ERROR" + ex);
                
                }
            }

        }
        public User GetUserCredentials(string username)
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand command = new SqlCommand())
            {

                command.CommandText = "Select * form User u Where u.username = @Username";
                command.Parameters.AddWithValue("Username", username);

                connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    User student = User.newUser(
                        dataReader["Username"].ToString(),
                        dataReader["Salt"].ToString(),
                        dataReader["HashedPassword"].ToString(),
                        dataReader["RefreshToken"].ToString(),
                        dataReader["Role"].ToString()
                        );

                    return student;
                };
                return null;
            }
        }

        public bool Validate(string value, string salt, string hash) => CreatePassword(value, salt) == hash;
      

        public static string CreatePassword(string password, string salt)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(valueBytes);
        }

        public static string CreateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var genertor = RandomNumberGenerator.Create())
            {
                genertor.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        public User GetUserByRefreshToken(string refToken)
        {
            throw new NotImplementedException();
        }

        public object UpdateRefreshToken(object refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
