using FEPlus.Models;
using FEPlus.Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEPlus.Contract
{
    public interface IEmployeeService
    {
        UserAccount ValidateUser(string token);
        UserAccount ValidateUser(string username, string password);
        bool ChangePassword(string username, string oldPassword, string newPassword);
        bool CheckTCode(string username, string tcode);
    }
}
