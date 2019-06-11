using FEPlus.Contract.EMCS;
using FEPlus.Pattern.UnitOfWork;
using FEPlus.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEPlus.Models;
using FEPlus.Pattern.DataContext;
using FEPlus.Pattern.Factory;
using FEPlus.Pattern.Repositories;
using FEPlus.Service.Pattern;
using System.Web.Http;
using System.Collections;

namespace FEPlus.Services.EMCS
{
    public class GateCheckerService : IGateCheckerService
    {
        public OperationResult operationResult = new OperationResult();
        public HelperBiz helperBiz = new HelperBiz();
        public NBear.Data.Gateway emcs = new NBear.Data.Gateway("Beling");
        public IUnitOfWorkAsync _unitOfWorkAsync;
        public GateCheckerService(IUnitOfWorkAsync unitOfWorkAsync) { _unitOfWorkAsync = unitOfWorkAsync; }

        [Obsolete]
        public DataTable GetCheckerByKind(string kind)
        {
            DataTable dt = emcs.ExecuteStoredProcedure("OS_GetCheckerByKind", new string[] { "Kind" }, new object[] { kind }).Tables[0];

            DataTable tb = new DataTable();
            tb.Columns.Add("Person");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i]["Person"].ToString()))
                {
                    string pStrings = dt.Rows[i]["Person"].ToString().Replace('|', ',');
                    DataRow row = tb.NewRow();
                    row["Person"] = pStrings;
                    tb.Rows.Add(row);
                }
            }
            return tb;
        }

        [Obsolete]
        public Dictionary<string, object> GetDefCheckersByOwner(string owner, string fLowKey, string kinds, DateTime? checkDate)
        {
            Console.WriteLine("GetDefCheckersByOwner：" + DateTime.Now.ToString());
            DataTable dt = emcs.ExecuteStoredProcedure("OS_GetCheckers", new string[] { "EmployeeID", "FLowKey", "Kinds", "CheckDate" }, new object[] { owner, fLowKey, kinds, checkDate }).Tables[0];
            Dictionary<string, Object> result = new Dictionary<string, object>();

            //Table1:  Person --Used to send to Next Checker
            DataTable tb = new DataTable();
            tb.Columns.Add("Person");


            //Table2: Username --Used to show UserName of Next Checker
            List<string> lsUser = new List<string>();
            DataTable tbUser = new DataTable();
            tbUser.Columns.Add("Username");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i]["Person"].ToString()))
                {
                    string pStrings = dt.Rows[i]["Person"].ToString().Replace('|', ',');
                    DataRow row = tb.NewRow();
                    row["Person"] = pStrings;
                    tb.Rows.Add(row);

                    foreach (string item in pStrings.Split(','))
                    {
                        string Username = "";
                        Username = GetUserName(item);
                        if (Username != null)
                        {
                            DataRow usRow = tbUser.NewRow();
                            usRow["Username"] = Username;
                            tbUser.Rows.Add(usRow);
                        }
                    }
                }
            }
            result.Add("Person", tb);
            result.Add("UserName", tbUser);

            return result;
        }

        [Obsolete]
        public string GetUserName(string UserID)
        {
            try
            {
                string getUserNameSql = @"select EmployeeID +'-'+ Name As Username from vHREmployees where EmployeeID ='" + UserID + "'";
                return emcs.SelectScalar<string>(getUserNameSql, new object[] { });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }



        [Obsolete]
        public Dictionary<string, string> GetCheckLevel(string voucherId)
        {
            Console.WriteLine("GetCheckLevel：" + DateTime.Now.ToString());
            string msg = emcs.ExecuteStoredProcedure("OS_GetCheckLevel", new string[] { "VoucherID" }, new object[] { voucherId }).Tables[0].Rows[0][0].ToString();

            Console.WriteLine("Level：" + msg);
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("msg", msg);
            return values;
        }

        [Obsolete]
        public Dictionary<string, string> IsWorkDay(DateTime day)
        {
            string msg = emcs.ExecuteStoredProcedure("OS_IsWorkDay", new string[] { "ExpectIn" }, new object[] { day }).Tables[0].Rows[0][0].ToString();

            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("msg", msg);
            return values;
        }

        [Obsolete]
        public DataTable GateQueryStatus(string ctype, string language, string flag)
        {
            DataTable table = emcs.ExecuteStoredProcedure("G_GetGateQueryStatus",
                new string[] { "Ctype", "Language", "Flag" }, new object[] { ctype, language, flag }).Tables[0];
            return table;
        }
    }
}
