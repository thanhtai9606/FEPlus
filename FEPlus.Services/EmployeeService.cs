using FEPlus.Contract;
using FEPlus.Models;
using FEPlus.Pattern.Factory;
using FEPlus.Pattern.Repositories;
using FEPlus.Pattern.UnitOfWork;
using FEPlus.Service.Pattern;
using log4net;
using NBear.Data;
using System;
using System.Data;
using System.Web.Security;

namespace FEPlus.Services
{
    public class EmployeeService : IEmployeeService
    {
        protected readonly ILog Loger = LogManager.GetLogger("HSSELogger");
        private NBear.Data.Gateway gateIM = new NBear.Data.Gateway("IM");
        private readonly IRepositoryAsync<Employee> _employeeService;

        public EmployeeService()
        {
            var context = new IMContext();
            var unit = new UnitOfWork(context);
            _employeeService = new Repository<Employee>(context, unit);
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (String.IsNullOrWhiteSpace(username)) throw new ArgumentException("Cannot type in Null or Empty", "Username");
            if (String.IsNullOrWhiteSpace(oldPassword)) throw new ArgumentException("Cannot type in Null or Empty", "Old Password");
            if (String.IsNullOrWhiteSpace(newPassword)) throw new ArgumentException("Cannot type in Null or Empty", "NewPassword");

            try
            {
                var _provider = Membership.Provider;
                MembershipUser membershipUser = _provider.GetUser(username, true);
                return membershipUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

        [Obsolete]
        public bool CheckTCode(string username, string tcode)
        {
            DataTable o = gateIM.ExecuteStoredProcedure("GetTcode2UserName",
               new string[] { "UserID", "RoleName" }, new object[] { username, tcode }).Tables[0];
            if (o.Rows.Count > 0)
                return true;
            else
                return false;
        }

        public UserAccount ValidateUser(string token)
        {
            var result = new UserAccount();
            if (!string.IsNullOrEmpty(token))
            {
                NBear.Data.Gateway cas = new NBear.Data.Gateway("CAS");
                DataTable dt = cas.DbHelper.Select("SELECT  *  from View_LoginList where Token=@Token", new object[] { token }).Tables[0];              
                if (dt.Rows.Count > 0)
                {
                    Console.WriteLine("ValidateUser:" + token);                   
                    result.Username = dt.Rows[0]["UserID"].ToString();
                    result.NickName = dt.Rows[0]["UserName"].ToString();
                    result.Token = token;
                }               
            }
            return result;
        }

        public UserAccount ValidateUser(string username, string password)
        {
            Console.WriteLine("ValidateUser: " + username);
            var validate = Membership.ValidateUser(username, password);
            var loginUser = new UserAccount();
            if (validate)
            {
                //employees for HRMS Data
                if (username.ToLower().Contains("fepv"))
                {
                    username = username.ToUpper();
                }
                var currentUser = _employeeService.Find(username);
                loginUser = new UserAccount(
                        currentUser.EmployeeID
                        , "" //email
                        , currentUser.Name
                        , currentUser.PositionName
                        , currentUser.DepartmentID
                        , currentUser.Specification
                        , "" //token
                        );               
                
                               
            }
            return loginUser;
           
        }
    }
   
}
