﻿using cw3.DAL.DTO;
using cw3.Models;

namespace cw3.DAL.Services
{
    public interface IUserDbService
    {
        public void AddUser(LoginDto registerData);
        public bool Validate(string value, string salt, string hash);

        public User GetUserCredentials(string username);
        public User GetUserByRefreshToken(string refToken);
        object UpdateRefreshToken(object refreshToken);
    }
}
