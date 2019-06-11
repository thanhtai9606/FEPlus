using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEPlus.Models
{
    public class UserAccount
    {
        public string Username { set; get; }
        public string Email { set; get; }
        public string NickName { set; get; }
        public string Position { set; get; }
        public string Department { set; get; }
        public string Specification { set; get; }
        public string Token { set; get; }


        public UserAccount() { }
        public UserAccount(string username, string email, string nickname, string position, string department, string specification, string token)
        {
            Username = username;
            Email = email;
            NickName = nickname;
            Position = position;
            Department = department;
            Specification = specification;
            Token = token;
        }
    }
}
