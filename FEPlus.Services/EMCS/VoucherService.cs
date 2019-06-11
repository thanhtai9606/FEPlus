using FEPlus.Contract.EMCS;
using FEPlus.Models;
using FEPlus.Pattern.Repositories;
using FEPlus.Pattern.UnitOfWork;
using FEPlus.Utility;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FEPlus.Services.EMCS
{
    public class VoucherService : IVoucherService
    {
        private readonly IRepositoryAsync<Plans> _planService;
        private readonly IRepositoryAsync<Requisition> _requisitionService;
        private readonly IRepositoryAsync<Profile> _profileService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private NBear.Data.Gateway db = new NBear.Data.Gateway("EMCS");
        protected readonly ILog Loger = LogManager.GetLogger("HSSELogger");
        OperationResult operationResult = new OperationResult();
        public VoucherService(IRepositoryAsync<Plans> planService,
                            IRepositoryAsync<Requisition> requisitionService,
                            IRepositoryAsync<Profile> profileService,
                            IUnitOfWorkAsync unitOfWorkAsync)
        {
            _planService = planService;
            _requisitionService = requisitionService;
            _profileService = profileService;
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        [Obsolete]
        public OperationResult AddRequsitionVoucher(Requisition req)
        {
            try
            {
                var checkResult = db.SelectScalar<string>("select dbo.CheckVoucher(@eqid,@month,@year,@VoucherID )", new object[] { req.EQID, req.MonthAdjust, req.YearAdjust, req.VoucherID });
                if (checkResult != null) return new OperationResult("Existsed plans!", checkResult, false);
                var newVoucherID = db.SelectScalar<string>("select dbo.GenNewID(@table,@id)", new object[] { "Requisition", "" });
                req.VoucherID = newVoucherID;

                req.ObjectState = Pattern.Infrastructure.ObjectState.Added;
                req.CreateTime = DateTime.Now;
                req.Stamp = DateTime.Now;
                _requisitionService.Add(req);
                if (req.Profiles != null)
                    foreach (var jobrequisition in req.Profiles)
                    {
                        jobrequisition.VoucherID = newVoucherID;
                        jobrequisition.ObjectState = Pattern.Infrastructure.ObjectState.Added;
                        _profileService.Add(jobrequisition);
                    }

                _unitOfWorkAsync.SaveChanges();

                operationResult.Success = true;
                operationResult.Message = "Requist Successed!";
                operationResult.Caption = "Successed!";

            }
            catch (Exception ex)
            {
                operationResult.Success = false;
                operationResult.Message = ex.Message.ToString(); ;
                operationResult.Caption = "Error!";
                Loger.Error(ex);

            }
            var loginfo = String.Format("EMCS - AddRequsitionVoucher - {0}",  req.VoucherID);
            Console.WriteLine(loginfo);
            Loger.Info(loginfo);

            return operationResult;

        }

        [Obsolete]
        public Dictionary<string, object> FindVoucher(string voucherID)
        {
            var data = db.ExecuteStoredProcedure("FindVoucher", new string[] { "VoucherID" }, new object[] { voucherID });

            Dictionary<string, Object> result = new Dictionary<string, object>();
            result.Add("Header", data.Tables[0]);
            result.Add("Detail", data.Tables[1]);
            return result;
        }

        [Obsolete]
        public DataTable GetDataVoucher(string departid, string type, string year, string status)
        {
            Console.WriteLine("EMCS - GetVoucher");
            var data = db.ExecuteStoredProcedure("GetVoucher", new string[] { "DepartID", "Type", "Year", "Status" },
                        new object[] { departid, type, year, status }).Tables[0];
            return data;
        }


        public OperationResult DeleteProfiles(string VoucherID)
        {
            try
            {
                var Profiles = _profileService.FindBy(x => x.VoucherID == VoucherID).ToList();
                foreach (Profile item in Profiles)
                {
                    item.ObjectState = Pattern.Infrastructure.ObjectState.Deleted;
                    _profileService.Delete(item);
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

            }
            return operationResult;
        }

        [Obsolete]
        public OperationResult UpdateVouchers(Requisition req)
        {
            try
            {
                var checkResult = db.SelectScalar<string>("select dbo.CheckVoucher(@eqid,@month,@year, @VoucherID)", new object[] { req.EQID, req.MonthAdjust, req.YearAdjust, req.VoucherID });
                if (checkResult != null) return new OperationResult("Existsed plans!", checkResult, false);
                req.Stamp = DateTime.Now;
                req.ObjectState = Pattern.Infrastructure.ObjectState.Modified;
                if (req.Profiles != null)
                {
                    DeleteProfiles(req.VoucherID);
                    foreach (var jobrequisition in req.Profiles)
                    {
                        jobrequisition.VoucherID = req.VoucherID;
                        jobrequisition.Stamp = DateTime.Now;
                        jobrequisition.ObjectState = Pattern.Infrastructure.ObjectState.Added;
                        _profileService.Add(jobrequisition);
                    }
                }

                if (req.VoucherID.IndexOf("PE") >= 0)
                {
                    var plans = new Plans();
                    plans.VoucherID = req.VoucherID;
                    plans.UserID = req.UserID;
                    plans.EQID = req.EQID;
                    plans.Stamp = DateTime.Now;
                    plans.State = req.State;
                    plans.Profiles = req.Profiles;
                    plans.ObjectState = req.ObjectState;
                    _planService.Update(plans);
                }
                else _requisitionService.Update(req);
                _unitOfWorkAsync.SaveChanges();
                operationResult.Success = true;
                operationResult.Message = "Update Successed!";
                operationResult.Caption = "Successed!";

            }
            catch (Exception ex)
            {
                operationResult.Success = false;
                operationResult.Message = "There are some things wrong: " + ex.Message.ToString();
                operationResult.Caption = "Error!";
                Loger.Error(ex);

            }
            var loginfo = String.Format("EMCS - UpdateVouchers - {0}",  req.VoucherID);
            Console.WriteLine(loginfo);
            Loger.Info(loginfo);

            return operationResult;
        }


        [Obsolete]
        public OperationResult UpdateVoucherState(string VoucherID, string State)
        {
            try
            {
                if (VoucherID.IndexOf("PE") >= 0)
                {
                    var plans = _planService.Find(VoucherID);
                    plans.State = State;
                    plans.ObjectState = Pattern.Infrastructure.ObjectState.Modified;
                    _planService.Update(plans);
                }
                else
                {
                    var requistion = _requisitionService.Find(VoucherID);
                    requistion.State = State;
                    requistion.ObjectState = Pattern.Infrastructure.ObjectState.Modified;
                    _requisitionService.Update(requistion);
                }
                _unitOfWorkAsync.SaveChanges();
                operationResult.Success = true;
                operationResult.Message = "Successed!";
                operationResult.Caption = "Successed!";
            }
            catch (Exception ex)
            {
                operationResult.Success = false;
                operationResult.Message = "There are some things wrong: " + ex.Message.ToString();
                operationResult.Caption = "Error!";
                Loger.Error(ex);

            }
            var loginfo = String.Format("EMCS - UpdateVoucherState - {0}: Voucher {1} with state {2}", new object[] {  VoucherID, State });
            Console.WriteLine(loginfo);
            Loger.Info(loginfo);
            return operationResult;
        }

        public OperationResult DeleteVoucher(string voucherid)
        {
            try
            {

                if (voucherid.IndexOf("PE") >= 0)
                {
                    var plans = _planService.Find(voucherid);
                    plans.Stamp = DateTime.Now;
                    plans.State = "X";
                    plans.ObjectState = Pattern.Infrastructure.ObjectState.Modified;
                    _planService.Update(plans);
                }
                else
                {
                    var req = _requisitionService.Find(voucherid);
                    req.Stamp = DateTime.Now;
                    req.State = "X";
                    req.ObjectState = Pattern.Infrastructure.ObjectState.Modified;
                    _requisitionService.Update(req);
                }
                _unitOfWorkAsync.SaveChanges();
                operationResult.Success = true;
                operationResult.Message = "Delete Successed!";
                operationResult.Caption = "Successed!";
            }
            catch (Exception ex)
            {
                operationResult.Success = false;
                operationResult.Message = "There are some things wrong: " + ex.Message.ToString();
                operationResult.Caption = "Error!";
                Loger.Error(ex);

            }
            var loginfo = String.Format("EMCS - DeleteVoucher - {0}: Voucher {1}", new object[] {  voucherid });
            Console.WriteLine(loginfo);
            Loger.Info(loginfo);
            return operationResult;
        }

    }
}
