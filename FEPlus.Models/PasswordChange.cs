using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEPlus.Models
{
    public class PasswordChange
    {
        public string username { set; get; }
        public string oldPassword { set; get; }

        public string newPassword { set; get; }
    }
}
