using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Models
{
    public class User
    {
            
        public string Username { get; set; }
        public string Salt { get; set; }
        public string HashedPassword { get; set; }
        public string RefreshToken { get; set; }
        
        public string Role { get; set; }

        public static User newUser(string username, string salt, string hashedPassword, string refreshToken, string role)
        {
            User user = new User();

            user.Username = username;
            user.Salt = salt;
            user.HashedPassword = hashedPassword;
            user.RefreshToken = refreshToken;
            user.Role = role;

            return user;
        }
    }
}
