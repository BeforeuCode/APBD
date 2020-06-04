using cw3.DAL.DTO;
using cw3.DAL.Services;
using cw3.Models;
using System;

namespace cw3.DAL
{
    public class UserDbService : IUserDbService
    {
        private const string CONNECTION_STRING = "Data Source=db-mssql;Initial Catalog=s17545;Integrated Security=True";

        public void AddUser(LoginDto registerData)
        {
            throw new NotImplementedException();
        }

        public User GetUserByRefreshToken(string refToken)
        {
            throw new NotImplementedException();
        }

        public User GetUserCredentials(string username)
        {
            throw new NotImplementedException();
        }

        public object UpdateRefreshToken(object refreshToken)
        {
            throw new NotImplementedException();
        }

        public bool Validate(string value, string salt, string hash)
        {
            throw new NotImplementedException();
        }
    }
}
