using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageUserPasswords.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsBlocked { get; set; }
        public string RestrictedPassword { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            IsBlocked = false;
            RestrictedPassword = "password1234";
        }
    }
}
