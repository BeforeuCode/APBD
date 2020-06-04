using System;
using System.Collections.Generic;

namespace cw3.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Salt { get; set; }
        public string HashedPassword { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
    }
}
