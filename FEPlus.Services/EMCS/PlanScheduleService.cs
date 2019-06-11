using FEPlus.Contract.EMCS;
using FEPlus.Models;
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
    public class PlanScheduleService : IPlanScheduleService
    {
        private readonly IRepositoryAsync<PlanTimeJob> _planJobService;
        private readonly IRepositoryAsync<PlanTimeJob_Items> _planJobItemService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        OperationResult operationResult = new OperationResult();
        private NBear.Data.Gateway db = new NBear.Data.Gateway("EMCS");
        protected readonly ILog Loger = LogManager.GetLogger("HSSELogger");

        public PlanScheduleService(IRepositoryAsync<PlanTimeJob> planJobService, IRepositoryAsync<PlanTimeJob_Items> planJobItemService, IUnitOfWorkAsync unitOfWorkAsync)
        {
            _planJobItemService = planJobItemService;
            _planJobService = planJobService;
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        [Obsolete]
        public Dictionary<string, object> FindSchedulePlan(string EQId)
        {
            
            var data = db.ExecuteStoredProcedure("FindSchedulePlan", new string[] { "EQID" }, new object[] { EQId });
            Dictionary<string, Object> result = new Dictionary<string, object>();
            result.Add("Header", data.Tables[0]);
            result.Add("Detail", data.Tables[1]);
            return result;
        }

        [Obsolete]
        public DataTable GetData(string DepartId, string Type, string Year, string Lang)
        {
            Console.WriteLine("EMCS - GetSchedulePlan");
            var data = db.ExecuteStoredProcedure("GetSchedulePlan", new string[] { "DepartID", "Type", "Year","Lang" },
                 new object[] { DepartId, Type, Year, Lang }).Tables[0];
            return data;

        }

        public OperationResult CheckItemDetail(PlanTimeJob_Items item)
        {
            try
            {
                var updateResult = db.ExecuteStoredProcedure("PlanSchedule_AddTimeJobDetail", new string[] { "EQID", "Month", "Year" }, new object[] { item.EQID, item.Month, item.Year});
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
            return operationResult;
        }



        public OperationResult UpdatePlanTimeJob(PlanTimeJob planTimeJob)
        {
            Console.WriteLine("PlanScheduleService-UpdatePlanTimeJob({0}): {1}", planTimeJob.ToString(), DateTime.Now);
            try
            {
                planTimeJob.ObjectState = Pattern.Infrastructure.ObjectState.Modified;
                //var _OldJobItem = _planJobItemService.FindBy(x => x.EQID == planTimeJob.EQID && x.IsCreated == false);
                //foreach (var jobitem in _OldJobItem)
                //{
                //    jobitem.ObjectState = Pattern.Infrastructure.ObjectState.Deleted;
                //    _planJobItemService.Delete(jobitem);
                //}
                //foreach (var jobitem in planTimeJob.PlanTimeJob_Items)
                //{
                //    jobitem.ObjectState = Pattern.Infrastructure.ObjectState.Added;
                //    _planJobItemService.Add(jobitem);
                //}

                _planJobService.Update(planTimeJob);
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
            return operationResult;
        }

    }
}
