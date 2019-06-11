using FEPlus.Contract.EMCS;
using FEPlus.Models;
using FEPlus.Pattern.DataContext;
using FEPlus.Pattern.Factory;
using FEPlus.Pattern.Repositories;
using FEPlus.Pattern.UnitOfWork;
using FEPlus.Service.Pattern;
using FEPlus.Utility;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEPlus.Services.EMCS
{
    public class EquipmentService : IEquipmentService
    {
        public IRepositoryAsync<Equipment> _equipmentService;
        public IRepositoryAsync<Method> _methodService;
        public IRepositoryAsync<Manual> _manualService;
        public IUnitOfWorkAsync _unitOfWorkAsync;
        public OperationResult operationResult = new OperationResult();
        public NBear.Data.Gateway db = new NBear.Data.Gateway("HSE");
        public NBear.Data.Gateway emcs = new NBear.Data.Gateway("EMCS");
        protected readonly ILog Loger = LogManager.GetLogger("HSSELogger");

        public EquipmentService( IRepositoryAsync<Equipment> equipmentService,
                                 IRepositoryAsync<Method> methodService,
                                 IRepositoryAsync<Manual> manualService,
                                 IUnitOfWorkAsync unitOfWorkAsync)
        {
            _equipmentService = equipmentService;
            _manualService = manualService;
            _methodService = methodService;
            _unitOfWorkAsync = unitOfWorkAsync;
        }
        public OperationResult AddEquipment(Equipment equipment)
        {
            try
            {
                equipment.EQID = emcs.SelectScalar<string>("select dbo.GenNewID('Equipment',@department)", new object[] { equipment.Department });
                equipment.ObjectState = Pattern.Infrastructure.ObjectState.Added;
                _equipmentService.Add(equipment);
                foreach (var manual in equipment.Manuals)
                {
                    manual.EQID = equipment.EQID;
                    manual.ObjectState = Pattern.Infrastructure.ObjectState.Added;
                    manual.Version = 1;
                    manual.State = "1";
                    _manualService.Add(manual);
                }
                foreach (var method in equipment.Methods)
                {
                    method.EQID = equipment.EQID;
                    method.Version = 1;
                    method.State = "1";
                    method.ObjectState = Pattern.Infrastructure.ObjectState.Added;
                    _methodService.Add(method);
                }
                _unitOfWorkAsync.SaveChanges();
                operationResult.Success = true;
                operationResult.Message = "Added Successed!";
                operationResult.Caption = "Successed!";

            }
            catch (Exception ex)
            {
                operationResult.Success = false;
                operationResult.Message = "There are some things wrong: " + ex.ToString();
                operationResult.Caption = "Error!";
                Loger.Error(ex);

            }
            var loginfo = String.Format("EMCS - AddEquipment - {0}",  equipment.EQID);
            Console.WriteLine(loginfo);
            Loger.Info(loginfo);
            return operationResult;
        }

        [Obsolete]
        public OperationResult DeleteEquipment(Equipment equipment)
        {
            try
            {
                equipment.ObjectState = Pattern.Infrastructure.ObjectState.Modified;
                equipment.State = "X";// Delete Voucher
                _equipmentService.Update(equipment);

                DeleteDetail(equipment.EQID);


                _unitOfWorkAsync.SaveChanges();
                operationResult.Success = true;
                operationResult.Message = "Delete Successed!";
                operationResult.Caption = "Successed!";

            }
            catch (Exception ex)
            {
                operationResult.Success = false;
                operationResult.Message = "There are some things wrong: " + ex.ToString();
                operationResult.Caption = "Error!";
                Loger.Error(ex);


            }
            var loginfo = String.Format("EMCS - AddEquipment - {0}",  equipment.EQID);
            Console.WriteLine(loginfo);
            Loger.Info(loginfo);
            return operationResult;
        }

        [Obsolete]
        public OperationResult DeleteDetail(string EQID)
        {
            try
            {
                //var x1 = _methodService.FindBy(x => x.EQID == EQID);
                var Method = _methodService.FindBy(x => x.EQID == EQID).ToList();
                foreach(Method item in Method)
                {
                    item.ObjectState = Pattern.Infrastructure.ObjectState.Modified;
                    item.State = "X";
                    _methodService.Update(item);
                }


                var Manual = _manualService.FindBy(x => x.EQID == EQID).ToList();
                foreach (Manual item in Manual)
                {
                    item.ObjectState = Pattern.Infrastructure.ObjectState.Modified;
                    item.State = "X";
                    _manualService.Update(item);
                }

                _unitOfWorkAsync.SaveChanges();
                operationResult.Success = true;
                operationResult.Message = "Delete Successed!";
                operationResult.Caption = "Successed!";

            }
            catch (Exception ex)
            {
                operationResult.Success = false;
                operationResult.Message = "There are some things wrong: " + ex.ToString();
                operationResult.Caption = "Error!";
                Loger.Error(ex);

            }
            return operationResult;
        }

        [Obsolete]
        public OperationResult UpdateEquipment(Equipment equipment)
        {
            try
            {
                equipment.ObjectState = Pattern.Infrastructure.ObjectState.Modified;
                equipment.State = "M";//Modified Equipment
                _equipmentService.Update(equipment);

                DeleteDetail(equipment.EQID);

                foreach (var manual in equipment.Manuals)
                {
                    manual.MethodID = 0;
                    manual.EQID = equipment.EQID;
                    manual.State = "1";
                    manual.ObjectState = Pattern.Infrastructure.ObjectState.Added;
                    _manualService.Add(manual);
                }
                foreach (var method in equipment.Methods)
                {
                    method.MethodID = 0;
                    method.EQID = equipment.EQID;
                    method.State = "1";
                    method.ObjectState = Pattern.Infrastructure.ObjectState.Added;
                    _methodService.Add(method);
                }
                _unitOfWorkAsync.SaveChanges();
                operationResult.Success = true;
                operationResult.Message = "Update Successed!";
                operationResult.Caption = "Successed!";

            }
            catch (Exception ex)
            {
                operationResult.Success = false;
                operationResult.Message = "There are some things wrong: " + ex.ToString();
                operationResult.Caption = "Error!";
                Loger.Error(ex);

            }
            var loginfo = String.Format("EMCS - UpdateEquipment - {0}",  equipment.EQID);
            Console.WriteLine(loginfo);
            Loger.Info(loginfo);
            return operationResult;
        }

        //[Obsolete]
        //public ICollection<Equipment> GetData() => _equipmentService.Queryable().ToList();

        [Obsolete]
        public DataTable GetEquipment(string AssetID, string EQName, string Department, string ProcessDepartment, string UserID, string Lang)
        {
            Console.WriteLine("EMCS - SearchEquipment");
            var data = emcs.ExecuteStoredProcedure("GetEquipment", new string[] { "AssetID", "EQName", "Department", "ProcessDepartment", "UserID" , "Language"},
              new object[] { AssetID, EQName, Department, ProcessDepartment, UserID, Lang }).Tables[0];
            return data;

        }

        [Obsolete]
        public DataTable GetBasic(string table, string lang)
        {
            var data = emcs.ExecuteStoredProcedure("GetBasic", new string[] { "Table", "Language" }, new object[] { table, lang }).Tables[0];
            return data;
        }

        [Obsolete]
        public DataTable GetDepartment(string Table, string Lang)
        {
            return db.ExecuteStoredProcedure("GetBasic", new string[] { "Table", "Lang" },
              new object[] { Table, Lang }).Tables[0];
        }

        [Obsolete]
        public Dictionary<string, object> FindDetail(string EQID)
        {
            var data = emcs.ExecuteStoredProcedure("FindDetailEquipment", new string[] { "EQID" }, new object[] { EQID });
            Dictionary<string, Object> result = new Dictionary<string, object>();
            result.Add("Header", data.Tables[0]);
            result.Add("Manuals", data.Tables[1]);
            result.Add("Methods", data.Tables[2]);
            return result;
        }


        [Obsolete]
        public int CheckUnique(string Table, string ColumnName, string Value)
        {
            var data = emcs.ExecuteStoredProcedure("CheckColumnUnique", new string[] { "TableName", "ColumnName", "CheckValue" },
              new object[] { Table, ColumnName, Value }).Tables[0];
            var tableRow = data.AsEnumerable().First();
            int result = tableRow.Field<int>(data.Columns.IndexOf("CountValue")); //CountValue
            return result;
        }

    }
}
