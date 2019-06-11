using FEPlus.Contract.EMCS;
using FEPlus.Models;
using FEPlus.Pattern.DataContext;
using FEPlus.Pattern.Repositories;
using FEPlus.Pattern.UnitOfWork;
using FEPlus.Service.Pattern;
using FEPlus.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace FEPlus.EMCSApi.Controller
{
    [Filter.FilterIP]
    [RoutePrefix("api/EMCS/Home")]
    public class HomeController:ApiController
    {

        private readonly IRepositoryAsync<Equipment> _equipmentService;
        private readonly IRepositoryAsync<Method> _methodService;
        private readonly IRepositoryAsync<Manual> _manualService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly OperationResult operationResult = new OperationResult();
        public NBear.Data.Gateway gateIM = new NBear.Data.Gateway("EMCS");
    
        public HomeController(   IRepositoryAsync<Equipment> equipmentService,
                                 IRepositoryAsync<Method> methodService,
                                 IRepositoryAsync<Manual> manualService,
                                 IUnitOfWorkAsync unitOfWorkAsync)
        {
            _equipmentService = equipmentService;
            _manualService = manualService;
            _methodService = methodService;
            _unitOfWorkAsync = unitOfWorkAsync;
        }
        [Route("AddEquipment")]
        [HttpPost]
        [Obsolete]
        public OperationResult AddEquipment(Equipment equipment)
        {
            try
            {
                equipment.EQID = Guid.NewGuid().ToString().ToUpper();
                equipment.ObjectState = Pattern.Infrastructure.ObjectState.Added;
                _equipmentService.Add(equipment);
                foreach (var manual in equipment.Manuals)
                {
                    manual.EQID = equipment.EQID;
                    manual.ObjectState = Pattern.Infrastructure.ObjectState.Added;
                    manual.Version = 1;
                    _manualService.Add(manual);
                }
                foreach (var method in equipment.Methods)
                {
                    method.EQID = equipment.EQID;
                    method.Version = 1;
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

            }
            return operationResult;
        }

        public OperationResult DeleteEquipment(Equipment equipment)
        {
            try
            {
                equipment.ObjectState = Pattern.Infrastructure.ObjectState.Modified;
                equipment.State = "X";// Delete Voucher
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

            }
            return operationResult;
        }

        [Obsolete]
        public DataTable GetData()
        {
            var data = gateIM.ExecuteStoredProcedure("", null, null).Tables[0];
            return data;
        }

        public OperationResult UpdateEquipment(Equipment equipment)
        {
            try
            {
                equipment.ObjectState = Pattern.Infrastructure.ObjectState.Modified;
                equipment.State = "M";//Modified Equipment
                _equipmentService.Update(equipment);
                foreach (var manual in equipment.Manuals)
                {
                    manual.ObjectState = Pattern.Infrastructure.ObjectState.Modified;
                    _manualService.Update(manual);
                }
                foreach (var method in equipment.Methods)
                {
                    method.ObjectState = Pattern.Infrastructure.ObjectState.Modified;
                    _methodService.Update(method);
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

            }
            return operationResult;
        }
    }
}
